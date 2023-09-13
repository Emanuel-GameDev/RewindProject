using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossProjectile : Projectile
{
    private bool spawned = false;
    
    public override void Dismiss()
    {
        Destroy(gameObject);
    }

    public override void Inizialize(Vector2 direction, Vector2 position, float speed)
    {
        this.direction = direction;
        this.speed = speed;
        elapsedTime = 0;
        if (!spawned)
        {
            animator.SetTrigger("Spawn");
            spawned = true;
        }
    }



}
