using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using UnityEngine.InputSystem;

public class AbilityWheel : MonoBehaviour
{
    [SerializeField] Transform rotPoint;
    [SerializeField] Transform selectionSocket;
    [SerializeField] float rotDuration;
    [SerializeField] float rotAngle = 45.0f;

    private List<Transform> _sockets = new List<Transform>();
    private List<Transform> _images = new List<Transform>();
    private List<Ability> _abilitiesUnlocked = new List<Ability>();

    private int activeSockets = 0;
    private float currentRot;
    private bool isRotating = false;

    private PlayerInputs inputs;

    private void Start()
    {
        PubSub.Instance.RegisterFunction(EMessageType.AbilityPicked, AddToWheel);
        SetupLists();
    }

    private void OnEnable()
    {
        inputs = new PlayerInputs();
        inputs.UI.Enable();

        inputs.UI.ScrollWheel.performed += ScrollInput;
    }

    private void OnDisable()
    {
        inputs.UI.Disable();

        inputs.UI.ScrollWheel.performed -= ScrollInput;
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

    IEnumerator Rotate(float duration, float rotation)
    {
        isRotating = true;
        float startRotation = rotPoint.GetComponent<RadialLayout>().StartAngle;
        Debug.Log(startRotation);
        float endRotation = startRotation + rotation;
        float t = 0.0f;
        while (t < duration)
        {
            t += Time.deltaTime;
            float zRotation = Mathf.Lerp(startRotation, endRotation, t / duration) % rotation;
            rotPoint.GetComponent<RadialLayout>().StartAngle = zRotation;
            rotPoint.GetComponent<RadialLayout>().UpdateLayout();
            //_sockets[0].eulerAngles = new Vector3(_sockets[0].eulerAngles.x, _sockets[0].eulerAngles.y,
            //zRotation);

            yield return null;
        }
        rotPoint.GetComponent<RadialLayout>().StartAngle = endRotation;
        rotPoint.GetComponent<RadialLayout>().UpdateLayout();
        isRotating = false;
    }

    private void ScrollInput(InputAction.CallbackContext scroll)
    {
        if (scroll.ReadValue<Vector2>().y > 0f)
        {
            StartCoroutine(Rotate(rotDuration, rotAngle));
        }
        else if (scroll.ReadValue<Vector2>().y < 0f)
        {
            StartCoroutine(Rotate(rotDuration, -rotAngle));
        }
    }

    //private void Update()
    //{
    //    float scroll = Input.GetAxis("Mouse ScrollWheel");

    //    //if (_abilitiesUnlocked.Count <= 2) return;

    //    // Check mouse scroll
    //    if (scroll > 0f && !isRotating)
    //    {
    //        //currentIndex = (currentIndex + 1) % activeSockets;
    //        //for (int i = 0; i < activeSockets; i++)
    //        //{
    //        //    _images[i].SetParent(_sockets[(currentIndex + i) % activeSockets], false);

    //        //    // Notify holders if current active ability changed
    //        //    if (_sockets[(currentIndex + i) % activeSockets] == selectionSocket)
    //        //        PubSub.Instance.Notify(EMessageType.ActiveAbilityChanged, _abilitiesUnlocked[i]);
    //        //}

    //        StartCoroutine(Rotate(rotDuration, rotAngle));
    //    }
    //    else if (scroll < 0f && !isRotating)
    //    {
    //        //currentIndex = (currentIndex + activeSockets - 1) % activeSockets;

    //        //for (int i = 0; i < activeSockets; i++)
    //        //{
    //        //    _images[(currentIndex + i) % activeSockets].SetParent(_sockets[activeSockets - 1 - i], false);

    //        //    // Notify holders if current active ability changed
    //        //    if (_sockets[activeSockets - 1 - i] == selectionSocket)
    //        //        PubSub.Instance.Notify(EMessageType.ActiveAbilityChanged, _abilitiesUnlocked[(currentIndex + i) % activeSockets]);
    //        //}

    //        //StartCoroutine(Rotate(rotDuration, -rotAngle));
    //    }
}
