using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEngine;

public class MainMenu : Menu
{
    [SerializeField] MenuButton continueButton;

    public override void Start()
    {
        gameObject.SetActive(true);
        if (DataSerializer.HasKey("HasSaving"))
            continueButton.gameObject.SetActive(true);
        else
            continueButton.gameObject.SetActive(false);

    }

    public void GameStartedOnce()
    {
        DataSerializer.Save("HasSaving", true);
    }

    public void DeleteSaves()
    {
        DataSerializer.DeleteAll();
    }
}
