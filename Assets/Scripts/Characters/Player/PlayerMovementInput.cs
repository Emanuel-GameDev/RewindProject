using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[ RequireComponent(typeof(Character))]
public class PlayerMovementInput : MonoBehaviour
{
    public PlayerInputs inputs { get; private set; }
    Character player;

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
        player = GetComponent<Character>();
    }

    private void OnDisable()
    {
        player.horizontalMovement = 0;

        inputs.Player.Move.performed -= SetMove;
        inputs.Player.Move.canceled -= SetMove;
        inputs.Player.Disable();
    }

    private void Jump(InputAction.CallbackContext obj)
    {
        player.Jump();
    }

    private void SetMove(InputAction.CallbackContext obj)
    {
        player.horizontalMovement = obj.ReadValue<float>();
    }
}
