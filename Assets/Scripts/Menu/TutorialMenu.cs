using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
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
        base.OnEnable();
        videoPlayer.gameObject.SetActive(false);
        videoButton.gameObject.SetActive(false);
        descriptionBox.text = "";

        if (GetComponentsInChildren<MenuVideoButton>().Length > 0)
            PlayerController.instance.inputs.Menu.MenuInteractionOne.started += MenuInteractionOne_performed;
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
