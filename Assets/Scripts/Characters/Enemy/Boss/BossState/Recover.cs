using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Recover : State
{
    BossBheaviour bossBheaviour;
    float recoverTime = 5f;
    float elapsed;
    Vector2 startPosition;
    Vector2 endPosition;
    BossPosition recoverPosition;

    public Recover(BossBheaviour bossBheaviour)
    {
        this.bossBheaviour = bossBheaviour;
    }
    public override void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed > recoverTime)
        {
            bossBheaviour.SetCurrentPosition(recoverPosition);
            bossBheaviour.ChangeState();
        }
        else
        {
            float t = elapsed / recoverTime;
            bossBheaviour.GetBossBody().transform.position = Vector3.Lerp(startPosition, endPosition, t);
        }
    }
    public override void Enter()
    {
        Debug.Log(this.GetType().Name);
        elapsed = 0;
        recoverTime = bossBheaviour.GetRecoverTime();
        startPosition = bossBheaviour.GetBossBody().transform.position;
        SetRecoverPosition();
        endPosition = recoverPosition.transform.position;
        bossBheaviour.RecoverTrigger();
    }

    private void SetRecoverPosition()
    {
        eVerticalPosition actualVerticalPosition = bossBheaviour.GetCurrentPosition().GetVerticalPosition();
        foreach(BossPosition bp in bossBheaviour.GetPositions())
        {
            if(bp.GetHorizontalPosition() == eHorizontalPosition.Center)
            {
                if(bp.GetVerticalPosition() == actualVerticalPosition)
                {
                    recoverPosition = bp;
                }
            }
        }
        
    }
}

