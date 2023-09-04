using BehaviorDesigner.Runtime.Tasks.Unity.UnityNavMeshAgent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : State
{
    BossBheaviour bossBheaviour;
    float timeToWait = 5f;
    float elapsed;
    Vector2 destination;

    public Falling(BossBheaviour bossBheaviour)
    {
        this.bossBheaviour = bossBheaviour;
    }

    public override void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed > timeToWait)
        {
            bossBheaviour.ChangeState(eBossState.Stunned);
        }
    }

    public override void Enter()
    {
        Debug.Log(this.GetType().Name);
        elapsed = 0;
        timeToWait = bossBheaviour.GetFallingDuration();
        SetArrivePoint();
        
    }

    private void SetArrivePoint()
    {
        
    }
}
