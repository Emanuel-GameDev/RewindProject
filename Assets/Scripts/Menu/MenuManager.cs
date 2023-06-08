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

        //SceneManager.sceneLoaded += registerFuntion;
        //SceneManager.sceneUnloaded += unregisterFuntion;
    }
    //private void registerFuntion(Scene arg0, LoadSceneMode arg1)
    //{
    //    PubSub.Instance.RegisterFunction(EMessageType.AbilityPicked, UnlockMenu);
    //}

    //private void UnlockMenu(object obj)
    //{
    //    AbilityMenu[] abilityMenus = GetComponentsInChildren<AbilityMenu>(true);

    //    //da rivedere
    //    if (obj is not Ability)
    //        return;

    //    Ability abilityPicked = (Ability)obj;

    //    foreach(AbilityMenu am in abilityMenus)
    //    {
    //        //if (am.ability == abilityPicked.eAbility)
    //        //{
    //        //    am.UnlockMenu();
    //        //}
    //    }

    //}


    private void Start()
    {
            Instance = this;



        //DontDestroyOnLoad(gameObject);
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

        if(menuToClose == submenus[0])
        {
            PlayerController.instance.inputs.Player.Enable();

            inputs.Menu.Disable();
        }
    }

    private void OnDisable()
    {
        inputs.Menu.CloseMenu.performed -= CloseMenuInput;

        //PubSub.Instance.UnregisterFunction(EMessageType.AbilityPicked, UnlockMenu);

        //SceneManager.sceneLoaded -= registerFuntion;
    }
}
