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
}
