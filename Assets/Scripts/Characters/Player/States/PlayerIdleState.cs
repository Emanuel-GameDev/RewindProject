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
        player.animator.SetBool("Moving", false);
    }

    public override void Update()
    {
        player.CalculateHorizontalMovement();
        player.CalculateFallSpeed();
        player.CheckRotation();

        
        if (player.isJumping)
            player.stateMachine.SetState(PlayerState.PlayerJumping);

        if(player.isMoving)
            player.stateMachine.SetState(PlayerState.PlayerMooving);

        if (player.isFalling)
            player.stateMachine.SetState(PlayerState.PlayerFalling);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
