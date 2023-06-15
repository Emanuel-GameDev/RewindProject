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
        player.fallStartPoint = player.transform.position.y;
        player.transform.rotation = Quaternion.Euler(0, 0, 0);
        player.animator.SetTrigger("Fall");
        player.animator.SetBool("Falling",player.isFalling);
    }

    public override void Update()
    {
        player.CalculateHorizontalMovement();
        player.AbortJump();
        player.CalculateFallSpeed();
        player.CheckFriction();

        if (player.isJumping)
        {
            player.stateMachine.SetState(PlayerState.PlayerJumping);
        }

        if (!player.isFalling)
        {
            if (player.CheckMaxFallDistanceReached())
            {
                if (player.GetComponent<Damageable>() != null)
                {
                    if (player.GetComponent<Damageable>().Health > 1)
                        LevelManager.instance.FastRespawn();

                    player.GetComponent<Damageable>().TakeDamage(1);
                }
            }

            player.stateMachine.SetState(PlayerState.PlayerIdle);
        }

    }

    public override void Exit()
    {
        base.Exit();
        player.animator.SetBool("Falling", player.isFalling);
        player.fallStartPoint = 0;
    }
}
