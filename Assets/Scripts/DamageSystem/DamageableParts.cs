using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageableParts : Damageable
{
    private Damageable damageableMaster;

    public new void TakeDamage(int damage)
    {
        damageableMaster.TakeDamage(damage);
    }

    public new void Heal(int damage)
    {
        damageableMaster.Heal(damage);
    }

    private void Start()
    {
        damageableMaster = GetComponentInParent<Damageable>();
    }
}
