using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelMaster : MonoBehaviour
{
    static LevelMaster _instance;
    public static LevelMaster Instance
    {
        get
        {
            if (_instance != null)
                return _instance;

            GameObject levelMasterObject = new GameObject("LevelMaster");

            _instance = levelMasterObject.AddComponent<LevelMaster>();

            return _instance;
        }
    }

    public Dictionary<string, List<bool>> levels;

    public Transform spawnPoint;
    public int spawnPointId = 0;

    private void OnEnable()
    {
        levels = new Dictionary<string, List<bool>>();
    }

    private void Awake()
    {

        DontDestroyOnLoad(gameObject);
    }
}
