using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dead : State
{
    BossBheaviour bossBheaviour;

    public Dead(BossBheaviour bossBheaviour)
    {
        this.bossBheaviour = bossBheaviour;
    }

    public override void Update()
    {
    }

    public override void Enter()
    {
        Debug.Log(this.GetType().Name);
        bossBheaviour.DeathTrigger();
    }

    public override void Exit()
    {
        
    }

}
