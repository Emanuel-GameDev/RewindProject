using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindableAttack : State
{
    private float elapsed;
    private BossBheaviour bossBheaviour;
    bool readyToShoot;
    bool shooting;
    BossProjectile rewindable;

    public RewindableAttack(BossBheaviour bossBheaviour)
    {
        this.bossBheaviour = bossBheaviour;
    }

    public override void Enter()
    {
        Debug.Log(this.GetType().Name);
        elapsed = 0;
        readyToShoot = false;
        shooting = false;
    }

    public override void Update()
    {
        elapsed += Time.deltaTime;

        if (shooting)
        {
            if (elapsed > bossBheaviour.GetRewindableLifeTime())
            {
                bossBheaviour.ChangeState();
            }
        }
        else if(readyToShoot)
        {
            if(elapsed > bossBheaviour.GetRewindableWaitBeforeShoot())
            {
                Shoot();
            }
        }
        else if(elapsed > bossBheaviour.GetRewindableWaitBeforeSpawn())
        {
            SpawnRewindable();
        }
        
    }

    private void Shoot()
    {
        Vector2 direction = (bossBheaviour.GetTargetPlayer().transform.position - rewindable.transform.position).normalized;
        rewindable.Inizialize(direction, rewindable.transform.position, bossBheaviour.GetRewindableSpeed());

        shooting = true;
        elapsed = 0;
    }

    private void SpawnRewindable()
    {
        Vector2 spawnPosition = bossBheaviour.GetCurrentPosition().transform.position;
        float offset = bossBheaviour.GetRewindableVerticalOffset();
        spawnPosition.y += bossBheaviour.GetCurrentPosition().GetVerticalPosition() == eVerticalPosition.Top ? - offset : offset;
        rewindable = bossBheaviour.GenerateRewindable(spawnPosition);
        readyToShoot = true;

        elapsed = 0;
    }
}
