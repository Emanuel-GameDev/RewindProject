using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LevelLight : MonoBehaviour
{
    public bool light=false;
    private void OnEnable()
    {
        GetComponent<SpriteRenderer>().color = Color.black;
    }

    public void Light()
    {
        light = true;
        GetComponent<SpriteRenderer>().color = Color.white;
    }


}
