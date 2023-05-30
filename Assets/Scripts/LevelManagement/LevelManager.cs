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


    public PlayerInputs inputs { get; private set; }
    

    private void OnEnable()
    {
        inputs = new PlayerInputs();
        inputs.Player.Enable();
        PubSub.Instance.RegisterFunction(EMessageType.CheckpointVisited, SaveCheckpoints);

        inputs.Player.Respawn.performed += OnRespawn;
        SceneManager.sceneLoaded += OnLevelLoaded;

    }

    private void SaveCheckpoints(object obj)
    {
        for (int i = 0; i < checkpointsTaken.Count; i++)
        {
            checkpointsTaken[i] = checkpoints[i].taken;
        }

        Checkpoint taken = (Checkpoint) obj;

        GameManager.Instance.levelMaster.spawnPoint = taken.transform;

        LevelMaster.Instance.levels[level.name] = checkpointsTaken;
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

        GetTakenCheckpoint();

        GetSpawnPoint();

        SetCheckpoint();

        GameManager.Instance.levelMaster.Respawn();
    }

    private void SetCheckpoint()
    {
        if (checkpoints.Count < 1)
            return;

        for (int i = 0; i < checkpointsTaken.Count; i++)
        {
            checkpoints[i].taken = checkpointsTaken[i];
        }

        checkpoints[0].taken = true;
    }

    private void GetSpawnPoint()
    {
        if (checkpoints.Count <1)
            GameManager.Instance.levelMaster.spawnPoint = transform;
        else if (LevelMaster.Instance.spawnPointId <= 0)
            GameManager.Instance.levelMaster.spawnPoint = checkpoints[0].transform;
        else
            GameManager.Instance.levelMaster.spawnPoint = checkpoints[LevelMaster.Instance.spawnPointId].transform;
    }

    private void GetTakenCheckpoint()
    {
        if (!LevelMaster.Instance.levels.ContainsKey(level.name))
        {
            foreach (Checkpoint check in checkpoints)
            {
                checkpointsTaken.Add(false);
            }

            LevelMaster.Instance.levels.Add(level.name, checkpointsTaken);
        }
        else
            checkpointsTaken = LevelMaster.Instance.levels[level.name];
    }

    private void OnRespawn(InputAction.CallbackContext obj)
    {
        GameManager.Instance.levelMaster.Respawn();
    }

    

}
