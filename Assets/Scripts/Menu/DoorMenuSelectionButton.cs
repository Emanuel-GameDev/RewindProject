using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoorMenuSelectionButton : LevelSelectionButton
{
    public void EnterDoorWithSelectedCheckpoint()
    {
        if (!locked)
        {
            DataSerializer.Save("CHECKPOINTIDTOLOAD", checkpointToLoadIndex);
            GetComponentInParent<LevelDoor>().EnterDoor();
        }
    }
}
