using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelMaster : MonoBehaviour
{
    public  static LevelMaster instance;
    public Dictionary<string, List<bool>> levels;

    
    private void OnEnable()
    {
        levels = new Dictionary<string, List<bool>>();
    }

    private void Awake()
    {
        if (instance == null)
            instance = this;
        else
            Destroy(gameObject);

        DontDestroyOnLoad(gameObject);
    }


}
