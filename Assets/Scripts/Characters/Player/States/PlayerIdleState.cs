using System.Collections;
using System.Collections.Generic;
using UnityEngine;

internal class PlayerIdleState : State
{
    private PlayerController player;
    public PlayerIdleState(PlayerController player)
    {
        this.player = player;
    }

    public override void Enter()
    {
        base.Enter();
    }

    public override void Update()
    {
        base.Update();
    }

    public override void Exit()
    {
        base.Exit();
    }
}
