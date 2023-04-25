using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLight : MonoBehaviour
{
    private void OnEnable()
    {
        GetComponent<SpriteRenderer>().color = Color.black;
    }

    public void Light()
    {
        GetComponent<SpriteRenderer>().color = Color.white;
    }


}
