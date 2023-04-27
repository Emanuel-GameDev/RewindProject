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
        //PubSub.Instance.RegisterFuncion(eMessageType.CheckpointVisited, SaveCheckpoints);

        PlayerMovementInput.instance.inputs.Player.Respawn.performed += OnRespawn;
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
        PlayerMovementInput.instance.inputs.Player.Respawn.performed -= OnRespawn;
    }

    private void OnLevelLoaded(Scene arg0, LoadSceneMode arg1)
    {
        level = SceneManager.GetActiveScene();

        if (!LevelMaster.instance.levels.ContainsKey(level.name))
        {
            checkpointsTaken.Add(false);
            checkpointsTaken.Add(false);
            checkpointsTaken.Add(false);

            LevelMaster.instance.levels.Add(level.name, checkpointsTaken);
        }
        else
            checkpointsTaken = LevelMaster.instance.levels[level.name];


        if (LevelMaster.instance.spawnPointId > -1)
            spawnPoint = checkpoints[LevelMaster.instance.spawnPointId].transform;
        else
            spawnPoint = transform;

        for (int i = 0; i < checkpointsTaken.Count; i++)
        {
            checkpoints[i].taken = checkpointsTaken[i];
        }

        Respawn();
    }

    private void OnRespawn(InputAction.CallbackContext obj)
    {
        Respawn();
    }

    public void Respawn()
    {
        PlayerMovementInput.instance.gameObject.transform.position = spawnPoint.position;
    }

}
