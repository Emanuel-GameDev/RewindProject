using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Moving : State
{
    BossBheaviour bossBheaviour;
    Vector3 startPosition;
    Vector3 endPosition;
    float elapsedTime;
    BossPosition targetPosition;
    
    public Moving(BossBheaviour bossBheaviour)
    {
        this.bossBheaviour = bossBheaviour;
    }

    public override void Enter()
    {
        startPosition = bossBheaviour.GetCurrentPosition().transform.position;
        endPosition = NextPosition();
        elapsedTime = 0f;
    }

    private Vector3 NextPosition()
    {
        List<BossPosition> positions = new();
        foreach(BossPosition p in bossBheaviour.GetPositions())
        {
            if(!Vector3.Equals(p.transform, bossBheaviour.GetCurrentPosition().transform))
                if (Enum.Equals(p.GetPosition(), bossBheaviour.GetCurrentPosition().GetPosition()))
                {
                    positions.Add(p);
                }
        }

        targetPosition = positions[Random.Range(0, positions.Count)];

        Vector3 destination = targetPosition.transform.position;

        return destination;

    }

    public override void Update()
    {
        float moveTime = bossBheaviour.GetMoveDuration();

        elapsedTime += Time.deltaTime;

        if (elapsedTime <= moveTime)
        {
            float t = elapsedTime / moveTime;
            bossBheaviour.GetBossBody().transform.position = Vector3.Lerp(startPosition, endPosition, t);
        }
        else
        {
            bossBheaviour.SetCurrentPosition(targetPosition);
            bossBheaviour.ChangeState(eBossState.Start);
        }
    }

}
