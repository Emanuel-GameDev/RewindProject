using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityWheel : MonoBehaviour
{
    [SerializeField] Transform rotPoint;
    [SerializeField] Transform selectionSocket;

    private List<Transform> _sockets = new List<Transform>();
    private List<Transform> _images = new List<Transform>();
    private List<Ability> _abilitiesUnlocked = new List<Ability>();

    private int currentIndex = 0;
    private int activeSockets = 0;

    private void Start()
    {
        PubSub.Instance.RegisterFunction(EMessageType.AbilityPicked, AddToWheel);
        SetupLists();
    }
    
    private void SetupLists()
    {
        // Ref to sockets
        for (int i = 0; i < rotPoint.childCount; i++)
        {
            _sockets.Add(rotPoint.GetChild(i));

            // Check if socket is visible
            if (rotPoint.GetChild(i).gameObject.activeSelf)
                activeSockets++;
        }

        // Ref to images
        foreach (Transform t in _sockets)
        {
            _images.Add(t.GetChild(0));
        }
    }

    private void AddToWheel(object obj)
    {
        if (obj == null || obj is not Ability) return;
        Ability ability = (Ability)obj;

        for (int i = 0; i < _sockets.Count; i++)
        {
            // Check for buffer socket
            if (!_sockets[i].gameObject.activeSelf)
            {
                _sockets[i].gameObject.SetActive(true);
                activeSockets++;
            }

            Image socketImage = _sockets[i].GetChild(0).GetComponent<Image>();
            if (socketImage.sprite == null)
            {
                // Set icon
                socketImage.sprite = ability.icon;

                // Set alpha
                Color tempColor = socketImage.color;
                tempColor.a = 1f;
                socketImage.color = tempColor;

                // Add to list
                ability.unlocked = true;
                _abilitiesUnlocked.Add(ability);

                // Notify holders if ability has been loaded in active socket
                if (_sockets[i] == selectionSocket)
                    PubSub.Instance.Notify(EMessageType.ActiveAbilityChanged, ability);

                break;
            }
        }
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        if (_abilitiesUnlocked.Count <= 2) return;

        // Check mouse scroll
        if (scroll > 0f)
        {
            currentIndex = (currentIndex + 1) % activeSockets;
            for (int i = 0; i < activeSockets; i++)
            {
                _images[i].SetParent(_sockets[(currentIndex + i) % activeSockets], false);

                // Notify holders if current active ability changed
                if (_sockets[(currentIndex + i) % activeSockets] == selectionSocket)
                    PubSub.Instance.Notify(EMessageType.ActiveAbilityChanged, _abilitiesUnlocked[i]);
            }
        }
        else if (scroll < 0f)
        {
            currentIndex = (currentIndex + activeSockets - 1) % activeSockets;

            for (int i = 0; i < activeSockets; i++)
            {
                _images[(currentIndex + i) % activeSockets].SetParent(_sockets[activeSockets - 1 - i], false);

                // Notify holders if current active ability changed
                if (_sockets[activeSockets - 1 - i] == selectionSocket)
                    PubSub.Instance.Notify(EMessageType.ActiveAbilityChanged, _abilitiesUnlocked[(currentIndex + i) % activeSockets]);
            }
        }
    }
}
