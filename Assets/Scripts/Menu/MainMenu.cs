using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEngine;

public class MainMenu : Menu
{
    [SerializeField] MenuButton continueButton;
    [SerializeField] string hubName;

    public override void Start()
    {
        gameObject.SetActive(true);

        if (DataSerializer.HasKey("HasSaving"))
            continueButton.gameObject.SetActive(true);
        else
            continueButton.gameObject.SetActive(false);

    }
    
    public void LoadLevel(string hubName)
    {
        LevelManager.instance.LoadLevel(hubName);   
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
