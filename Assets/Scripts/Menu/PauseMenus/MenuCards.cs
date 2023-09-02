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
     


    public override void OnEnable()
    {
        cardName.text = "";
        descriptionBox.text = "";
        fullscreenCardImageBox.sprite = null;
        artworkSelection.gameObject.SetActive(false);
        descriptionSelection.gameObject.SetActive(false);

        if(GetComponentsInChildren<MenuCardButton>().Length>0)
            PlayerController.instance.inputs.Menu.MenuInteractionOne.started += MenuInteractionOne_performed;




        MenuCardButton[] buttons = GetComponentsInChildren<MenuCardButton>();

        if (buttons.Length > 0 && eventSystemDefaultButton == null)
        {
            eventSystemDefaultButton = buttons[0];
        }

        SetNavigation(buttons);

        if (eventSystemDefaultButton != null)
            SetEventSystemSelection();
        
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

    public void OnDisable()
    {
        PlayerController.instance.inputs.Menu.MenuInteractionOne.started -= MenuInteractionOne_performed;
    }

    private void MenuInteractionOne_performed(UnityEngine.InputSystem.InputAction.CallbackContext obj)
    {
        GameManager.Instance.pauseMenuManager.OpenMenu(fullscreenMenu);
        PlayerController.instance.inputs.Menu.MenuInteractionOne.Disable();
    }



}
