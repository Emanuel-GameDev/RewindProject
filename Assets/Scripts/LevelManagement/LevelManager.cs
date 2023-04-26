using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Transform startPoint; 

    [SerializeField] List<Checkpoint> checkpoints;

    public static Transform spawnPoint;

    public List<bool> checkpointsTaken;

    public Scene level;


    private void OnEnable()
    {
        level = SceneManager.GetActiveScene();

        if (!LevelMaster.instance.levels.ContainsKey(level.name))
        {
            LevelMaster.instance.levels.Add(level.name,checkpointsTaken);
            Debug.Log("c");
        }
        else
            checkpointsTaken = LevelMaster.instance.levels[level.name];

        PlayerMovementInput.instance.inputs.Player.Respawn.performed += OnRespawn;
        PubSub.Instance.RegisterFuncion(eMessageType.CheckpointVisited, SaveCheckpoints);
        //lastCheckpointVisited = checkpoints[levelData.lastCheckpointVisitedIndex];

        if (checkpointsTaken.Count==0)
        {
            checkpointsTaken.Add(false);
            checkpointsTaken.Add(false);
            checkpointsTaken.Add(false);
        }

        //checkpointsTaken.Clear();
        spawnPoint = transform;

        for (int i = 0; i < checkpointsTaken.Count; i++)
        {
            checkpoints[i].taken = checkpointsTaken[i];
        }
        
        SceneManager.sceneLoaded += OnLevelLoaded;

    }

    private void SaveCheckpoints(object obj)
    {
        for (int i = 0; i < checkpointsTaken.Count; i++)
        {
            checkpointsTaken[i] = checkpoints[i].taken;
        }

        LevelMaster.instance.levels[level.name] = checkpointsTaken;
        //PlayerPrefsExtra.SetList(level.name, checkpointsTaken);
    }

    private void OnLevelLoaded(Scene arg0, LoadSceneMode arg1)
    {
        Respawn();
    }

    

    private void OnDisable()
    {
        PlayerMovementInput.instance.inputs.Player.Respawn.performed -= OnRespawn;
    }

    private void OnRespawn(InputAction.CallbackContext obj)
    {
        Respawn();
    }

    public void Respawn()
    {
        //PlayerMovementInput.instance.gameObject.transform.position = spawnPoint.position;
    }
}
