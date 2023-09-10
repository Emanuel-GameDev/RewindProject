using System;
using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.LowLevel;

public class GamepadCursor : MonoBehaviour
{
    [SerializeField] public PlayerInput playerInputs;

    [SerializeField] public RectTransform cursorTransform;

    [SerializeField] public Canvas canvas;

    [SerializeField] public RectTransform canvasRectTransform;

    [SerializeField] float cursorSpeed = 3000f;

    private bool previousMouseState;
    private Mouse virtualMouse;
    private Mouse currentMouse;
    private Camera mainCamera;


    string previousControlScheme = "";
    [HideInInspector] public string gamepadScheme = "Gamepad";
    [HideInInspector] public string mouseScheme = "Keyboard&Mouse";


    private void OnEnable()
    {
        mainCamera = Camera.main;
        currentMouse = Mouse.current;

        if (virtualMouse == null)
            virtualMouse = (Mouse)InputSystem.AddDevice("VirtualMouse");
        else if (!virtualMouse.added)
            InputSystem.AddDevice(virtualMouse);

        if(cursorTransform != null)
        {
            Vector2 position = cursorTransform.anchoredPosition;
            InputState.Change(virtualMouse.position, position);
        }

        InputSystem.onAfterUpdate += UpdateMotion;
        playerInputs.onControlsChanged += OnControlsChanged;

    }

    private void Start()
    {
        SetFakeCursor(true);
        SetRealCursor(false);
        

        if (playerInputs.currentControlScheme == mouseScheme)
        {
            cursorTransform.gameObject.SetActive(false);
            Cursor.visible = true;
            previousControlScheme = mouseScheme;
        }
        else if (playerInputs.currentControlScheme == gamepadScheme)
        {
            cursorTransform.gameObject.SetActive(true);
            Cursor.visible = false;
        }
    }

    private void OnControlsChanged(PlayerInput input)
    {
        if (playerInputs.currentControlScheme == mouseScheme && previousControlScheme != mouseScheme)
        {
            cursorTransform.gameObject.SetActive(false);
            Cursor.visible = true;
            currentMouse.WarpCursorPosition(virtualMouse.position.ReadValue());
            previousControlScheme = mouseScheme;
        }
        else if (playerInputs.currentControlScheme == gamepadScheme && previousControlScheme != gamepadScheme)
        {
            cursorTransform.gameObject.SetActive(true);
            Cursor.visible = false;
            InputState.Change(virtualMouse.position, currentMouse.position.ReadValue());
            AnchorCursor(currentMouse.position.ReadValue());
            previousControlScheme = gamepadScheme;
        }
    }

    public void SetFakeCursor(bool active)
    {
        cursorTransform.gameObject.SetActive(active);
    }
    public void SetRealCursor(bool visible)
    {
        cursorTransform.gameObject.SetActive(visible);
    }


    private void UpdateMotion()
    {
        if (virtualMouse == null || Gamepad.current == null)
            return;

        Vector2 deltaValue = Gamepad.current.leftStick.ReadValue();
        deltaValue *= cursorSpeed * Time.deltaTime;

        Vector2 currentPosition = virtualMouse.position.ReadValue();
        Vector2 newPosition = currentPosition + deltaValue;

        newPosition.x = Mathf.Clamp(newPosition.x, 0, Screen.width);
        newPosition.y = Mathf.Clamp(newPosition.y, 0, Screen.height);

        InputState.Change(virtualMouse.position, newPosition);
        InputState.Change(virtualMouse.delta, deltaValue);


        bool aButtonIsPressed = Gamepad.current.aButton.IsPressed();
        if(previousMouseState!= aButtonIsPressed)
        {
            virtualMouse.CopyState<MouseState>(out var mouseState);
            mouseState.WithButton(MouseButton.Left, Gamepad.current.aButton.IsPressed());
            InputState.Change(virtualMouse, mouseState);
            previousMouseState = aButtonIsPressed;
        }

        AnchorCursor(newPosition);

    }

    private void AnchorCursor(Vector2 position)
    {
        Vector2 anchoredPosition;

        RectTransformUtility.ScreenPointToLocalPointInRectangle(canvasRectTransform, position, canvas.renderMode ==
            RenderMode.ScreenSpaceOverlay ? null : mainCamera, out anchoredPosition);

        cursorTransform.anchoredPosition = anchoredPosition;
    }

    private void OnDisable()
    {
        if(virtualMouse !=null && virtualMouse.added)
            InputSystem.RemoveDevice(virtualMouse);

        InputSystem.onAfterUpdate -= UpdateMotion;
        playerInputs.onControlsChanged -= OnControlsChanged;
    }
}
