using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentDamager : Damager
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Damageable>())
        {
            if(collision.gameObject.GetComponent<Damageable>().Health<=1)
            LevelManager.instance.FastRespawn();

            DealDamage(collision.gameObject.GetComponent<Damageable>());

        }
    }
}
