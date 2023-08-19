using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Start : State
{
    BossBheaviour bossBheaviour;
    float timeToWait = 5f;
    float elapsed;


    public Start(BossBheaviour bossBheaviour)
    {
        this.bossBheaviour = bossBheaviour;
    }

    public override void Update()
    {
        elapsed += Time.deltaTime;
        if(elapsed > timeToWait)
        {
            bossBheaviour.ChangeState();
        }
    }

    public override void Enter()
    {
        elapsed = 0;
    }

}
