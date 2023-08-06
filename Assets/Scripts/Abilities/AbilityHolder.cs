using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class AbilityHolder : Character
{
    public Ability activeAbility;

    private PlayerInputs playerInputs;

    private void Awake()
    {
        playerInputs = new PlayerInputs();
    }

    private void OnEnable()
    {
        playerInputs.AbilityController.Activate1.performed += Activate1;
        playerInputs.AbilityController.Activate2.performed += Activate2;
        playerInputs.AbilityController.Activate3.performed += Activate3;

        playerInputs.AbilityController.Activate1.canceled += Disactivate1;
        playerInputs.AbilityController.Activate2.canceled += Disactivate2;
        playerInputs.AbilityController.Activate3.canceled += Disactivate3;

        playerInputs.Enable();
    }

    private void Update()
    {
        if (activeAbility != null)
            activeAbility.UpdateAbility();
    }

    private void Disactivate3(InputAction.CallbackContext obj)
    {
        if (activeAbility == null) return;

        activeAbility.Disactivate3(gameObject);
    }

    private void Disactivate2(InputAction.CallbackContext obj)
    {
        if (activeAbility == null) return;

        activeAbility.Disactivate2(gameObject);
    }

    private void Disactivate1(InputAction.CallbackContext obj)
    {
        if (activeAbility == null) return;

        activeAbility.Disactivate1(gameObject);
    }

    private void Activate3(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (activeAbility == null) return;

        activeAbility.Activate3(gameObject);
    }

    private void Activate2(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (activeAbility == null) return;

        activeAbility.Activate2(gameObject);
    }

    private void Activate1(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        if (activeAbility == null) return;

        activeAbility.Activate1(gameObject);
    }

    private void OnDisable()
    {
        playerInputs.AbilityController.Activate1.performed -= Activate1;
        playerInputs.AbilityController.Activate2.performed -= Activate2;
        playerInputs.AbilityController.Activate3.performed -= Activate3;

        playerInputs.AbilityController.Activate1.canceled -= Disactivate1;
        playerInputs.AbilityController.Activate2.canceled -= Disactivate2;
        playerInputs.AbilityController.Activate3.canceled -= Disactivate3;

        playerInputs.Disable();
    }


}
