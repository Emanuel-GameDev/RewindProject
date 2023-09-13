using BehaviorDesigner.Runtime.Tasks.Unity.UnityNavMeshAgent;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Falling : State
{
    BossBheaviour bossBheaviour;
    float fallingDuration = 5f;
    float elapsed;
    Vector3 startPosition;
    Vector2 destination;

    public Falling(BossBheaviour bossBheaviour)
    {
        this.bossBheaviour = bossBheaviour;
    }

    public override void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed > fallingDuration)
        {
            bossBheaviour.ChangeState(eBossState.Stunned);
        }
        else
        {
            float t = elapsed / fallingDuration;
            bossBheaviour.GetBossBody().transform.position = Vector3.Lerp(startPosition, destination, t);
        }
    }

    public override void Enter()
    {
        Debug.Log(this.GetType().Name);
        elapsed = 0;
        fallingDuration = bossBheaviour.GetFallingDuration();
        SetArrivePoint();
        startPosition = bossBheaviour.GetBossBody().transform.position;
        bossBheaviour.StunTrigger();
    }

    private void SetArrivePoint()
    {
        destination = bossBheaviour.GetOppositePosition().transform.position;
        destination.y += bossBheaviour.GetCurrentPosition().GetVerticalPosition() == eVerticalPosition.Top ? - bossBheaviour.GetFallingVerticalOffset() : + bossBheaviour.GetFallingVerticalOffset();
    }
}
