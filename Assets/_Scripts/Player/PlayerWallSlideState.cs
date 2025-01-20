using UnityEngine;

public class PlayerWallSlideState : PlayerState
{
    public PlayerWallSlideState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 2f;

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();
        stateTimer -= Time.deltaTime;

        player.SetVelocity(rb.linearVelocityX, rb.linearVelocityY * 0.5f);

        bool wallOnRight = player.IsWallOnRight();

        if ((wallOnRight && player.facingRight) || (!wallOnRight && !player.facingRight))
        {
            player.Flip();
        }




        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            stateMachine.ChangeState(player.wallJumpState);
        }
        
        if (stateTimer < 0f)
        {
            stateMachine.ChangeState(player.airState);
        }

        // Debug.Log(player.IsWallOnRight() + " " + player.facingRight);



    }
}
