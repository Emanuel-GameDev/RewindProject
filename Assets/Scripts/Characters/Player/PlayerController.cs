using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    PlayerIdle,
    PlayerMooving,
    PlayerJumping,
    PlayerFalling
}


public class PlayerController : Character
{
    public static PlayerController instance;
    public PlayerInputs inputs { get; private set; }
    public StateMachine<PlayerState> stateMachine { get; } = new();

    [Header("MOVEMENT")]
    [SerializeField] float walkSpeed = 10;
    [SerializeField] float runSpeed = 15;
    [SerializeField] float acceleration = 90;
    [SerializeField] float deAcceleration = 60f;

    float horizontalInput = 0;

    [Header("JUMP")]
    [SerializeField] float jumpForce=10;
    [SerializeField] float jumpAbortForce = 50;
    [SerializeField] float maxJumpHeight = 5;

    [Header("GROUND")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 1;

    [Header("FALL")]
    [SerializeField] float _minFallSpeed = 15f;
    [SerializeField] float _maxFallSpeed = 30f;
    
    Vector2 jumpStartPoint;

    public bool isJumping = false;
    public bool isFalling = false;
    public bool isMooving = false;
    public bool isRunning = false;

    Rigidbody2D rBody;

    [HideInInspector] public bool grounded = false;
    float horizontalMovement = 0;

    //da usare per l'abilità
    public bool canDoubleJump;
    bool doubleJump = false;

    #region UnityFunctions

    private void OnEnable()
    {
        inputs = new PlayerInputs();
        inputs.Player.Enable();

        inputs.Player.Walk.performed += SetHorizontalInput;
        inputs.Player.Walk.canceled += SetHorizontalInput;

        inputs.Player.Run.performed += RunInput;
        inputs.Player.Run.canceled += RunInput;

        inputs.Player.Jump.performed += JumpInput;
    }

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        rBody = GetComponent<Rigidbody2D>();

        stateMachine.RegisterState(PlayerState.PlayerIdle, new PlayerIdleState(this));
        stateMachine.RegisterState(PlayerState.PlayerJumping, new PlayerJumpingState(this));
        stateMachine.RegisterState(PlayerState.PlayerFalling, new PlayerFallingState(this));
        stateMachine.RegisterState(PlayerState.PlayerMooving, new PlayerMoovingState(this));
        stateMachine.SetState(PlayerState.PlayerIdle);
    }


    private void Update()
    {
        stateMachine.StateUpdate();

        // da cambiare per l'abilità
        if (Input.GetKeyDown(KeyCode.G))
            InvertGravity();
    }

    public  void FixedUpdate()
    {
        GroundCheck();
    }

    public void OnDrawGizmos()
    {
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + maxJumpHeight, 0));
    }


    private void OnDisable()
    {
        horizontalMovement = 0;

        inputs.Player.Walk.performed -= SetHorizontalInput;
        inputs.Player.Walk.canceled -= SetHorizontalInput;

        inputs.Player.Run.performed -= RunInput;
        inputs.Player.Run.canceled -= RunInput;

        inputs.Player.Jump.performed -= JumpInput;

        inputs.Player.Disable();
    }

    #endregion

    #region Inputs

    private void JumpInput(InputAction.CallbackContext obj)
    {
        CheckJump();
    }

    private void SetHorizontalInput(InputAction.CallbackContext obj)
    {
        horizontalInput = obj.ReadValue<float>();
    }

    private void RunInput(InputAction.CallbackContext obj)
    {
        isRunning = obj.performed;
    }

    #endregion

    #region Movement

    public void CalculateHorizontalMovement()
    {
        if (horizontalInput != 0)
        {
            //calcolo movimento
            horizontalMovement += horizontalInput * acceleration * Time.deltaTime;

            //controllo per la corsa
            if (!isRunning)
                horizontalMovement = Mathf.Clamp(horizontalMovement, -walkSpeed, walkSpeed);
            else
                horizontalMovement = Mathf.Clamp(horizontalMovement, -runSpeed, runSpeed);

        }
        else
        {
            //decellerazione se non c'è input
            horizontalMovement = Mathf.MoveTowards(horizontalMovement, 0, deAcceleration * Time.deltaTime);
        }


        if (horizontalMovement == 0)
            isMooving = false;
        else
            isMooving = true;


        rBody.velocity = new Vector2(horizontalMovement, rBody.velocity.y);
    }

    public void CalculateFallSpeed()
    {
        if (IsGravityDownward())
        {
            if (rBody.velocity.y < 0)
            {
                rBody.velocity = new Vector2(rBody.velocity.x, Mathf.Clamp(rBody.velocity.y, -_maxFallSpeed, -_minFallSpeed));
                isFalling = true;
            }
        }
        else
        {
            if (rBody.velocity.y > 0)
            {
                rBody.velocity = new Vector2(rBody.velocity.x, Mathf.Clamp(rBody.velocity.y, _minFallSpeed, _maxFallSpeed));
                isFalling = true;
            }
        }
    }

    public void CheckJump()
    {
        if (grounded)
        {
            //salto
            Jump();
        }
        else if (canDoubleJump && !grounded && doubleJump)
        {
            //doppio salto se possibile
            Jump();
            doubleJump = false;
        }
    }

    private void Jump()
    {
        rBody.velocity = new Vector2(rBody.velocity.x, 0);

        //salto in base alla direzione della gravità
        if (IsGravityDownward())
        {
            jumpStartPoint = transform.position;
            rBody.AddForce(Vector2.up * jumpForce, ForceMode2D.Impulse);
            isJumping = true;
            isFalling = false;
        }
        else
        {
            jumpStartPoint = transform.position;
            rBody.AddForce(Vector2.down * jumpForce, ForceMode2D.Impulse);
            isJumping = true;
            isFalling = false;
        }
    }

    void CalculateMaxJumpHeightReached()
    {
        //controllo altezza massima del salto raggiunta
        if (!isJumping)
            return;

        if (IsGravityDownward())
        {
            if (transform.position.y >= jumpStartPoint.y + maxJumpHeight)
                isJumping = false;
        }
        else
        {
            if (transform.position.y <= jumpStartPoint.y - maxJumpHeight)
                isJumping = false;
        }

    }

    public void AbortJump()
    {
        CalculateMaxJumpHeightReached();

        //Decellerazione alla fine del salto
        if (IsGravityDownward())
        {
            if (rBody.velocity.y > 0 && !inputs.Player.Jump.IsPressed() || rBody.velocity.y > 0 && !isJumping)
            {
                isJumping = false;
                rBody.AddForce(Vector3.down * jumpAbortForce);
            }
        }
        else
        {
            if (rBody.velocity.y < 0 && !inputs.Player.Jump.IsPressed() || rBody.velocity.y < 0 && !isJumping)
            {
                isJumping = false;
                rBody.AddForce(Vector3.up * jumpAbortForce);
            }
        }
    }

    #endregion

    #region Functions

    bool IsGravityDownward()
    {
        if (rBody.gravityScale >= 0)
            return true;
        else
            return false;
    }

    //da usare per l'abilità
    public void InvertGravity()
    {
        rBody.gravityScale = -rBody.gravityScale;
        transform.localScale = new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
    }

    public void GroundCheck()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);

        if (!isJumping && grounded)
        {
            doubleJump = true;
            isFalling = false;
        }
    }

    #endregion
}
