using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class PlayerMovementInput : Character
{
    public static PlayerMovementInput instance;
    public PlayerInputs inputs { get; private set; }
    //Character player;

    private void OnEnable()
    {
        inputs = new PlayerInputs();
        inputs.Player.Enable();

        inputs.Player.Move.performed += SetMove;
        inputs.Player.Move.canceled += SetMove;

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
    }


    private void Update()
    {
        rBody.velocity = new Vector2(horizontalMovement * speed, rBody.velocity.y);
    }


    //private void OnDisable()
    //{
    //    player.horizontalMovement = 0;

    //    inputs.Player.Move.performed -= SetMove;
    //    inputs.Player.Move.canceled -= SetMove;

    //    inputs.Player.Disable();
    //}


    private void Jump(InputAction.CallbackContext obj)
    {
        Jump();
    }

    public override void Jump()
    {
        if (grounded)
            rBody.AddForce(Vector2.up * jumpForce * rBody.gravityScale);
    }


    private void SetMove(InputAction.CallbackContext obj)
    {
        horizontalMovement = obj.ReadValue<float>();
    }

}
