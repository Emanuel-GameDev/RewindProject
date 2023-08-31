using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.EventSystems;

public class MenuDiary : Menu
{
    public TextMeshProUGUI nameBox;
    public TextMeshProUGUI descriptionBox;

    public override void OnEnable()
    {
        if (startUnlocked)
            UnlockMenu();



        if (eventSystemDefaultButton != null)
        {

            SetEventSystemSelection(eventSystemDefaultButton);
        }
    }

    public override void SetEventSystemSelection(MenuButton selection)
    {
        EventSystem.current.SetSelectedGameObject(selection.gameObject);
    }

}
