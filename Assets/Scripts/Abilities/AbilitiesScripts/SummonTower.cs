using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SummonTower : Ability
{
    public override void Activate(GameObject parent)
    {
        base.Activate(parent);
        Debug.Log(name);
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
