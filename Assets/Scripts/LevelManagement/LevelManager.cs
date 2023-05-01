using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] List<Checkpoint> checkpoints;

    [HideInInspector]
    public List<bool> checkpointsTaken;

    Scene level;

    Transform spawnPoint;

    private void OnEnable()
    {
        PubSub.Instance.RegisterFunction(EMessageType.CheckpointVisited, SaveCheckpoints);

        PlayerController.instance.inputs.Player.Respawn.performed += OnRespawn;
        SceneManager.sceneLoaded += OnLevelLoaded;
    }

    private void SaveCheckpoints(object obj)
    {
        for (int i = 0; i < checkpointsTaken.Count; i++)
        {
            checkpointsTaken[i] = checkpoints[i].taken;
        }

        Checkpoint taken = (Checkpoint)obj;

        spawnPoint = taken.transform;

        LevelMaster.instance.levels[level.name] = checkpointsTaken;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnLevelLoaded;
        PlayerController.instance.inputs.Player.Respawn.performed -= OnRespawn;
    }

    private void OnLevelLoaded(Scene arg0, LoadSceneMode arg1)
    {
        level = SceneManager.GetActiveScene();

        GetTakenCheckpoint();

        GetSpawnPoint();

        SetCheckpoint();

        Respawn();
    }

    private void SetCheckpoint()
    {
        for (int i = 0; i < checkpointsTaken.Count; i++)
        {
            checkpoints[i].taken = checkpointsTaken[i];
        }
    }

    private void GetSpawnPoint()
    {
        if (LevelMaster.instance.spawnPointId <= 0)
            spawnPoint = checkpoints[0].transform;
        else
            spawnPoint = checkpoints[LevelMaster.instance.spawnPointId].transform;
    }

    private void GetTakenCheckpoint()
    {
        if (!LevelMaster.instance.levels.ContainsKey(level.name))
        {
            foreach (Checkpoint check in checkpoints)
            {
                checkpointsTaken.Add(false);
            }

            LevelMaster.instance.levels.Add(level.name, checkpointsTaken);
        }
        else
            checkpointsTaken = LevelMaster.instance.levels[level.name];
    }

    private void OnRespawn(InputAction.CallbackContext obj)
    {
        Respawn();
    }

    public void Respawn()
    {
        PlayerController.instance.gameObject.transform.position = spawnPoint.position;
    }

}
