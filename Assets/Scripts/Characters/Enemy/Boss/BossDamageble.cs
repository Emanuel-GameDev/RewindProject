using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossDamageble : Damageable
{
    [SerializeField] int healthTreshold = 50;
    public override void TakeDamage(int healthToRemove)
    {
        if(healthToRemove > healthTreshold)
        {
            base.TakeDamage(1);
        }
    }
}
