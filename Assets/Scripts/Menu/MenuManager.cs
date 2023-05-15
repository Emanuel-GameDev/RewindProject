using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public PlayerInputs inputs { get; private set; }

    private void OnEnable()
    {
        inputs = new PlayerInputs();
        inputs.Player.Enable();

        inputs.Menu.OpenMenu.performed += OpenMenuInput;

        PlayerController.instance.inputs.Player.Disable();
    }

    private void OpenMenuInput(InputAction.CallbackContext obj)
    {
        
    }

    private void OnDisable()
    {
        PlayerController.instance.inputs.Player.Enable();
    }
}
