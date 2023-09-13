using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;

public class MenuManager : MonoBehaviour
{
    public PlayerInputs inputs { get; private set; }

    public AudioSource audioSource;
    public AudioClip buttonSelectedSound;
    public AudioClip buttonClickSound;

    [SerializeField] AudioClip nextTabAudioClip;


    [HideInInspector] public Menu[] submenus;

    public virtual void OnEnable()
    {
        inputs = PlayerController.instance.inputs;

        inputs.Menu.Disable();

        inputs.Menu.CloseMenu.performed += CloseMenuInput;
        inputs.Menu.NextTab.performed += OpenNextTab;
        inputs.Menu.PreviousTab.performed += OpenPreviousTab;

        audioSource = GetComponent<AudioSource>();

        submenus = GetComponentsInChildren<Menu>(true);
    }

    private void OpenPreviousTab(InputAction.CallbackContext obj)
    {
        if (GetComponentsInChildren<Menu>().Length < 1)
            return;

        Menu currentMenu = GetComponentsInChildren<Menu>()[GetComponentsInChildren<Menu>().Length - 1];

        if (currentMenu.previousTab == null)
            return;

        audioSource.clip = nextTabAudioClip;
        audioSource.Play();

        CloseActiveMenu();
        OpenMenu(currentMenu.previousTab);
    }

    private void OpenNextTab(InputAction.CallbackContext obj)
    {
        if (GetComponentsInChildren<Menu>().Length < 1)
            return;

        Menu currentMenu = GetComponentsInChildren<Menu>()[GetComponentsInChildren<Menu>().Length - 1];

        if (currentMenu.nextTab == null)
            return;

        audioSource.clip = nextTabAudioClip;
        audioSource.Play();

        CloseActiveMenu();
        OpenMenu(currentMenu.nextTab);
    }

    public void OpenMenu(Menu menu)
    {
        inputs.Menu.Enable();

        menu.gameObject.SetActive(true);
    }

    private void CloseMenuInput(InputAction.CallbackContext obj)
    {
        if(GetComponentsInChildren<Menu>().Length>0)
           CloseActiveMenu();
    }

    public void CloseActiveMenu()
    {
        if (GetComponentsInChildren<Menu>().Length < 1)
            return;

        Menu menuToClose = GetComponentsInChildren<Menu>()[GetComponentsInChildren<Menu>().Length - 1];

        if (menuToClose.gameObject.activeSelf)
        {
            menuToClose.gameObject.SetActive(false);
        }

        if(menuToClose == submenus[0])
        {
            inputs.Menu.Disable();
            Time.timeScale = 1;
            inputs.Player.Enable();
            inputs.AbilityController.Enable();
            inputs.UI.Enable();
        }
        else
            GetComponentsInChildren<Menu>()[GetComponentsInChildren<Menu>().Length - 1].SetEventSystemSelection();


    }

    public virtual void OnDisable()
    {
        inputs.Menu.CloseMenu.performed -= CloseMenuInput;
        inputs.Menu.NextTab.performed -= OpenNextTab;
        inputs.Menu.PreviousTab.performed -= OpenPreviousTab;
    }
}
