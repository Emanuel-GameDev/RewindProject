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

    [HideInInspector] public AudioSource audioSource;
    [SerializeField] AudioClip nextTabAudioClip;
    public AudioClip buttonSelectedSound;
    [HideInInspector] public Menu[] submenus;

    public virtual void OnEnable()
    {
        inputs = PlayerController.instance.inputs;
        inputs.Menu.Disable();
        inputs.Menu.CloseMenu.performed += CloseMenuInput;
        inputs.Menu.NextTab.performed += OpenNextTab;
        inputs.Menu.PreviousTab.performed += OpenPreviousTab;
        submenus = GetComponentsInChildren<Menu>(true);
        audioSource = GetComponent<AudioSource>();
    }

    private void OpenPreviousTab(InputAction.CallbackContext obj)
    {
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
        Menu currentMenu = GetComponentsInChildren<Menu>()[GetComponentsInChildren<Menu>().Length - 1];

        if (currentMenu.nextTab == null)
            return;

        audioSource.clip = nextTabAudioClip;
        audioSource.Play();

        CloseActiveMenu();
        OpenMenu(currentMenu.nextTab);
    }

    private void Start()
    {
        
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
            GetComponentsInChildren<Menu>()[GetComponentsInChildren<Menu>().Length - 1].SetEventSystemSelection(GetComponentsInChildren<Menu>()[GetComponentsInChildren<Menu>().Length - 1].eventSystemDefaultButton);


    }

    public virtual void OnDisable()
    {
        inputs.Menu.CloseMenu.performed -= CloseMenuInput;
        inputs.Menu.NextTab.performed -= OpenNextTab;
        inputs.Menu.PreviousTab.performed -= OpenPreviousTab;
    }
}
