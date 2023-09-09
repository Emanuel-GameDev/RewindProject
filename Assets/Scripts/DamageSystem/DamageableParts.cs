using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class DamageableParts : Damageable
{
    private Damageable damageableMaster;

    public new void TakeDamage(int damage, Damager damager)
    {
        damageableMaster.TakeDamage(damage, damager);
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
