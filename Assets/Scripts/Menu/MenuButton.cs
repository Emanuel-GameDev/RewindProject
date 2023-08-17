using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public class MenuButton : Button
{
    internal TextMeshProUGUI buttonTextUI;
    internal Image icon;

    //[SerializeField] internal Color baseColor;
    //[SerializeField] internal Color selectedColor;

    //[SerializeField] internal UnityEvent OnClick;

    public bool locked = false;
    

    //public override void OnPointerClick(PointerEventData eventData)
    //{
    //    if (!locked)
    //        OnClick.Invoke();
    //}

    //public override void OnPointerEnter(PointerEventData eventData)
    //{
    //    if (!locked)
    //    {
    //        SelectButton();
    //    }
    //}

    //private void SelectButton()
    //{
    //    if (buttonTextUI)
    //        buttonTextUI.color = selectedColor;
    //    if (icon)
    //        icon.color = selectedColor;
    //}

    //public override void OnPointerExit(PointerEventData eventData)
    //{
    //    if (!locked)
    //    {
    //        DeselectButton();
    //    }

    //}

    //private void DeselectButton()
    //{
    //    if (buttonTextUI)
    //        buttonTextUI.color = baseColor;
    //    if (icon)
    //        icon.color = baseColor;
    //}

    protected override void OnEnable()
    {
        base.OnEnable();
        buttonTextUI = GetComponentInChildren<TextMeshProUGUI>();
        icon = GetComponentInChildren<Image>();

        //if (locked)
        //{
        //    if (buttonTextUI)
        //        buttonTextUI.color = Color.gray;
        //    if (icon)
        //        icon.color = Color.gray;
        //}
        //else
        //{
        //    //DeselectButton();
        //}
    }

    public void LoadLevel(SceneAsset levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad.name);
    }
}
