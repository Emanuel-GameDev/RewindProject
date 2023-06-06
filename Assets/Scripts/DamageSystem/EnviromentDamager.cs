using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentDamager : Damager
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<Damageable>())
        {
            GameManager.Instance.levelMaster.FastRespawn();
            DealDamage(collision.gameObject.GetComponent<Damageable>());
        }
    }
}
