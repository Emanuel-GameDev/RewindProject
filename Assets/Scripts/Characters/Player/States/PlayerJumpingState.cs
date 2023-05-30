using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerJumpingState : State
{
    private PlayerController player;

    public PlayerJumpingState(PlayerController player)
    {
        this.player = player;
    }

    public override void Enter()
    {
        base.Enter();
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
    }

    public override void Update()
    {
        player.CalculateHorizontalMovement();
        player.AbortJump();
        player.CalculateFallSpeed();

        

        if (player.isFalling)
            player.stateMachine.SetState(PlayerState.PlayerFalling);

    }

    public override void Exit()
    {
        base.Exit();
    }
}
