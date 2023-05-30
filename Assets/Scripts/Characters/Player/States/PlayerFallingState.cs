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
    }

    public override void Update()
    {
        player.CalculateHorizontalMovement();
        player.AbortJump();
        player.CalculateFallSpeed();


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
                    player.GetComponent<Damageable>().TakeDamage(1);
                    GameManager.Instance.levelMaster.FastRespawn();

                }
            }

            player.stateMachine.SetState(PlayerState.PlayerIdle);
        }

    }

    public override void Exit()
    {
        base.Exit();
    }
}
