using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class MenuWithSubmenuButton : MenuButton
{
    [SerializeField] Menu submenuToOpen;
    NotificationDot notificationDot;

    public override void OnEnable()
    {
        buttonTextUI = GetComponentInChildren<TextMeshProUGUI>();

        notificationDot = GetComponentInChildren<NotificationDot>(true);

        //if (!submenuToOpen.unlocked)
        //{
        //    buttonTextUI.color = Color.gray;
        //}
        //else
        //{
        //    buttonTextUI.color = baseColor;
        //}
    }

    private void Update()
    {
        if (notificationDot != null)
        {
            if (submenuToOpen.notificationInThisMenu)
                notificationDot.gameObject.SetActive(true);
            else
                notificationDot.gameObject.SetActive(false);
        }
    }

    public override void OnPointerClick(PointerEventData eventData)
    {
        if (notificationDot != null)
        {
            if (submenuToOpen.notificationInThisMenu)
            {
                notificationDot.gameObject.SetActive(true);
            }
        }

        if (submenuToOpen.unlocked)
            OnClick.Invoke();

    }

    public override void OnPointerEnter(PointerEventData eventData)
    {
        if (submenuToOpen.unlocked)
            buttonTextUI.color = selectedColor;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        if (submenuToOpen.unlocked)
            buttonTextUI.color = baseColor;
    }

    public void OpenSubmenu()
    {
        submenuToOpen.gameObject.SetActive(true);
    }
}
