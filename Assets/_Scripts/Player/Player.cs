using Photon.Pun;
using UnityEngine;
using Photon.Realtime;

public class Player : MonoBehaviour
{
    [SerializeField] PhotonView PV;

    PlayerStateMachine stateMachine;
    public Rigidbody2D rb {  get; private set; }
    public SpriteRenderer sr { get; private set; }
    public Animator anim {  get; private set; }





    public PlayerState idleState { get; private set; }
    public PlayerState moveState { get; private set; }
    public PlayerState jumpState { get; private set; }
    public PlayerState airState { get; private set; }
    public PlayerState attackState { get; private set; }
    public PlayerState wallSlideState { get; private set; }
    public PlayerState wallJumpState { get; private set; }
    public PlayerState dashState { get; private set; }




    public int facingDir { get; private set; } = 1;
    public bool facingRight = true;




    [Header("Collision")]
    [SerializeField] private Transform groundCheck;
    [SerializeField] private float groundCheckDistance;
    [SerializeField] private LayerMask groundCheckLayerMask;
    [SerializeField] private Transform attackPoint;
    [SerializeField] private float attackCheckRadius;
    [SerializeField] private Transform wallCheck;
    [SerializeField] private float wallCheckDistance;
    [SerializeField] private Transform wallSelection;
    [SerializeField] private float wallSelectionDistance;



    [Header ("Movement")]
    public float moveSpeed = 5f;
    public float jumpForce = 10f;
    public float maxJumps = 2;
    public Vector2 attackMovement;

    [Header("Dash")]
    public float dashSpeed;
    public float dashDuration;
    public float dashCount;



    [Header("Stats")]
    public float health;
    public float stamina;


    private void Awake()
    {
        

        stateMachine = new PlayerStateMachine();

        rb = GetComponent<Rigidbody2D>();
        sr = GetComponent<SpriteRenderer>();
        anim = GetComponent<Animator>();



        idleState = new PlayerIdleState(this, stateMachine, "idle");
        moveState = new PlayerMoveState(this, stateMachine, "run");
        jumpState = new PlayerJumpState(this, stateMachine, "jump");
        airState = new PlayerAirState(this, stateMachine, "air");
        attackState = new PlayerAttackState(this, stateMachine, "attack");
        wallSlideState = new PlayerWallSlideState(this, stateMachine, "wallSlide");
        wallJumpState = new PlayerWallJumpState(this, stateMachine, "jump");
        dashState = new PlayerDashState(this, stateMachine, "dash");


    }
    protected void Start()
    {
        stateMachine.Initialize(idleState);


    }

    private void Update()
    {
        if (!PV.IsMine) 
            return; 


        stateMachine.currentState.Update();

        //Debug.Log(IsWallDetected() + "walldetect");

        //Debug.Log(facingDir);

        Debug.Log(stateMachine.currentState);



        
    }


    public void SetVelocity(float xVelocity, float yVelocity)
    {
        Vector2 targetVelocity = new Vector2(xVelocity, yVelocity);

        //rb.linearVelocity = Vector2.Lerp(rb.linearVelocity, targetVelocity, Time.deltaTime*moveSpeed);

        rb.linearVelocity = targetVelocity;


        FlipController(xVelocity);
    }

    public void SetZeroVelocity()
    {
        rb.linearVelocity = new Vector2(0,0);
    }


    public bool IsGroundDetected() => Physics2D.Raycast(groundCheck.position, Vector2.down, groundCheckDistance, groundCheckLayerMask);
    public bool IsWallDetected() => Physics2D.Raycast(wallCheck.position, Vector2.right*facingDir, wallCheckDistance, groundCheckLayerMask);
    public bool IsWallOnRight() => Physics2D.Raycast(wallSelection.position, Vector2.right, wallSelectionDistance, groundCheckLayerMask);


    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(groundCheck.position, new Vector3(groundCheck.position.x, groundCheck.position.y - groundCheckDistance));
        Gizmos.DrawLine(wallCheck.position, new Vector3(wallCheck.position.x + wallCheckDistance * facingDir, wallCheck.position.y));
        Gizmos.DrawLine(wallSelection.position, new Vector3(wallSelection.position.x + wallSelectionDistance, wallSelection.position.y));
        Gizmos.DrawWireSphere(attackPoint.position, attackCheckRadius);

    }



    [PunRPC]
    public void SyncFlip()
    {
        facingDir = facingDir * -1;
        facingRight = !facingRight;
        transform.Rotate(0, 180, 0);
    }

    public virtual void Flip()
    {
        if (PhotonNetwork.IsConnected)
            PV.RPC("SyncFlip", RpcTarget.AllBuffered);
        else
        {
            facingDir = facingDir * -1;
            facingRight = !facingRight;
            transform.Rotate(0, 180, 0);
        }
    }

    public virtual void FlipController(float _x)
    {
        if (_x > 0 && !facingRight)
            Flip();
        else if (_x < 0 && facingRight)
            Flip();
    }



    [PunRPC]

    private void SyncMovement(Vector3 position, Vector2 velocity)
    {
        transform.position = position;
        rb.linearVelocity = velocity;
    }


    public void AnimationTrigger() => stateMachine.currentState.AnimationFinishTrigger();


    private void AttackTrigger()
    {

        Collider2D[] collider = Physics2D.OverlapCircleAll(attackPoint.position, attackCheckRadius);

        foreach (var hit in collider)
        {
            if (hit.gameObject == gameObject)
            {
                continue;
            }

            if (hit.GetComponent<Player>() != null)
            {
                Debug.Log("hit!");
            }
        }
    }



}
