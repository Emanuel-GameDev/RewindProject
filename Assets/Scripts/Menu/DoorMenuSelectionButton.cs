using System.Collections;
using System.Collections.Generic;
using TMPro;
using ToolBox.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoorMenuSelectionButton : LevelSelectionButton
{
    protected override void OnEnable()
    {
        buttonText = GetComponentInChildren<TextMeshPro>();
        buttonText.color = Color.white;

        if (locked)
            buttonText.color = Color.gray;
    }

    public void EnterDoorWithSelectedCheckpoint()
    {
        if (!locked)
        {
            DataSerializer.Save("CHECKPOINTIDTOLOAD", checkpointToLoadIndex);
            GetComponentInParent<LevelDoor>().EnterDoor();
        }
    }
}
