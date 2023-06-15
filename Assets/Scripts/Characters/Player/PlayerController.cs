using ToolBox.Serialization;
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


    [Header("JUMP")]
    [SerializeField] float jumpForce = 10;
    [SerializeField] float jumpAbortForce = 50;
    [SerializeField] float maxJumpHeight = 5;

    [Header("GROUND")]
    [SerializeField] LayerMask groundMask;
    [SerializeField] Transform groundCheck;
    [SerializeField] float groundCheckRadius = 1;

    [Header("FALL")]
    [SerializeField] float minFallSpeed = 15f;
    [SerializeField] float maxFallSpeed = 30f;
    [SerializeField] float maxFallDistanceWithoutTakingDamage = 5;

    [Header("Slope")]
    [SerializeField] float maxSlope = 45;
    [SerializeField] float rotationRatioOnSlopes = 3;

    [Header("OTHER")]
    [SerializeField] float fastRespawnRefreshTimer = 0.5f;
    [SerializeField] PhysicsMaterial2D noFriction;
    [SerializeField] PhysicsMaterial2D fullFriction;

    [HideInInspector] public float fallStartPoint;
    [HideInInspector] public float fastRespawnTimer = 0;
    Vector2 jumpStartPoint;
    [HideInInspector] public Vector3 fastSpawnPoint;
    public bool isJumping = false;
    public bool isFalling = false;
    public bool isMooving = false;
    public bool isRunning = false;
    [HideInInspector] public bool grounded = false;

    internal Rigidbody2D rBody;
    [HideInInspector] public Animator animator;
    SpriteRenderer bodySprite;

    float horizontalInput = 0;
    float horizontalMovement = 0;
    float groundAngle = 0;

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

        inputs.Player.OpenMenu.performed += OpenMenuInput;

        inputs.Player.Jump.performed += JumpInput;
    }


    private void Awake()
    {
        instance = this;
        animator = GetComponent<Animator>();
        bodySprite = GetComponentInChildren<SpriteRenderer>();
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

        if (Input.GetKeyDown(KeyCode.CapsLock))
        {
            DataSerializer.DeleteAll();
            Debug.Log("Saving deleted");
        }
    }

    public void FixedUpdate()
    {
        GroundCheck();

        if (grounded)
        {
            if (fastRespawnTimer < fastRespawnRefreshTimer)
                fastRespawnTimer += Time.deltaTime;
            else
            {
                fastSpawnPoint = transform.position;
                fastRespawnTimer = 0;
            }
        }
        else
            fastRespawnTimer = 0;


    }

    public void OnDrawGizmos()
    {
        if (groundCheck != null)
            Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y + maxJumpHeight, 0));

        Gizmos.DrawLine(transform.position, new Vector3(transform.position.x, transform.position.y - maxFallDistanceWithoutTakingDamage, 0));

    }


    private void OnDisable()
    {
        horizontalMovement = 0;

        inputs.Player.Walk.performed -= SetHorizontalInput;
        inputs.Player.Walk.canceled -= SetHorizontalInput;

        inputs.Player.Run.performed -= RunInput;
        inputs.Player.Run.canceled -= RunInput;

        inputs.Player.Jump.performed -= JumpInput;

        inputs.Player.OpenMenu.performed -= OpenMenuInput;

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
        animator.SetBool("Running", isRunning);
    }

    private void OpenMenuInput(InputAction.CallbackContext obj)
    {
        if (MenuManager.Instance != null)
        {
            MenuManager.Instance.OpenMenu(MenuManager.Instance.submenus[0]);
            inputs.Player.Disable();
        }
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

        Vector2 relativMovement = Quaternion.Euler(0, 0, -groundAngle) * new Vector3(horizontalMovement, 0, 0);

        if (horizontalMovement > 0.1)
            bodySprite.gameObject.transform.localScale = new Vector3(1, 1 ,1);
        else if(horizontalMovement < -0.1)
            bodySprite.gameObject.transform.localScale = new Vector3(-1, 1, 1);


        rBody.velocity = new Vector3(relativMovement.x, rBody.velocity.y, 0);
    }

    public void CalculateFallSpeed()
    {
        if (IsGravityDownward())
        {
            if (rBody.velocity.y < -1 && !grounded)
            {
                rBody.velocity = new Vector2(rBody.velocity.x, Mathf.Clamp(rBody.velocity.y, -maxFallSpeed, -minFallSpeed));
                isFalling = true;
            }
        }
        else
        {
            if (rBody.velocity.y > 1 && !grounded)
            {
                rBody.velocity = new Vector2(rBody.velocity.x, Mathf.Clamp(rBody.velocity.y, minFallSpeed, maxFallSpeed));
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
                rBody.AddForce(Vector3.down * jumpAbortForce);
            }

            if (rBody.velocity.y < 0)
                isJumping = false;
        }
        else
        {
            if (rBody.velocity.y < 0 && !inputs.Player.Jump.IsPressed() || rBody.velocity.y < 0 && !isJumping)
            {
                rBody.AddForce(Vector3.up * jumpAbortForce);
            }

            if (rBody.velocity.y > 0)
                isJumping = false;
        }

    }

    public void CheckRotation()
    {
        RaycastHit2D[] hits = new RaycastHit2D[2];

        int h;

        if (IsGravityDownward())
            h = Physics2D.RaycastNonAlloc(transform.position, Vector2.down, hits, 1.5f);
        else
            h = Physics2D.RaycastNonAlloc(transform.position, Vector2.up, hits, 1.5f);

        if (h > 1)
        {
            groundAngle = Mathf.Atan2(hits[1].normal.x, hits[1].normal.y) * Mathf.Rad2Deg; //calcola l'inclinazione del terreno

            if (!IsGravityDownward())
            {
                if (groundAngle >= 0)
                    groundAngle -= 180;
                else
                    groundAngle += 180;
            }

            transform.rotation = Quaternion.Euler(0, 0, -groundAngle / rotationRatioOnSlopes);

        }

        CheckFriction();
    }

    public void CheckFriction()
    {//modifica la frizione in base a l'inclinazione del terreno
        if (!isMooving)
        {
            if (Mathf.Abs(groundAngle) < maxSlope)
                rBody.sharedMaterial = fullFriction;
            else
                rBody.sharedMaterial = noFriction;

        }
        else
            rBody.sharedMaterial = noFriction;

    }

    #endregion

    #region Functions

    public bool CheckMaxFallDistanceReached()
    {
        if (IsGravityDownward())
        {
            if (maxFallDistanceWithoutTakingDamage < Mathf.Abs(fallStartPoint - transform.position.y))
                    return true;
        }
        else
        {
            if (maxFallDistanceWithoutTakingDamage < Mathf.Abs(fallStartPoint + transform.position.y))
                return true;
        }

        return false;
    }

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
