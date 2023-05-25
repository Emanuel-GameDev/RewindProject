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
        player.CalculateHorizontalMovement();
        player.CalculateFallSpeed();

        

        if (player.isJumping)
            player.stateMachine.SetState(PlayerState.PlayerJumping);

        if(player.isMooving)
            player.stateMachine.SetState(PlayerState.PlayerMooving);

        if (player.isFalling)
            player.stateMachine.SetState(PlayerState.PlayerFalling);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
