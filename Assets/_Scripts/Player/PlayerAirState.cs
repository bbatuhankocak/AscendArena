using UnityEngine;

public class PlayerAirState : PlayerState
{
    public PlayerAirState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();
        stateTimer = 1f;
    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        player.SetVelocity(xInput * player.moveSpeed, rb.linearVelocityY);

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
            Debug.Log("1");
        }

        if (player.IsWallDetected() && stateTimer < 0f)
        {
            stateMachine.ChangeState(player.wallSlideState);
            Debug.Log("2");
        }

    }
}
