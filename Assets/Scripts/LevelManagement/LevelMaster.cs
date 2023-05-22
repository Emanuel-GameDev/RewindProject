using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class LevelMaster : MonoBehaviour
{
     public static LevelMaster Instance;

    public Dictionary<string, List<bool>> levels;

    [HideInInspector] public Transform spawnPoint;
    [HideInInspector] public Vector3 fastSpawnPoint;
    [HideInInspector] public int spawnPointId = 0;

    float fastRespawnTimer = 0;

    private void OnEnable()
    {
        levels = new Dictionary<string, List<bool>>();
    }

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }
    
    private void Update()
    {
        if(PlayerController.instance.grounded)
        {
            if (fastRespawnTimer < 0.8f)
                fastRespawnTimer += Time.deltaTime;
            else
            {
                fastSpawnPoint = PlayerController.instance.transform.position;
                fastRespawnTimer = 0;
            }
        }
        else
            fastRespawnTimer = 0;
    }

    public void Respawn()
    {
        Teleport(PlayerController.instance.gameObject, spawnPoint.position);
        PlayerController.instance.GetComponent<Damageable>().SetMaxHealth();
    }

    public void FastRespawn()
    {
        Teleport(PlayerController.instance.gameObject, fastSpawnPoint);
    }

    public void Teleport(GameObject objectToTeleport, Vector3 teleportPosition)
    {
        objectToTeleport.transform.position = new Vector3(teleportPosition.x,teleportPosition.y,0) ;
    }

    
}
