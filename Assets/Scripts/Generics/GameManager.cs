using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [Header("Managers")]
    public CameraManager cameraManager;
    public AbilityManager abilityManager;
    public MenuManager pauseMenuManager;
    public UIManager uiManager;

    [Tooltip("Enable only for debugging, this bool set to true on a serious test will make scripts not work")]
    public bool debug;

    public bool dontDestroyOnLoad = false;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (transform.parent != null)
            transform.parent = null;

        if(dontDestroyOnLoad)
            DontDestroyOnLoad(gameObject);
    }
}
