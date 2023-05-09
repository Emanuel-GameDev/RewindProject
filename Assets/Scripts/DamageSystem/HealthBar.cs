using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthBar : MonoBehaviour
{
    [SerializeField]  Damageable damageable;
    [SerializeField] GameObject healtIconPrefab;

    List<HealthIcon> icons;

    private void Start()
    {
        for (int i = 0; i < damageable.maxHealth; i++)
        {
            Instantiate(healtIconPrefab,transform);
        }
    }

}
