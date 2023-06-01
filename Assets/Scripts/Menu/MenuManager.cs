using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    public static MenuManager Instance;

    public PlayerInputs inputs { get; private set; }

    [HideInInspector] public Menu[] submenus;

    private void OnEnable()
    {
        inputs = new PlayerInputs();
        inputs.Menu.CloseMenu.performed += CloseMenuInput;

        submenus = GetComponentsInChildren<Menu>(true);

        
    }


    private void Start()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);


        
        DontDestroyOnLoad(gameObject);
    }

    public void OpenMenu(Menu menu)
    {
        foreach (Menu m in submenus)
        {
            if (m.unlocked)
            {
                foreach (Menu p in m.GetComponentsInParent<Menu>(true))
                {
                    p.unlocked = true;
                }

            }
        }

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

        if(menuToClose == submenus[0])
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
