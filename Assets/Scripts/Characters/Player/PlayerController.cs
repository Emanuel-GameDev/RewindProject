using UnityEngine;
using UnityEngine.InputSystem;

public enum PlayerState
{
    PlayerIdle,
    PlayerWalking,
    PlayerJumping,
    PlayerFalling
}


public class PlayerController : Character
{
    public static PlayerController instance;
    public PlayerInputs inputs { get; private set; }
    public StateMachine<PlayerState> stateMachine { get; } = new();

    [Header("MOVEMENT")]
    //[SerializeField]  float walkSpeed = 10;
    [SerializeField] float runSpeed = 15;
    [SerializeField] float acceleration = 90;
    [SerializeField] float deAcceleration = 60f;

    float horizontalInput = 0;

    [Header("JUMP")]
    //[SerializeField] float jumpForce;
    [SerializeField] float jumpAbortForce = 2;
    [SerializeField] float maxJumpHeight = 5;

    //[Header("GROUND")]
    //[SerializeField] LayerMask groundMask;
    //[SerializeField] Transform groundCheck;

    [Header("FALL")]
    [SerializeField] private float _minFallSpeed = 1f;
    [SerializeField] private float _maxFallSpeed = 15f;
    
    Vector2 jumpStartPoint;

    bool isJumping = false;
    bool isFalling = false;
    bool isMooving = false;
    bool isRunning = false;

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
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        DontDestroyOnLoad(gameObject);
        rBody = GetComponent<Rigidbody2D>();

        stateMachine.RegisterState(PlayerState.PlayerIdle, new PlayerIdleState(this));
    }


    private void Update()
    {
        stateMachine.StateUpdate();

        // da cambiare per l'abilità
        if (Input.GetKeyDown(KeyCode.R))
            InvertGravity();

        CalculateHorizontalMovement();
        AbortJump();
        CalculateFallSpeed();

    }
    public override void OnDrawGizmos()
    {
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + maxJumpHeight, 0));
    }


    //private void OnDisable()
    //{
    //    horizontalMovement = 0;

    //    inputs.Player.Move.performed -= SetHorizontalInput;
    //    inputs.Player.Move.canceled -= SetHorizontalInput;

    //    inputs.Player.Disable();
    //}

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

    private void CalculateHorizontalMovement()
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

        rBody.velocity = new Vector2(horizontalMovement, rBody.velocity.y);
    }

    private void CalculateFallSpeed()
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

    public override void CheckJump()
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
        }
        else
        {
            jumpStartPoint = transform.position;
            rBody.AddForce(Vector2.down * jumpForce, ForceMode2D.Impulse);
            isJumping = true;
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

    private void AbortJump()
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
            if (rBody.velocity.y < 0 && !inputs.Player.Jump.IsPressed() || rBody.velocity.y > 0 && !isJumping)
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

    public override void GroundCheck()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);

        if (!isJumping && grounded)
            doubleJump = true;
    }

    #endregion
}
