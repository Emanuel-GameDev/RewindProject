using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AbilityHolder : Character
{
    public Ability activeAbility;

    private PlayerInputs playerInputs;

    private void Update()
    {
        if (activeAbility == null) return;

        //bool activate1 = playerInputs.AbilityController.Activate1.ReadValue<float>() > 0;
        //bool activate2 = playerInputs.AbilityController.Activate2.ReadValue<float>() > 0;
        //bool activate3 = playerInputs.AbilityController.Activate3.ReadValue<float>() > 0;

        //if(activate1) activeAbility.Activate1(gameObject);
        //if(activate2) activeAbility.Activate2(gameObject);
        //if(activate3) activeAbility.Activate3(gameObject);  
    }

    private void Awake()
    {
        playerInputs = new PlayerInputs();
    }

    private void OnEnable()
    {
        playerInputs.AbilityController.Activate1.performed += Activate1;
        playerInputs.AbilityController.Activate2.performed += Activate2;
        playerInputs.AbilityController.Activate3.performed += Activate3;


        playerInputs.Enable();
    }

    private void Activate3(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        activeAbility.Activate3(gameObject);
    }

    private void Activate2(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        activeAbility.Activate2(gameObject);
    }

    private void Activate1(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        activeAbility.Activate1(gameObject);
    }

    private void OnDisable()
    {
        playerInputs.Disable();
    }


}
