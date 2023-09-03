using ToolBox.Serialization;

public class DoorMenuSelectionButton : LevelSelectionButton
{

    public void EnterDoorWithSelectedCheckpoint()
    {
        DataSerializer.Save("CHECKPOINTIDTOLOAD", checkpointToLoadIndex);
        GetComponentInParent<LevelDoor>().EnterDoor();
    }
}
