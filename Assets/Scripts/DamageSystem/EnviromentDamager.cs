using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnviromentDamager : Damager
{
    public override void OnTriggerEnter2D(Collider2D collision)
    {
        base.OnTriggerEnter2D(collision);
        GameManager.Instance.levelMaster.FastRespawn();
    }
}
