using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    public bool notificationInThisMenu;

    public bool unlocked = false;

    private void OnEnable()
    {
        
    }

    private void Start()
    {
        gameObject.SetActive(false);
        
    }

    public virtual void UnlockMenu()
    {
        if (unlocked)
            return;

        unlocked = true;
        notificationInThisMenu = true;

        foreach (Menu p in GetComponentsInParent<Menu>(true))
        {
            p.unlocked = true;
            p.notificationInThisMenu = true;
        }

    }

    public void CancelNotification()
    {
        foreach (Menu p in GetComponentsInParent<Menu>(true))
        {
            p.notificationInThisMenu=false;
        }
    }
}
