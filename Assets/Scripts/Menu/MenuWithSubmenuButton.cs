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

        if (locked)
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
        if (!locked)
            OnClick.Invoke();
    }

    public void OpenSubmenu()
    {
        submenuToOpen.gameObject.SetActive(true);
    }
}
