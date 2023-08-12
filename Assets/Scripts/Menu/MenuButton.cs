using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;
using UnityEditor;
using UnityEngine.UI;

public class MenuButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    internal TextMeshProUGUI buttonTextUI;
    internal Image icon;

    [SerializeField] internal Color baseColor;
    [SerializeField] internal Color selectedColor;

    [SerializeField] internal UnityEvent OnClick;

    public bool locked = false;
    

    public virtual void OnPointerClick(PointerEventData eventData)
    {
        if (!locked)
            OnClick.Invoke();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        if (!locked)
        {
            if(buttonTextUI)
            buttonTextUI.color = selectedColor;
            if (icon)
                icon.color = selectedColor;
        }
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (!locked)
        {
            if (buttonTextUI)
                buttonTextUI.color = baseColor;
            if (icon)
                icon.color = baseColor;
        }

    }

    public virtual void OnEnable()
    {
        buttonTextUI = GetComponentInChildren<TextMeshProUGUI>();
        icon = GetComponentInChildren<Image>();

        if (locked)
        {
            if (buttonTextUI)
                buttonTextUI.color = Color.gray;
            if (icon)
                icon.color = Color.gray;
        }
        else
        {
            if (buttonTextUI)
                buttonTextUI.color = baseColor;
            if (icon)
                icon.color = baseColor;
        }
    }

    public void LoadLevel(SceneAsset levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad.name);
    }
}
