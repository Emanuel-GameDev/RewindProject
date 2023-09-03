using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuCardButton : MenuButton
{
    Ability ability;
    MenuCards menuCards;

    protected override void OnEnable()
    {
        base.OnEnable();

       
        ability = GetComponent<CardMenuData>().ability;
        GetComponent<CardMenuData>().smallIcon.sprite = ability.smallIcon;
        buttonTextUI.text = ability.name;
        menuCards = GetComponentInParent<MenuCards>();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        onClick?.Invoke();
    }


    public void ChangeCardMenu()
    {
        if (!menuCards.artworkSelection.gameObject.activeSelf)
        {
            menuCards.artworkSelection.gameObject.SetActive(true);
            menuCards.descriptionSelection.gameObject.SetActive(true);
            menuCards.openTutorialMenuButton.gameObject.SetActive(true);
        }

        menuCards.cardName.text = ability.name;
        menuCards.cardImageBox.sprite = ability.icon;
        menuCards.descriptionBox.text = GetComponentInChildren<Text>().text;
        menuCards.fullscreenCardImageBox.sprite = ability.icon;
        menuCards.cardIcon.sprite = ability.smallIcon;

        menuCards.eventSystemDefaultButton = this;

        if (GetComponent<CardMenuData>().tutorialButton == null)
        {
            menuCards.openTutorialMenuButton.gameObject.SetActive(false);
            PlayerController.instance.inputs.Menu.MenuInteractionTwo.started -= menuCards.MenuInteractionTwo_started;
            return;
        }

        if (GetComponent<CardMenuData>().tutorialButton.GetComponent<SaveUniqueObj>().objActive)
        {
            PlayerController.instance.inputs.Menu.MenuInteractionTwo.started += menuCards.MenuInteractionTwo_started;
            menuCards.openTutorialMenuButton.gameObject.SetActive(true);
            menuCards.goToTutorialButton = GetComponent<CardMenuData>().tutorialButton;
        }
        else
        {
            menuCards.openTutorialMenuButton.gameObject.SetActive(false);
            PlayerController.instance.inputs.Menu.MenuInteractionTwo.started -= menuCards.MenuInteractionTwo_started;
        }


    }
}
