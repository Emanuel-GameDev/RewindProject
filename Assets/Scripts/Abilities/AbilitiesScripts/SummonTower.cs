using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonTower : Ability
{
    [SerializeField] GameObject towerPrefab;

    public override void Activate1(GameObject parent)
    {
        PlayerController player = parent.GetComponent<PlayerController>();

        // Evoco torre
        if (player.grounded && player != null)
            Instantiate(towerPrefab, player.gameObject.transform.position, player.gameObject.transform.rotation);
    }
}
