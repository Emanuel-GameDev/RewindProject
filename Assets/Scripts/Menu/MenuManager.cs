using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public PlayerInputs inputs { get; private set; }

    [HideInInspector] public Menu[] menus;

    private void OnEnable()
    {
        Instance = this;

        inputs = new PlayerInputs();
        inputs.Menu.CloseMenu.performed += CloseMenuInput;

        menus = gameObject.GetComponentsInChildren<Menu>(true);
    }

    public void OpenMenu(Menu menu)
    {
        inputs.Menu.Enable();
        
        menu.gameObject.SetActive(true);
    }

    private void CloseMenuInput(InputAction.CallbackContext obj)
    {
        CloseActiveMenu();
    }

    public void CloseActiveMenu()
    {
        
        Menu menuToClose = GetComponentsInChildren<Menu>()[GetComponentsInChildren<Menu>().Length - 1];

        if (menuToClose.gameObject.activeSelf)
        {
            menuToClose.gameObject.SetActive(false);
        }

        if(menuToClose == menus[0])
        {
            PlayerController.instance.inputs.Player.Enable();

            inputs.Menu.Disable();
        }
    }

    private void OnDisable()
    {
        inputs.Menu.CloseMenu.performed -= CloseMenuInput;
    }
}
