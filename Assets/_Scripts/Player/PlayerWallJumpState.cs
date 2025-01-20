public class PlayerWallJumpState : PlayerState
{
    public PlayerWallJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();

        player.SetVelocity(5 * player.facingDir, player.jumpForce);

        stateTimer = .3f;

    }

    public override void Exit()
    {
        base.Exit();
    }

    public override void Update()
    {
        base.Update();

        if (player.IsGroundDetected())
        {
            stateMachine.ChangeState(player.idleState);
        }

        if (stateTimer < 0)
        {
            player.SetVelocity(xInput * player.moveSpeed, rb.linearVelocity.y);

        }

        if (player.IsWallDetected() && stateTimer < 0)
        {

            stateMachine.ChangeState(player.wallSlideState);
        }





    }
}
