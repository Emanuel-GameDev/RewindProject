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

    [HideInInspector]
    public bool debug;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        if (transform.parent != null)
            transform.parent = null;

        DontDestroyOnLoad(gameObject);
    }
}
