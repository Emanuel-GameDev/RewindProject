using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class RewindableAttack : State
{
    private float elapsed;
    private BossBheaviour bossBheaviour;
    bool readyToShoot;
    bool shooting;
    BossRewindableProjectile rewindable;
    

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
        //PubSub.Instance.RegisterFunction(EMessageType.TimeRewindStart, StartRewind);
        PubSub.Instance.RegisterFunction(EMessageType.TimeRewindStop, StopRewind);
        bossBheaviour.NonRewindableCountReset();
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
        SetAuraTrigger();
        shooting = true;
        elapsed = 0;
    }

    private void SetAuraTrigger()
    {
        bossBheaviour.GetRewindableAuraObject().transform.parent = bossBheaviour.GetTargetPlayer().transform;
        bossBheaviour.GetRewindableAuraObject().transform.localPosition = Vector3.zero;
        bossBheaviour.GetRewindableAuraObject().SetActive(true);
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

    //public void StartRewind(object obj)
    //{
    //    rewindable.StartRewind();
    //}

    public override void Exit()
    {
        //PubSub.Instance.UnregisterFunction(EMessageType.TimeRewindStart, StartRewind);
        PubSub.Instance.UnregisterFunction(EMessageType.TimeRewindStop, StopRewind);
        DismissAuraTrigger();
    }

    private void DismissAuraTrigger()
    {
        bossBheaviour.GetRewindableAuraObject().SetActive(false);
        bossBheaviour.GetRewindableAuraObject().transform.parent = bossBheaviour.transform;
    }

    private void StopRewind(object obj)
    {
        elapsed += bossBheaviour.GetRewindableLifeTime();
    }
}
