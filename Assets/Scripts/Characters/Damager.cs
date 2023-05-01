using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Damager : MonoBehaviour
{
    [SerializeField] int damage = 1;

    public void SetDamage(int dmg)
    {
        damage = dmg;
    }

    private void HitDamageable(Damageable damageable)
    {
        damageable.TakeDamage(damage);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger)
            return;
        
        Damageable damageable = collision.gameObject.GetComponent<Damageable>();

        if (damageable != null)
            HitDamageable(damageable);

        HitSomethings(collision.gameObject);
    }

    private void HitSomethings(GameObject gameObject)
    {
        //Destroy(this);
    }
}
