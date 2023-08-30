using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Utilities;

[RequireComponent(typeof(TMP_Text))]
public class SetTextToTextBox : MonoBehaviour
{
    [TextArea(2, 3)]
    [SerializeField] string message = "Press BUTTONPROMPT to interact";
    [SerializeField] InputActionReference bindingToShow;

    [Header("Setup for sprites")]
    [SerializeField] ListOfTmpSpriteAsset listOfTmpSpriteAsset;

    IDevice currentDevice;
    PlayerInputs playerInputs;
    TMP_Text textBox;



    private void Start()
    {
        playerInputs = PlayerController.instance.inputs;
        textBox = GetComponent<TMP_Text>();
        currentDevice = IDevice.KeyboardMouse;

        InputSystem.onActionChange += InputSystem_onActionChange;
        SetText();
    }

    private void InputSystem_onActionChange(object arg1, InputActionChange arg2)
    {
        if (arg2 == InputActionChange.ActionPerformed)
        {
            if (arg1 is InputAction)
            {
                InputAction inputAction = (InputAction)arg1;

                if (inputAction.activeControl != null)
                    ButtonPressed(inputAction.activeControl);
            }
        }

       
    }

    [ContextMenu("Set Text")]
    private void SetText() 
    {
        textBox.text = CompleteTextWithButtonPromptSprite.ReadAndReplaceBindings(message,
            bindingToShow.action.bindings[(int)currentDevice],
            bindingToShow);
    }

    

    private void ButtonPressed(InputControl device)
    {
        if (device.device is Gamepad)
        {
            currentDevice = IDevice.Gamepad;
            textBox.spriteAsset = listOfTmpSpriteAsset.spriteAssets[1];
        }
        else if (device.device is Keyboard || device.device is Mouse)
        {
            currentDevice = IDevice.KeyboardMouse;
            textBox.spriteAsset = listOfTmpSpriteAsset.spriteAssets[0];
        }
        SetText();
    }
    

    enum IDevice
    {
        KeyboardMouse=0,
        Gamepad=1
    }
    

}
