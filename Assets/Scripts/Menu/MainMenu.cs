using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEngine;

public class MainMenu : Menu
{
    [SerializeField] MenuButton continueButton;

    [SerializeField, SceneDetails]
    private SerializedScene hub;

    [SerializeField, SceneDetails]
    private SerializedScene intro;

    public override void Start()
    {
        gameObject.SetActive(true);

        if (DataSerializer.HasKey("HasSaving"))
            continueButton.gameObject.SetActive(true);
        else
            continueButton.gameObject.SetActive(false);

    }
    
    public void LoadHub()
    {
        LevelManager.instance.LoadLevel(hub.SceneName);   
    }

    public void LoadIntro()
    {
        LevelManager.instance.LoadLevel(intro.SceneName);
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
