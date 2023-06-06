using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class DataPersistanceManager : MonoBehaviour
{
    public static DataPersistanceManager instance { get; private set; }

    GameData gameData;
    List<IDataPersistance> dataPersistanceObjects;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        this.dataPersistanceObjects = FindAllDataPersistanceObject();
        LoadGame();
    }

    public void NewGame()
    {
        this.gameData = new GameData();
    }

    public void LoadGame()
    {
        //TODO load saved data

        if(this.gameData == null)
        {
            Debug.Log("No data was found");
            NewGame();
        }

        //toDo push loaded data to all scripts needed
        foreach(IDataPersistance data in dataPersistanceObjects)
        {
            data.LoadData(gameData);
        }
        Debug.Log("loaded");
    }

    public void SaveGame()
    {
        // to do pass data to other scripts to update it
        foreach(IDataPersistance data in dataPersistanceObjects)
        {
            data.SaveData(ref gameData);
        }

        Debug.Log("saved");
        // saving the data using data handler
    }

    private void OnApplicationQuit()
    {
        SaveGame();
    }

    List<IDataPersistance> FindAllDataPersistanceObject()
    {
        IEnumerable<IDataPersistance> dataPersistanceObjects = FindObjectsOfType<MonoBehaviour>().OfType<IDataPersistance>();

        return new List<IDataPersistance>(dataPersistanceObjects);
    }
}
