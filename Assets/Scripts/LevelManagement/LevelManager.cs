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
    public List<Checkpoint> checkpoints;
    public static LevelManager instance;

    [HideInInspector] public List<bool> checkpointsTaken;

    Scene level;

    public bool deleteSavesOnEditorQuit=false; 
    public PlayerInputs inputs { get; private set; }

    [SerializeField] bool hideCursor;

    [Header("Loadings")]
    [SerializeField] GameObject loadingScreen;
    [SerializeField] bool fadeInOnLevelUnload = true;
    [SerializeField] bool fadeOutOnLevelLoad = true;

    private void OnEnable()
    {
        Time.timeScale = 1;

        instance = this;

        level = SceneManager.GetActiveScene();

        GetSpawnPoint();

        if(PlayerController.instance)
            Respawn();

        if (loadingScreen)
        {
            if (fadeOutOnLevelLoad)
            {
                loadingScreen.SetActive(true);
                GetComponent<Animator>().SetTrigger("FadeOut");
            }
            else
            {
                loadingScreen.SetActive(false);
            }


        }

        if (PlayerController.instance)
        {
            inputs = PlayerController.instance.inputs;
            inputs.Player.Enable();
            PubSub.Instance.RegisterFunction(EMessageType.CheckpointVisited, SaveCheckpoints);

        }

        DataSerializer.FileSaving += DeleteSaves;
    }
    
    private void Start()
    {
        SetCheckpoint();
        // Locks the cursor
        if (hideCursor)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
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
        if (PlayerController.instance)
        {
            inputs.Player.Disable();

        }
        

        DataSerializer.FileSaving -= DeleteSaves;
    }

    private void DeleteSaves()
    {
        if(deleteSavesOnEditorQuit && EditorApplication.isPlaying)
        DataSerializer.DeleteAll();
    }


    private void SetCheckpoint()
    {
        if (checkpoints.Count < 1)
            return;

        if (DataSerializer.HasKey(level.name + "TAKENCHECKPOINTS"))
            checkpointsTaken = DataSerializer.Load<List<bool>>(level.name + "TAKENCHECKPOINTS");
        else
            checkpointsTaken = new List<bool>() { true,false,false,false,false };

        for (int i = 0; i < checkpointsTaken.Count; i++)
        {
            checkpoints[i].taken = checkpointsTaken[i];
        }

    }

    private void GetSpawnPoint()
    {
        DataSerializer.TryLoad("CHECKPOINTIDTOLOAD", out int idToLoad);
       


        if (checkpoints.Count < 1)
            DataSerializer.Save("SPAWNPOINT", transform.position);
        else if (idToLoad <= 0)
            DataSerializer.Save("SPAWNPOINT", checkpoints[0].transform.position);
        else
            DataSerializer.Save("SPAWNPOINT", checkpoints[idToLoad].transform.position);
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

    public void LoadingScreenState()
    {
        if(loadingScreen)
        loadingScreen.SetActive(false);
    }

    public void LoadLevel(SceneAsset levelToLoad)
    {
        if (loadingScreen && fadeInOnLevelUnload)
        {
            loadingScreen.SetActive(true);
            GetComponent<Animator>().SetTrigger("FadeIn");
        }
        StartCoroutine(LoadSceneAsynchronously(levelToLoad));
    }



    public IEnumerator LoadSceneAsynchronously(SceneAsset levelToLoad)
    {
        if(PlayerController.instance)
            PlayerController.instance.inputs.Disable();

        AsyncOperation operation = SceneManager.LoadSceneAsync(levelToLoad.name);

        AudioSource[] components = FindObjectsOfType<AudioSource>();

        foreach (AudioSource a in components)
        {
            a.Stop();
        }

        while (!operation.isDone)
        {
            yield return null;
        }
    }


}
