using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float speed;
    [SerializeField] float jumpForce;

    Rigidbody2D rBody;

     float horizontalInput = 0;

    PlayerInputs inputs;


    private void OnEnable()
    {
        inputs = new PlayerInputs();
        inputs.Player.Enable();

        inputs.Player.Move.performed += SetMove;
        inputs.Player.Move.canceled += SetMove;

        inputs.Player.Jump.performed += Jump;
    }


    private void Start()
    {
        rBody = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        rBody.velocity = new Vector2(horizontalInput * speed, rBody.velocity.y);
    }

    private void OnDisable()
    {
        inputs.Player.Disable();

        inputs.Player.Move.performed -= SetMove;
        inputs.Player.Move.canceled -= SetMove;
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        rBody.AddForce(Vector2.up * jumpForce * rBody.gravityScale);
    }

    private void SetMove(InputAction.CallbackContext obj)
    {
        horizontalInput = obj.ReadValue<float>();
    }

}
