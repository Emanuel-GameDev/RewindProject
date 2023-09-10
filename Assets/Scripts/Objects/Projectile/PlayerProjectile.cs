using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerProjectile : Projectile
{
    public override void Dismiss()
    {
        Destroy(gameObject);
    }
}
