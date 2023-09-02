using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class MenuDiary : Menu
{
    public TextMeshProUGUI nameBox;
    public TextMeshProUGUI descriptionBox;

    public override void OnEnable()
    {
        nameBox.text = "";
        descriptionBox.text = "";

        MenuDiaryButton[] buttons = GetComponentsInChildren<MenuDiaryButton>();

        if (buttons.Length > 0 && eventSystemDefaultButton == null)
        {
            eventSystemDefaultButton = buttons[0];
        }

        SetNavigation(buttons);

        if (eventSystemDefaultButton != null)
            SetEventSystemSelection();

        
    }

    private static void SetNavigation(MenuDiaryButton[] buttons)
    {
        for (int i = 0; i < buttons.Length; i++)
        {
            Navigation data = buttons[i].navigation;
            data.mode = Navigation.Mode.Explicit;

            if (i == 0)
                data.selectOnUp = buttons[buttons.Length-1];
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

    

}
