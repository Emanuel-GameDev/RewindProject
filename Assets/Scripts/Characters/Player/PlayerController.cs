using System;
using System.Collections;
using ToolBox.Serialization;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;
using UnityEngine.Profiling;
using UnityEngine.SceneManagement;

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
    public Transform projectileSpawn;
    public GameObject buttonReminder;

    public float fallStartPoint;
    [HideInInspector] public float fastRespawnTimer = 0;
    [HideInInspector] public float horizontalInput = 0;
    [HideInInspector] public Vector3 fastSpawnPoint;
    [HideInInspector] public Queue previousHorizontalInputs = new Queue();
    [HideInInspector] public Animator animator;

    //Dash
    [HideInInspector] public bool canDash;
    private bool isDashing = false;
    public float dashPower = 50;
    public float dashTime = 0.1f;

    //Debug
    public bool grounded = false;
    public bool isJumping = false;
    public bool isFalling = false;
    public bool isMoving = false;
    public bool isRunning = true;

    //Attack
    public bool isAttacking = false;
    [HideInInspector] public bool canAttack = true;
    [HideInInspector] public float cooldownMeleeAttack;

    TrailRenderer trail;

    internal Rigidbody2D rBody;
    [HideInInspector] public SpriteRenderer bodySprite;

    public float horizontalMovement = 0;
    [HideInInspector] public float groundAngle = 0;


    //Double jump
    Vector2 jumpStartPoint;
    [HideInInspector] public bool canDoubleJump;
    bool doubleJump = false;


    #region UnityFunctions

    private void OnEnable()
    {
        inputs = new PlayerInputs();
        inputs.Player.Enable();

        //inputs.Player.Walk.performed += SetHorizontalInput;
        //inputs.Player.Walk.canceled += SetHorizontalInput;

        inputs.Player.Run.performed += RunInput;
        inputs.Player.Run.canceled += RunInput;

        inputs.Player.OpenMenu.performed += OpenMenuInput;

        inputs.Player.Jump.performed += JumpInput;

        inputs.Player.Dash.performed += TryDash;

        PubSub.Instance.RegisterFunction(EMessageType.TimeRewindStart, ActivateCardAnimation);
        PubSub.Instance.RegisterFunction(EMessageType.TimeRewindStop, DeactivateCardAnimation);

        instance = this;
    }

    private void DeactivateCardAnimation(object obj)
    {
        Debug.Log("D");
        animator.SetBool("UsingCard", false);
    }

    private void ActivateCardAnimation(object obj)
    {
        Debug.Log("A");
        animator.SetTrigger("ActivateCard");
        animator.SetBool("UsingCard", true);
    }



    private void Awake()
    {
        
        animator = GetComponent<Animator>();
        bodySprite = GetComponentInChildren<SpriteRenderer>();
        trail = GetComponent<TrailRenderer>();
        animator.SetBool("Running", isRunning);

        DataSerializer.TryLoad("TemperanceAbility", out canDoubleJump);
        DataSerializer.TryLoad("TemperanceAbility", out canDash);
    }

    private void Start()
    {
        rBody = GetComponent<Rigidbody2D>();

        stateMachine.RegisterState(PlayerState.PlayerIdle, new PlayerIdleState(this));
        stateMachine.RegisterState(PlayerState.PlayerJumping, new PlayerJumpingState(this));
        stateMachine.RegisterState(PlayerState.PlayerFalling, new PlayerFallingState(this));
        stateMachine.RegisterState(PlayerState.PlayerMooving, new PlayerMoovingState(this));
        stateMachine.SetState(PlayerState.PlayerIdle);

        buttonReminder.SetActive(false);
    }

    private void Update()
    {
        stateMachine.StateUpdate();

        if (previousHorizontalInputs.Count >= 5)
            previousHorizontalInputs.Dequeue();
        else
            previousHorizontalInputs.Enqueue(horizontalInput);

        GroundCheck();
    }

    public void FixedUpdate()
    {
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

        //inputs.Player.Walk.performed -= SetHorizontalInput;
        //inputs.Player.Walk.canceled -= SetHorizontalInput;

        inputs.Player.Run.performed -= RunInput;
        inputs.Player.Run.canceled -= RunInput;

        inputs.Player.Jump.performed -= JumpInput;

        inputs.Player.OpenMenu.performed -= OpenMenuInput;

        inputs.Player.Dash.performed -= TryDash;

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
        isRunning = !obj.performed;
        animator.SetBool("Running", isRunning);
    }

    private void OpenMenuInput(InputAction.CallbackContext obj)
    {
        if (GameManager.Instance.pauseMenuManager != null)
        {
            GameManager.Instance.pauseMenuManager.OpenMenu(GameManager.Instance.pauseMenuManager.submenus[0]);
            Time.timeScale = 0;
            inputs.Player.Disable();
            inputs.AbilityController.Disable();
            inputs.UI.Disable();
        }
    }

    #endregion

    #region Movement
    public void CalculateHorizontalMovement()
    {
        horizontalInput = inputs.Player.Walk.ReadValue<float>();

        if (horizontalInput != 0 && !animator.GetBool("Attacking") && canMove)
        {
                //calcolo movimento
                horizontalMovement += horizontalInput * acceleration * Time.deltaTime;

            //controllo per la corsa
            if (!isRunning)
                    horizontalMovement = Mathf.Clamp(horizontalMovement, -walkSpeed, walkSpeed);
                else
                    horizontalMovement = Mathf.Clamp(horizontalMovement, -runSpeed, runSpeed);

                animator.SetFloat("Speed", 1);
        }
        else
        {
                //decellerazione se non c'è input
                horizontalMovement = Mathf.MoveTowards(horizontalMovement, 0, deAcceleration * Time.deltaTime);
                animator.SetFloat("Speed", 0);
        }


        if (Mathf.Abs(horizontalMovement) <= 0.05f)
        {
            isMoving = false;
            previousHorizontalInputs.Clear();
        }
        else
            isMoving = true;

        Vector2 relativMovement = Quaternion.Euler(0, 0, -groundAngle) * new Vector3(horizontalMovement, 0, 0);

        if (horizontalMovement > 0.1)
        {
            if (bodySprite.gameObject.transform.localScale != new Vector3(1, 1, 1))
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("MainCharacter_ChangeDirection"))
                    bodySprite.gameObject.transform.localScale = new Vector3(1, 1, 1);
                else
                    previousHorizontalInputs.Clear();

            }
        }
        else if (horizontalMovement < -0.1)
        {
            if (bodySprite.gameObject.transform.localScale != new Vector3(-1, 1, 1))
            {
                if (!animator.GetCurrentAnimatorStateInfo(0).IsName("MainCharacter_ChangeDirection"))
                    bodySprite.gameObject.transform.localScale = new Vector3(-1, 1, 1);
                else
                    previousHorizontalInputs.Clear();

            }
        }

        if (horizontalInput != 0 && previousHorizontalInputs.Contains(-horizontalInput) && isRunning && !animator.GetCurrentAnimatorStateInfo(0).IsName("MainCharacter_ChangeDirection") && previousHorizontalInputs.Count>0)
        {
            animator.SetTrigger("DirectionChanged");
            previousHorizontalInputs.Clear();
        }

        
        if(!isDashing)
        rBody.velocity = new Vector3(relativMovement.x, rBody.velocity.y, 0);
    }



    public void CalculateFallSpeed()
    {
        if (IsGravityDownward())
        {
            if (rBody.velocity.y < -0.1 && !grounded)
            {
                rBody.velocity = new Vector2(rBody.velocity.x, Mathf.Clamp(rBody.velocity.y, -maxFallSpeed, -minFallSpeed));
                isFalling = true;
            }
        }
        else
        {
            if (rBody.velocity.y > 0.1 && !grounded)
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
            h = Physics2D.RaycastNonAlloc(transform.position, Vector2.down, hits, 1.5f, groundMask);
        else
            h = Physics2D.RaycastNonAlloc(transform.position, Vector2.up, hits, 1.5f,groundMask);

        if (h >= 1)
        {
            groundAngle = Mathf.Atan2(hits[0].normal.x, hits[0].normal.y) * Mathf.Rad2Deg; //calcola l'inclinazione del terreno

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


    private void TryDash(InputAction.CallbackContext obj)
    {
        if (!canDash || isDashing || !grounded)
            return;

        Dash();

    }

    private void Dash()
    {
        isDashing = true;
        Vector2 dashDir = new Vector2(horizontalInput, 0f);
        if(horizontalInput>=0)
            bodySprite.gameObject.transform.localScale = new Vector3(1, 1, 1);
        else
            bodySprite.gameObject.transform.localScale = new Vector3(-1, 1, 1);

        trail.emitting = true;

        rBody.sharedMaterial = noFriction;
        rBody.velocity = dashDir.normalized * dashPower;

        animator.SetBool("Dashing", true);

        StartCoroutine(StopDash());

    }


    private IEnumerator StopDash()
    {
        yield return new WaitForSeconds(dashTime);
        trail.emitting = false;
        isDashing = false;
        animator.SetBool("Dashing", false);
        rBody.sharedMaterial = fullFriction;
    }

    public void CheckFriction()
    {//modifica la frizione in base a l'inclinazione del terreno
        if (isDashing)
            return;

        if (!isMoving)
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

    bool combo = false;
    bool nextAttack = false;

    public void StartComboCheck()
    {
        combo = true;
    }

    public void EndComboCheck()
    {
        combo = false;
    }

    public void EndAttack()
    {
        if(!nextAttack)
        animator.SetBool("Attacking", false);
        nextAttack = false;
    }

    public void StartAttackCooldown()
    {
        StartCoroutine(AttackCooldown(cooldownMeleeAttack));
    }

    public void Attack()
    {
        animator.SetBool("Attacking", true);
        
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("MainCharacter_FirstAttack"))
        {
            if (combo)
            {
                animator.SetTrigger("Hit2");
                nextAttack = true;
            }
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("MainCharacter_SecondAttack"))
        {
            if (combo)
            {
                animator.SetTrigger("Hit3");
                nextAttack = true;

            }
        }
        else if (animator.GetCurrentAnimatorStateInfo(0).IsName("MainCharacter_ThirdAttack"))
        {
            
        }
        else
        {
            animator.SetTrigger("Hit1");
        }
        
    }
    IEnumerator AttackCooldown(float seconds)
    {
        canAttack = false;
        yield return new WaitForSeconds(seconds);
        canAttack = true;
    }

    public bool CheckMaxFallDistanceReached()
    {
        if (IsGravityDownward())
        {
            if (transform.position.y >= fallStartPoint)
                return false;
        }
        else
        {
            if (transform.position.y <= fallStartPoint)
                return false;

        }
            if (maxFallDistanceWithoutTakingDamage < Mathf.Abs(fallStartPoint - transform.position.y))
                return true;
       
        return false;
    }

    public bool IsGravityDownward()
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

    public bool activateCurrentAbility;

    public void AbilityActivationAnimationEvent()
    {
        activateCurrentAbility = true;
    }

    public void GroundCheck()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
        animator.SetBool("Grounded", grounded);
        if (!grounded)
            previousHorizontalInputs.Clear();

        if (!isJumping && grounded)
        {
            doubleJump = true;
            isFalling = false;
        }
    }
    public void ReloadLevel()
    {
        LevelManager.instance.ReloadLevel();
    }

    public void TakeHit()
    {
        animator.SetTrigger("Hitted");
        canMove = false;
    }
    public bool canMove = true;
    public void EnableAllInputs()
    {
        canMove = true;
    }

    public void EnableInputs()
    {
        inputs.Player.Enable();
    }

    public void DisableInputs()
    {
        inputs.Player.Disable();
    }

    #endregion
}
