using System.Collections;
using System.Collections.Generic;
using UnityEngine;
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

    public void UnlockButton()
    {
        gameObject.SetActive(true);
        GetComponent<SaveObjState>().ChangeObjectStateOnReload(true);
    }

    public void ChangeCardMenu()
    {
        if (!menuCards.artworkSelection.gameObject.activeSelf)
        {
            menuCards.artworkSelection.gameObject.SetActive(true);
            menuCards.descriptionSelection.gameObject.SetActive(true);
        }

        menuCards.cardName.text = ability.name;
        menuCards.cardImageBox.sprite = ability.icon;
        menuCards.descriptionBox.text = ability.menuDescription;
        menuCards.fullscreenCardImageBox.sprite = ability.icon;
        menuCards.cardIcon.sprite = ability.smallIcon;
    }
}
