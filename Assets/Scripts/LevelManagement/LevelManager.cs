using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    public Transform startPoint; 

    public List<Checkpoint> checkpoints;

    public static Transform spawnPoint;



    public List<bool> checkpointsTaken;

    private void OnEnable()
    {
        PlayerMovementInput.instance.inputs.Player.Respawn.performed += OnRespawn;
        PubSub.Instance.RegisterFuncion(eMessageType.CheckpointVisited, SaveCheckpoints);
        //lastCheckpointVisited = checkpoints[levelData.lastCheckpointVisitedIndex];

        if (PlayerPrefsExtra.GetList<bool>("a").Count > 0)
        {
            checkpointsTaken = PlayerPrefsExtra.GetList<bool>("a", checkpointsTaken);
            Debug.Log("i");
        }
        else
        {
            checkpointsTaken.Add(false);
            checkpointsTaken.Add(false);
            checkpointsTaken.Add(false);
            checkpointsTaken.Add(false);
            Debug.Log("a");
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

        PlayerPrefsExtra.SetList("a", checkpointsTaken);
        Debug.Log(SceneManager.GetActiveScene().name);
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
