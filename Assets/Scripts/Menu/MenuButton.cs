using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;
using UnityEditor;

public class MenuButton : MonoBehaviour, IPointerClickHandler, IPointerEnterHandler, IPointerExitHandler
{
    internal TextMeshProUGUI buttonTextUI;

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
            buttonTextUI.color = selectedColor;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        if (!locked)
            buttonTextUI.color = baseColor;
    }

    public virtual void OnEnable()
    {
        buttonTextUI = GetComponentInChildren<TextMeshProUGUI>();

        if (locked)
        {
            buttonTextUI.color = Color.gray;
        }
        else
        {
            buttonTextUI.color = baseColor;
        }
    }

    public void LoadLevel(SceneAsset levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad.name);
    }
}
