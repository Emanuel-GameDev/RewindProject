using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UroboroAttack : State
{
    private float elapsed;
    private BossBheaviour bossBheaviour;
    bool spawned;
    float timeToChangeState;

    public UroboroAttack(BossBheaviour bossBheaviour)
    {
        this.bossBheaviour = bossBheaviour;
    }

    public override void Enter()
    {
        Debug.Log(this.GetType().Name);
        elapsed = 0;
        spawned = false;
        timeToChangeState = bossBheaviour.GetUroboroTimeChange();
        bossBheaviour.NonRewindableCountUpdate();
    }

    public override void Update()
    {
        elapsed += Time.deltaTime;

        if (elapsed > timeToChangeState)
        {
            bossBheaviour.ChangeState();
        }

        if(!spawned)
            SpawnUroboro();
    }

    private void SpawnUroboro()
    {
        Vector2 spawnPoint1 = Vector2.zero;
        Vector2 spawnPoint2 = Vector2.zero;

        foreach(BossPosition bp in bossBheaviour.GetPositions())
        {
            if (bossBheaviour.GetCurrentPosition().GetVerticalPosition() == eVerticalPosition.Top)
            {
                if (bp.GetVerticalPosition() == eVerticalPosition.Bottom)
                {
                    if(bp.GetHorizontalPosition() == eHorizontalPosition.Left)
                    {
                        spawnPoint1 = bp.transform.position;
                    }
                    else
                    {
                        spawnPoint2 = bp.transform.position;
                    }
                }
            }
            else
            {
                if (bp.GetVerticalPosition() == eVerticalPosition.Top)
                {
                    if (bp.GetHorizontalPosition() == eHorizontalPosition.Left)
                    {
                        spawnPoint1 = bp.transform.position;
                    }
                    else
                    {
                        spawnPoint2 = bp.transform.position;
                    }
                }
            }
        }

        float verticalOffset = bossBheaviour.GetUroboroVerticalSpawnOffset();
        float horizontalOffset = bossBheaviour.GetUroboroHorizontalSpawnOffset();
        spawnPoint1.x -= horizontalOffset;
        spawnPoint2.x += horizontalOffset;
        spawnPoint1.y += bossBheaviour.GetCurrentPosition().GetVerticalPosition() == eVerticalPosition.Top ? - verticalOffset : verticalOffset;
        spawnPoint2.y = spawnPoint1.y;
        EnemyThree uroboro1 = bossBheaviour.GetUroboro1().GetComponent<EnemyThree>();
        EnemyThree uroboro2 = bossBheaviour.GetUroboro2().GetComponent<EnemyThree>();
        bossBheaviour.GetUroboroEndPoint1().Set(uroboro1.gameObject,spawnPoint2);
        bossBheaviour.GetUroboroEndPoint2().Set(uroboro2.gameObject, spawnPoint1);
        uroboro1.SetStartPosition(spawnPoint1);
        uroboro2.SetStartPosition(spawnPoint2);
        uroboro1.SetTarget(bossBheaviour.GetUroboroEndPoint1().gameObject);
        uroboro2.SetTarget(bossBheaviour.GetUroboroEndPoint2().gameObject);
        uroboro1.SetSpeed(bossBheaviour.GetUroboroSpeed());
        uroboro2.SetSpeed(bossBheaviour.GetUroboroSpeed());
        uroboro1.Spawn();
        uroboro2.Spawn();

        spawned = true;
    }
}
