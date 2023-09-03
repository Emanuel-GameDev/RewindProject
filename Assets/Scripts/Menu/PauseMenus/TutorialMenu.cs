using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialMenu : Menu
{
    public TextMeshProUGUI descriptionBox;
    public VideoPlayer videoPlayer;
    public MenuButton videoButton;
    public MenuFullscreen fullscreenMenu;
    public VideoPlayer fullscreenVideoPlayer;

    public override void OnEnable()
    {
        MenuVideoButton[] buttons = GetComponentsInChildren<MenuVideoButton>();

        if (buttons.Length <= 0)
        {
            descriptionBox.text = "";
            videoPlayer.gameObject.SetActive(false);
            videoButton.gameObject.SetActive(false);
        }
        else
            videoPlayer.Play();


        if (GetComponentsInChildren<MenuVideoButton>().Length > 0)
            PlayerController.instance.inputs.Menu.MenuInteractionOne.started += MenuInteractionOne_performed;


        
        if (buttons.Length > 0 && eventSystemDefaultButton == null)
        {
            eventSystemDefaultButton = buttons[0];
        }

        SetNavigation(buttons);

        if (eventSystemDefaultButton != null)
            SetEventSystemSelection();
    }

    private void SetNavigation(MenuVideoButton[] buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Navigation data = buttons[i].navigation;
            data.mode = Navigation.Mode.Explicit;

            if (i == 0)
                data.selectOnUp = buttons[buttons.Length - 1];
            else
                data.selectOnUp = buttons[i - 1];

            if (i == buttons.Length - 1)
            {
                data.selectOnDown = buttons[0];
            }
            else
                data.selectOnDown = buttons[i + 1];


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
