using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class AbilityWheel : MonoBehaviour
{
    [SerializeField] Transform rotPoint;

    private List<Transform> _sockets = new List<Transform>();
    private List<Ability> _abilitiesUnlocked = new List<Ability>();

    private int currentIndex = 0;

    private void Start()
    {
        _abilitiesUnlocked = GameManager.Instance.abilityManager.GetUnlockedAbilities();
        LoadAbilities();
    }

    private void LoadAbilities()
    {
        // Ref to sockets
        for (int i = 0; i < rotPoint.childCount; i++)
        {
            _sockets.Add(rotPoint.GetChild(i));
        }

        // Load abilities
        for (int i = 0; i < _abilitiesUnlocked.Count; i++)
        {
            Ability spawnedAbility = Instantiate(_abilitiesUnlocked[i], _sockets[i]);
            spawnedAbility.GetComponent<RectTransform>().sizeDelta = _sockets[i].GetComponent<RectTransform>().sizeDelta;

            // Replace _abilities with abilities spawned, otherwise we will modify prefab
            _abilitiesUnlocked.RemoveAt(i);
            _abilitiesUnlocked.Insert(i, spawnedAbility);
        }
    }

    private void Update()
    {
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // Check mouse scroll
        if (scroll != 0f)
        {
            if (scroll > 0.1)
            {
                for (int i = 0; i < _abilitiesUnlocked.Count; i++)
                {
                    int tmp = (currentIndex + i + _sockets.Count) % _sockets.Count;
                    _abilitiesUnlocked[i].transform.SetParent(_sockets[tmp], false);
                    _abilitiesUnlocked[i].GetComponent<RectTransform>().sizeDelta = _abilitiesUnlocked[i].transform.parent.GetComponent<RectTransform>().sizeDelta;
                }
                currentIndex++;
            }
            else if (scroll < -0.1)
            {
                for (int i = 0; i < _abilitiesUnlocked.Count; i++)
                {
                    // Non sicuro di questa linea
                    int tmp = ((currentIndex - i + _sockets.Count) % _sockets.Count + _sockets.Count) % _sockets.Count;
                    _abilitiesUnlocked[i].transform.SetParent(_sockets[tmp], false);
                    _abilitiesUnlocked[i].GetComponent<RectTransform>().sizeDelta = _abilitiesUnlocked[i].transform.parent.GetComponent<RectTransform>().sizeDelta;
                }
                // NOn sicuro di questa linea
                currentIndex = ((currentIndex - 1 + _sockets.Count) % _sockets.Count + _sockets.Count) % _sockets.Count;
            }
            Debug.Log(currentIndex);
        }

    }

    private int Mod(int a, int n)
    {
        return ((a % n) + n) % n;
    }
}
