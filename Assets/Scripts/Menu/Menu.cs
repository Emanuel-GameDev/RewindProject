using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public List<NotificationDot> notificationsInThisMenu;
    public bool newNotificationsInThisMenu;


    private void Start()
    {
        gameObject.SetActive(false);
    }
}
