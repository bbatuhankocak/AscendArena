using UnityEngine;

public class PlayerJumpState : PlayerState
{

    private int jumpCount;
    public PlayerJumpState(Player _player, PlayerStateMachine _stateMachine, string _animBoolName) : base(_player, _stateMachine, _animBoolName)
    {
    }

    public override void Enter()
    {
        base.Enter();



        rb.linearVelocity = new Vector2(rb.linearVelocityX, player.jumpForce);
        jumpCount++;


        stateTimer = .2f;

    }

    public override void Exit()
    {
        base.Exit();

        jumpCount = 0;
    }

    public override void Update()
    {
        base.Update();



        player.SetVelocity(xInput * player.moveSpeed, rb.linearVelocityY);

        if (rb.linearVelocityY < 0)
        {
            stateMachine.ChangeState(player.airState);
        }

        if (jumpCount < player.maxJumps && Input.GetKeyDown(KeyCode.Space))
        {
            rb.linearVelocity = new Vector2(rb.linearVelocityX, player.jumpForce);
            jumpCount++;
        }

        if (player.IsWallDetected())
        {
            stateMachine.ChangeState(player.wallSlideState);
        }

        
    }
}
