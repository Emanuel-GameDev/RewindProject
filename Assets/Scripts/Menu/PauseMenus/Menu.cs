using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;

public class Menu : MonoBehaviour
{

    [SerializeField] public Menu nextTab;
    [SerializeField] public Menu previousTab;

    [SerializeField] public MenuButton eventSystemDefaultButton;

    [HideInInspector] public bool notificationInThisMenu=false;
    [HideInInspector] public bool unlocked = false;

    public virtual void OnEnable()
    {
        if(eventSystemDefaultButton!=null)
            SetEventSystemSelection();
    }

    
    public virtual void SetEventSystemSelection()
    {
        EventSystem.current.SetSelectedGameObject(eventSystemDefaultButton.gameObject);
    }

    private void Awake()
    {
        DataSerializer.TryLoad("PAUSEMENULOCK" + gameObject.name, out unlocked);
        DataSerializer.TryLoad("PAUSEMENUNOT" + gameObject.name, out notificationInThisMenu);
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


        DataSerializer.Save("PAUSEMENULOCK" + gameObject.name, unlocked);
        DataSerializer.Save("PAUSEMENUNOT" + gameObject.name, notificationInThisMenu);

        foreach (Menu p in GetComponentsInParent<Menu>(true))
        {
            p.unlocked = true;
            p.notificationInThisMenu = true;

            DataSerializer.Save("PAUSEMENULOCK" + p.gameObject.name, unlocked);
            DataSerializer.Save("PAUSEMENUNOT" + p.gameObject.name, notificationInThisMenu);
        }

    }

    public void CancelNotification()
    {
        foreach (Menu p in GetComponentsInParent<Menu>(true))
        {
            p.notificationInThisMenu=false;
        }
    }

    private void OnDisable()
    {
        DataSerializer.Save("PAUSEMENULOCK" + gameObject.name,unlocked);
        DataSerializer.Save("PAUSEMENUNOT" + gameObject.name, notificationInThisMenu);
    }
}
