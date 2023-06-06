using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandler 
{
    string dataDirPath = "";
    string dataFileName = "";

    public FileDataHandler(string dataDirPath, string dataFileName)
    {
        this.dataDirPath = dataDirPath;
        this.dataFileName = dataFileName;
    }

    //public GameData Load()
    //{

    //}

    public void Save(GameData data)
    {
        string fullPath = Path.Combine(dataDirPath, dataFileName);

        //try
        //{
        //    Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

        //    string dataToStore = JsonUtility.ToJson(data, true);// data da salvare in json

        //    using (FileStream stream = new FileStream(fullPath, FileMode.Create)) ;
        //}
        //catch(Exception e)
        //{
        //    Debug.LogError("Error occured when trying to save to file: " + fullPath + "\n" + e);
        //}

    }
}
