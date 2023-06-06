using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GameData
{
    public bool dialogue;

    public GameData()//valore dei dati quando non si hanno salvataggi
    {
        this.dialogue = false;
    }



}
