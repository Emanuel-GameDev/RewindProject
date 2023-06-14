using System;
using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] List<Checkpoint> checkpoints;
    public static LevelManager instance;

    public List<bool> checkpointsTaken;

    Scene level;


    public PlayerInputs inputs { get; private set; }
    

    private void OnEnable()
    {
        instance = this;
        inputs = new PlayerInputs();
        inputs.Player.Enable();
        PubSub.Instance.RegisterFunction(EMessageType.CheckpointVisited, SaveCheckpoints);

        SceneManager.sceneLoaded += OnLevelLoaded;
        inputs.Player.Respawn.performed += OnRespawn;

    }

    private void SaveCheckpoints(object obj)
    {
        for (int i = 0; i < checkpointsTaken.Count; i++)
        {
            checkpointsTaken[i] = checkpoints[i].taken;
        }

        DataSerializer.Save(level.name + "TAKENCHECKPOINTS", checkpointsTaken);
    }

    private void OnDisable()
    {
        inputs.Player.Disable();

        SceneManager.sceneLoaded -= OnLevelLoaded;
        inputs.Player.Respawn.performed -= OnRespawn;
    }

    private void OnLevelLoaded(Scene arg0, LoadSceneMode arg1)
    {
        level = SceneManager.GetActiveScene();

        //GetTakenCheckpoint();

        GetSpawnPoint();

        SetCheckpoint();

        Respawn();
    }

    private void SetCheckpoint()
    {
        if (checkpoints.Count < 1)
            return;

        if (DataSerializer.HasKey(level.name + "TAKENCHECKPOINTS"))
            checkpointsTaken = DataSerializer.Load<List<bool>>(level.name + "TAKENCHECKPOINTS");
        else
            checkpointsTaken = new List<bool>() { true,false,false };

        for (int i = 0; i < checkpointsTaken.Count; i++)
        {
            checkpoints[i].taken = checkpointsTaken[i];
        }

    }

    private void GetSpawnPoint()
    {
        DataSerializer.TryLoad("CHECKPOINTIDTOLOAD", out int idToLoad);
        DataSerializer.DeleteKey("CHECKPOINTIDTOLOAD");

        if (checkpoints.Count < 1)
            DataSerializer.Save("SPAWNPOINT", transform.position);
        else if (idToLoad <= 0)
            DataSerializer.Save("SPAWNPOINT", checkpoints[0].transform.position);
        else
            DataSerializer.Save("SPAWNPOINT", checkpoints[idToLoad].transform.position);
    }



    //private void GetTakenCheckpoint()
    //{
    //    if (!DataSerializer.TryLoad(level.name,out checkpoints)/*LevelMaster.Instance.levels.ContainsKey(level.name)*/)
    //    {
    //        foreach (Checkpoint check in checkpoints)
    //        {
    //            checkpointsTaken.Add(false);
    //        }

    //        LevelMaster.Instance.levels.Add(level.name, checkpointsTaken);
    //    }
    //    else
    //        checkpointsTaken = LevelMaster.Instance.levels[level.name];
    //}

    private void OnRespawn(InputAction.CallbackContext obj)
    {
        Respawn();
    }
    public void Respawn()
    {

        if(DataSerializer.TryLoad("SPAWNPOINT", out Vector3 spawnPoint))
        Teleport(PlayerController.instance.gameObject, spawnPoint);

        PlayerController.instance.GetComponent<Damageable>().SetMaxHealth();
    }

    public void FastRespawn()
    {
        Teleport(PlayerController.instance.gameObject, PlayerController.instance.fastSpawnPoint);
    }

    public void Teleport(GameObject objectToTeleport, Vector3 teleportPosition)
    {
        objectToTeleport.transform.position = new Vector3(teleportPosition.x, teleportPosition.y, 0);
    }

}
