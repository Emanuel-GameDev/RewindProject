using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class NotificationDot : MonoBehaviour
{
    [HideInInspector] public bool newNotification = true;
    Image dotImage;

    private void OnEnable()
    {
        dotImage = GetComponent<Image>();

        if (newNotification)
            dotImage.enabled = true;
        else
            dotImage.enabled = false;
    }

}
