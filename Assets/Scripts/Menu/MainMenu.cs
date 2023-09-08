using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEngine;

public class MainMenu : Menu
{
    public override void Start()
    {
        gameObject.SetActive(true);
    }

    public void DeleteSaves()
    {
        DataSerializer.DeleteAll();
    }
}
