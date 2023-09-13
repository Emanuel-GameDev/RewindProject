using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Stunned : State
{
    BossBheaviour bossBheaviour;
    float timeToWait = 5f;
    float elapsed;

    public Stunned(BossBheaviour bossBheaviour)
    {
        this.bossBheaviour = bossBheaviour;
    }
    public override void Update()
    {
        elapsed += Time.deltaTime;
        if (elapsed > timeToWait)
        {
            bossBheaviour.ChangeState(eBossState.Recover);
        }
    }
    public override void Enter()
    {
        Debug.Log(this.GetType().Name);
        elapsed = 0;
        timeToWait = bossBheaviour.GetStunnedTime();
    }

}