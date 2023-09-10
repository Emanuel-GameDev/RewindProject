using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeGroundAttack : State
{
    BossBheaviour bossBheaviour;
    BossGroundManager groundManager;
    float elapsedTime;
    Vector3 startPosition;
    Vector3 endPosition;
    bool moveBoss;

    public ChangeGroundAttack(BossBheaviour bossBheaviour)
    {
        this.bossBheaviour = bossBheaviour;
    }

    public override void Enter()
    {
        Debug.Log(this.GetType().Name);
        if (bossBheaviour.GetCurrentPosition().GetHorizontalPosition() == eHorizontalPosition.Center)
        {
            bossBheaviour.SetNextState(eBossState.ChangeGroundAttack);
            bossBheaviour.ChangeState(eBossState.Moving);
        }
        else
        {
            groundManager = bossBheaviour.GetBossGroundManager();
            groundManager.StartChangeGround();
            elapsedTime = 0;
            startPosition = bossBheaviour.GetCurrentPosition().transform.position;
            endPosition = bossBheaviour.GetOppositePosition().transform.position;
            bossBheaviour.SetCurrentPosition(bossBheaviour.GetOppositePosition());
            moveBoss = false;
        }
    }

    public override void Update()
    {
        bool changeState = groundManager.UpdateState();

        if (groundManager.IsInFadeOut())
        {
            moveBoss = true;
        }

        if (moveBoss)
        {
            elapsedTime += Time.deltaTime;
            if(elapsedTime < bossBheaviour.GetVerticalMoveDuration())
            {
                float t = elapsedTime / bossBheaviour.GetVerticalMoveDuration();
                bossBheaviour.GetBossBody().transform.position = Vector3.Lerp(startPosition, endPosition, t);
            }
            else
            {
                moveBoss = false;
            }
            
        }

        if (changeState && !moveBoss)
        {
            bossBheaviour.ResetChangeGroundCountdown();
            bossBheaviour.ChangeState();
        }
    }
}
