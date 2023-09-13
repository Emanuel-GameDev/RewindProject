using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuCards : Menu
{
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI descriptionBox;

    public Image cardImageBox;
    public Image fullscreenCardImageBox;

    public GameObject artworkSelection;
    public GameObject descriptionSelection;

    public MenuFullscreen fullscreenMenu;

    public Image cardIcon;

    public MenuButton openTutorialMenuButton;
    [HideInInspector] public MenuVideoButton goToTutorialButton;

    public override void OnEnable()
    {
        MenuCardButton[] buttons = GetComponentsInChildren<MenuCardButton>();

        if (buttons.Length <= 0)
        {
            cardName.text = "";
            descriptionBox.text = "";
            fullscreenCardImageBox.sprite = null;
            artworkSelection.gameObject.SetActive(false);
            descriptionSelection.gameObject.SetActive(false);
            openTutorialMenuButton.gameObject.SetActive(false);
        }

        if(GetComponentsInChildren<MenuCardButton>().Length>0)
            PlayerController.instance.inputs.Menu.MenuInteractionOne.performed += MenuInteractionOne_performed;
       

        if (buttons.Length > 0 && eventSystemDefaultButton == null)
        {
            eventSystemDefaultButton = buttons[0];
        }

        SetNavigation(buttons);

        if (eventSystemDefaultButton != null)
            SetEventSystemSelection();
        
    }

    public void MenuInteractionTwo_started(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        GameManager.Instance.pauseMenuManager.CloseActiveMenu();
        GameManager.Instance.pauseMenuManager.OpenMenu(nextTab);

        nextTab.eventSystemDefaultButton = goToTutorialButton;
        nextTab.SetEventSystemSelection();

    }

    private static void SetNavigation(MenuCardButton[] buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Navigation data = buttons[i].navigation;
            data.mode = Navigation.Mode.Explicit;

            if (i < 2)
            {
                if(i % 2 == 0)
                {
                    if((buttons.Length - 1) % 2 == 0)
                        data.selectOnUp = buttons[buttons.Length - 1];
                    else
                        data.selectOnUp = buttons[buttons.Length - 2];
                }
                else
                    data.selectOnUp = buttons[buttons.Length - 1];
            }
            else
                data.selectOnUp = buttons[i - 2];



            if (buttons.Length-1 == i && i % 2 == 0)
            {
                data.selectOnLeft = buttons[i];
                data.selectOnRight = buttons[i];
            }
            else
            {
                if (i % 2 == 0)
                {
                    data.selectOnLeft = buttons[i+1];
                    data.selectOnRight = buttons[i+1];
                }
                else
                {
                    data.selectOnLeft = buttons[i - 1];
                    data.selectOnRight = buttons[i - 1];
                }
            }
           

            if (i >= buttons.Length - 2)
            {
                if (i % 2 == 0)
                    data.selectOnDown = buttons[0];
                else
                    data.selectOnDown = buttons[1];
            }
            else
                data.selectOnDown = buttons[i + 2];

           


            buttons[i].navigation = data;
        }
    }


    private void MenuInteractionOne_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        GameManager.Instance.pauseMenuManager.OpenMenu(fullscreenMenu);
        PlayerController.instance.inputs.Menu.MenuInteractionOne.Disable();
    }

    public void OnDisable()
    {
        PlayerController.instance.inputs.Menu.MenuInteractionOne.performed -= MenuInteractionOne_performed;
        PlayerController.instance.inputs.Menu.MenuInteractionTwo.performed -= MenuInteractionTwo_started;
    }


}
