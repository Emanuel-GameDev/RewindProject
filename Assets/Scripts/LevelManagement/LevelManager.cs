using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    [SerializeField] List<Checkpoint> checkpoints;

    public List<bool> checkpointsTaken;

    Scene level;

    Transform spawnPoint;

    private void OnEnable()
    {
        level = SceneManager.GetActiveScene();

        if (!LevelMaster.instance.levels.ContainsKey(level.name))
        {
            LevelMaster.instance.levels.Add(level.name,checkpointsTaken);
        }
        else
            checkpointsTaken = LevelMaster.instance.levels[level.name];

        
        PubSub.Instance.RegisterFuncion(eMessageType.CheckpointVisited, SaveCheckpoints);

        PlayerMovementInput.instance.inputs.Player.Respawn.performed += OnRespawn;
        SceneManager.sceneLoaded += OnLevelLoaded;

        if (checkpointsTaken.Count==0)
        {
            checkpointsTaken.Add(false);
            checkpointsTaken.Add(false);
            checkpointsTaken.Add(false);
        }


        for (int i = 0; i < checkpointsTaken.Count; i++)
        {
            checkpoints[i].taken = checkpointsTaken[i];
        }
        
        

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
        if (LevelMaster.instance.spawnPointId > -1)
            spawnPoint = checkpoints[LevelMaster.instance.spawnPointId].transform;
        else
            spawnPoint = transform;

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
