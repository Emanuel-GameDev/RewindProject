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
    TextMeshProUGUI buttonTextUI;

    [SerializeField] internal Color baseColor;
    [SerializeField] internal Color selectedColor;

    [SerializeField] UnityEvent OnClick;


    public virtual void OnPointerClick(PointerEventData eventData)
    {
        OnClick.Invoke();
    }

    public virtual void OnPointerEnter(PointerEventData eventData)
    {
        buttonTextUI.color = selectedColor;
    }

    public virtual void OnPointerExit(PointerEventData eventData)
    {
        buttonTextUI.color = baseColor;
    }


    public virtual void OnEnable()
    {
        buttonTextUI = GetComponentInChildren<TextMeshProUGUI>();
        buttonTextUI.color = baseColor;
    }

    public void LoadLevel(SceneAsset levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad.name);
    }
}
