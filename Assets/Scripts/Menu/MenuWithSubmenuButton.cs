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

        if (!submenuToOpen.unlocked)
        {
            if (notificationDot != null)
                notificationDot.gameObject.SetActive(false);
            buttonTextUI.color = Color.gray;
        }
        else
        {
            if (notificationDot != null)
                notificationDot.gameObject.SetActive(true);
            buttonTextUI.color = baseColor;
        }

        if (submenuToOpen.notificationsInThisMenu.Count > 0)
            notificationDot.newNotification = true;

    }

    public override void OnPointerClick(PointerEventData eventData)
    {
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
