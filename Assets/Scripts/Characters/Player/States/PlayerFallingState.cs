using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerFallingState : State
{
    private PlayerController player;
    public PlayerFallingState(PlayerController player)
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

        

        if (!player.isFalling)
            player.stateMachine.SetState(PlayerState.PlayerIdle);

        if (player.isJumping)
            player.stateMachine.SetState(PlayerState.PlayerJumping);
    }

    public override void Exit()
    {
        base.Exit();
    }
}
