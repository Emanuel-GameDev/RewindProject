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

        player.animator.SetBool("Jumping",player.isJumping);
        player.animator.SetTrigger("Jump");
    }

    public override void Update()
    {
        player.CalculateHorizontalMovement();
        player.AbortJump();
        player.CalculateFallSpeed();
        player.CheckFriction();
        //Debug.Log(this);

        if (player.isFalling)
            player.stateMachine.SetState(PlayerState.PlayerFalling);

        if(player.grounded && player.rBody.velocity.y>-0.1f && player.rBody.velocity.y < 0.1f)
            player.stateMachine.SetState(PlayerState.PlayerIdle);
    }

    public override void Exit()
    {
        base.Exit();
        player.animator.SetBool("Jumping", player.isJumping);
    }
}
