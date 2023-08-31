using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MenuDiaryButton : MenuButton
{
    MenuDiary menuDiary;
    

    protected override void OnEnable()
    {
        base.OnEnable();
        menuDiary = GetComponentInParent<MenuDiary>();
    }

    public void ChangeMenu()
    {
        menuDiary.nameBox.text = GetComponentInChildren<TextMeshProUGUI>().text;
        menuDiary.descriptionBox.text = GetComponent<DiaryButtonData>().text;
    }
}
