using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelSelectionButton : MenuButton
{
    [HideInInspector] public int checkpointToLoadIndex;
    [HideInInspector] public bool unlocked;


    public override void OnPointerClick(PointerEventData eventData)
    {
        if (unlocked)
        {
            LevelMaster.Instance.spawnPointId = checkpointToLoadIndex;
            GetComponentInParent<LevelDoor>().EnterDoor();
        }
    }

    // da rivedere
    void Update()
    {
        if (unlocked)
            GetComponent<SpriteRenderer>().color = Color.yellow;
        else
            GetComponent<SpriteRenderer>().color = Color.gray;
    }

   
}
