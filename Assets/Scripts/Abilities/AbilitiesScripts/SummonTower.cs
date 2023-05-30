using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonTower : Ability
{
    [SerializeField] GameObject towerPrefab;

    public override void Activate(GameObject parent)
    {
        PlayerController player = parent.GetComponent<PlayerController>();

        // Evoco torre
        if (player.grounded && player != null)
            Instantiate(towerPrefab, player.gameObject.transform.position, player.gameObject.transform.rotation);
    }

    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
    }

    public override void Start()
    {
        base.Start();
    }
}
