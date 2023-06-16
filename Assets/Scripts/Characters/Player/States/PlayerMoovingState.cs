using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMoovingState : State
{
    private PlayerController player;
    public PlayerMoovingState(PlayerController player)
    {
        this.player = player;
    }

    public override void Enter()
    {
        base.Enter();
        player.animator.SetBool("Moving", true);
    }

    public override void Update()
    {
        player.CalculateHorizontalMovement();
        player.CalculateFallSpeed();
        player.CheckRotation();


        if (!player.isMooving)
            player.stateMachine.SetState(PlayerState.PlayerIdle);

        if (player.isJumping)
            player.stateMachine.SetState(PlayerState.PlayerJumping);

        if (player.isFalling)
            player.stateMachine.SetState(PlayerState.PlayerFalling);

    }

    public override void Exit()
    {
        base.Exit();
        player.animator.SetBool("Moving", false);
    }
}
