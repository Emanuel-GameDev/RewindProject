using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DoorMenuSelectionButton : LevelSelectionButton
{
    public void EnterDoorWithSelectedCheckpoint()
    {
        if (!locked)
        {
            LevelMaster.Instance.spawnPointId = checkpointToLoadIndex;
            GetComponentInParent<LevelDoor>().EnterDoor();
        }
    }
}
