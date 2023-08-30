using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuCards : Menu
{
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI descriptionBox;
    public Image cardImageBox;
    public Image fullscreenCardImageBox;
    public GameObject artworkSelection;
    public MenuFullscreen fullscreenMenu;
    public Image cardIcon;
    public GameObject descriptionSelection;



    public override void OnEnable()
    {
        base.OnEnable();
        cardName.text = "";
        descriptionBox.text = "";
        fullscreenCardImageBox.sprite = null;
        artworkSelection.gameObject.SetActive(false);
        descriptionSelection.gameObject.SetActive(false);

        if(GetComponentsInChildren<MenuCardButton>().Length>0)
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
