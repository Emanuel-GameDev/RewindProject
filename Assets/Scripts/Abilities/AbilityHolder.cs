using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AbilityHolder : Character
{
    public Ability activeAbility;

    private PlayerInputs playerInputs;

    [SerializeField] public Image abilityIconReminder;

    private void Awake()
    {
        playerInputs = PlayerController.instance.inputs;
    }

    private void OnEnable()
    {
        playerInputs.AbilityController.Activate1.performed += Activate1;
        playerInputs.AbilityController.Activate2.performed += Activate2;
        playerInputs.AbilityController.Activate3.performed += Activate3;

        playerInputs.AbilityController.Activate1.canceled += Disactivate1;
        playerInputs.AbilityController.Activate2.canceled += Disactivate2;
        playerInputs.AbilityController.Activate3.canceled += Disactivate3;

        playerInputs.AbilityController.Enable();
    }

    private void Update()
    {
        if (activeAbility != null && !activeAbility.global)
            activeAbility.UpdateAbility();
    }

    private void Disactivate3(InputAction.CallbackContext obj)
    {
        AbilityWheel wheel = GameManager.Instance.abilityManager.wheel;

        if (activeAbility == null || !wheel.gameObject.activeSelf) return;

        activeAbility.Disactivate3(gameObject);
    }

    private void Disactivate2(InputAction.CallbackContext obj)
    {
        AbilityWheel wheel = GameManager.Instance.abilityManager.wheel;

        if (activeAbility == null || !wheel.gameObject.activeSelf) return;

        activeAbility.Disactivate2(gameObject);
    }

    private void Disactivate1(InputAction.CallbackContext obj)
    {
        AbilityWheel wheel = GameManager.Instance.abilityManager.wheel;

        if (activeAbility == null || !wheel.gameObject.activeSelf) return;

        activeAbility.Disactivate1(gameObject);
    }

    private void Activate3(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        AbilityWheel wheel = GameManager.Instance.abilityManager.wheel;

        if (activeAbility == null || !wheel.gameObject.activeSelf) return;

        activeAbility.Activate3(gameObject);
    }

    private void Activate2(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        AbilityWheel wheel = GameManager.Instance.abilityManager.wheel;

        if (activeAbility == null || !wheel.gameObject.activeSelf) return;

        activeAbility.Activate2(gameObject);
    }

    private void Activate1(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        AbilityWheel wheel = GameManager.Instance.abilityManager.wheel;

        if (activeAbility == null || !wheel.gameObject.activeSelf) return;

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

        playerInputs.AbilityController.Disable();
    }


}
