using System;
using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEditor;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] List<Checkpoint> checkpoints;
    public static LevelManager instance;

    [HideInInspector] public List<bool> checkpointsTaken;

    Scene level;

    public bool deleteSavesOnEditorQuit=false; 
    public PlayerInputs inputs { get; private set; }
    

    private void OnEnable()
    {
        instance = this;
        inputs = new PlayerInputs();
        inputs.Player.Enable();
        PubSub.Instance.RegisterFunction(EMessageType.CheckpointVisited, SaveCheckpoints);

        SceneManager.sceneLoaded += OnLevelLoaded;
        inputs.Player.Respawn.performed += OnRespawn;

        DataSerializer.FileSaving += DeleteSaves;
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

        DataSerializer.FileSaving -= DeleteSaves;
    }

    private void DeleteSaves()
    {
        if(deleteSavesOnEditorQuit && EditorApplication.isPlaying)
        DataSerializer.DeleteAll();
    }

    private void OnLevelLoaded(Scene arg0, LoadSceneMode arg1)
    {
        level = SceneManager.GetActiveScene();


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
        if (!DataSerializer.TryLoad("CHECKPOINTIDTOLOAD", out int idToLoad))
            return;

        DataSerializer.DeleteKey("CHECKPOINTIDTOLOAD");



        if (checkpoints.Count < 1)
            DataSerializer.Save("SPAWNPOINT", transform.position);
        else if (idToLoad <= 0)
            DataSerializer.Save("SPAWNPOINT", checkpoints[0].transform.position);
        else
            DataSerializer.Save("SPAWNPOINT", checkpoints[idToLoad].transform.position);
    }



    private void OnRespawn(InputAction.CallbackContext obj)
    {
        Respawn();
    }

    public void Respawn()
    {
        if(DataSerializer.TryLoad("SPAWNPOINT", out Vector3 spawnPoint))
            Teleport(PlayerController.instance.gameObject, spawnPoint);
        else
            Teleport(PlayerController.instance.gameObject, checkpoints[0].transform.position);

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
