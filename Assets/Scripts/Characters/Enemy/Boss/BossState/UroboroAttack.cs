using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UroboroAttack : State
{


    private float elapsed;
    private BossBheaviour bossBheaviour;

    public UroboroAttack(BossBheaviour bossBheaviour)
    {
        this.bossBheaviour = bossBheaviour;
    }

    public override void Enter()
    {
        Debug.Log(this.GetType().Name);
        elapsed = 0;
    }

    public override void Update()
    {
        elapsed += Time.deltaTime;

        if (elapsed > 2)
        {
            bossBheaviour.ChangeState();
        }

    }
}
