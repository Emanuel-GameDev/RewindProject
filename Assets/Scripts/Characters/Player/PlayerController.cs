using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

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

    [SerializeField] float acceleration = 90;
    [SerializeField] float deAcceleration = 60f;
    [SerializeField] float groundCheckRadius = 1;

    float horizontalInput = 0;

    [SerializeField] float jumpAbortForce=2;

    [SerializeField] private float _minFallSpeed = 1f;
    [SerializeField] private float _maxFallSpeed = 15f;

    bool isJumping = false;
    bool isFalling = false;
    bool isMooving = false;


    private void OnEnable()
    {
        inputs = new PlayerInputs();
        inputs.Player.Enable();

        inputs.Player.Move.performed += SetHorizontalInput;
        inputs.Player.Move.canceled += SetHorizontalInput;

        inputs.Player.Jump.performed += Jump;
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

        if (Input.GetKeyDown(KeyCode.R))
            InvertGravity();

        CalculateWalk();
        AbortJump();
        CalculateFallSpeed();
        
    }


    //private void OnDisable()
    //{
    //    horizontalMovement = 0;

    //    inputs.Player.Move.performed -= SetHorizontalInput;
    //    inputs.Player.Move.canceled -= SetHorizontalInput;

    //    inputs.Player.Disable();
    //}


    private void Jump(InputAction.CallbackContext obj)
    {
        Jump();
    }

    private void SetHorizontalInput(InputAction.CallbackContext obj)
    {
        horizontalInput = obj.ReadValue<float>();
    }

    private void CalculateFallSpeed()
    {
        if (rBody.gravityScale > 0)
        {
            if (rBody.velocity.y < 0)
            {
                rBody.velocity = new Vector2(rBody.velocity.x, Mathf.Clamp(rBody.velocity.y, -_maxFallSpeed, -_minFallSpeed));
                isFalling = true;
            }
        }
        else if (rBody.gravityScale < 0)
        {
            if (rBody.velocity.y > 0)
            {
                rBody.velocity = new Vector2(rBody.velocity.x, Mathf.Clamp(rBody.velocity.y, _minFallSpeed, _maxFallSpeed));
                isFalling = true;
            }
        }
    }

    private void CalculateWalk()
    {
        if (horizontalInput != 0)
        {
            horizontalMovement += horizontalInput * acceleration * Time.deltaTime;

            horizontalMovement = Mathf.Clamp(horizontalMovement, -maxSpeed, maxSpeed);
        }
        else
        {
            horizontalMovement = Mathf.MoveTowards(horizontalMovement, 0, deAcceleration * Time.deltaTime);
        }
        rBody.velocity = new Vector2(horizontalMovement, rBody.velocity.y);
    }

    


    public override void Jump()
    {
        if (grounded)
        {
            isJumping = true;
           rBody.AddForce(Vector2.up * jumpForce * rBody.gravityScale, ForceMode2D.Impulse);
        }
    }

    private void AbortJump()
    {
        if(rBody.gravityScale > 0) 
        { 
            if (rBody.velocity.y > 0 && inputs.Player.Jump.WasReleasedThisFrame())
            {
                isJumping = false;
                rBody.velocity = new Vector2(rBody.velocity.x, rBody.velocity.y / jumpAbortForce);
            }
        }

        if(rBody.gravityScale < 0)
        {
            if (rBody.velocity.y < 0 && inputs.Player.Jump.WasReleasedThisFrame())
            {
                isJumping = false;
                rBody.velocity = new Vector2(rBody.velocity.x, rBody.velocity.y / jumpAbortForce);
            }
        }
    }

    public void InvertGravity()
    {
        rBody.gravityScale = -rBody.gravityScale;
        transform.localScale =new Vector3(transform.localScale.x, -transform.localScale.y, transform.localScale.z);
    }

    public override void GroundCheck()
    {
        grounded = Physics2D.OverlapCircle(groundCheck.position, groundCheckRadius, groundMask);
        //if (grounded)
        //    isFalling = false;
    }


    public override void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(groundCheck.position, groundCheckRadius);
    }
}
