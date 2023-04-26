using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelSelectionButton : MonoBehaviour , IPointerClickHandler,IPointerEnterHandler,IPointerExitHandler
{
    public int checkpointToLoadIndex;
    RectTransform rect;
    public bool unlocked;


    public void OnPointerClick(PointerEventData eventData)
    {
        if (unlocked)
        {

        //GetComponentInParent<LevelDoor>().level.lastCheckpointVisitedIndex = checkpointToLoadIndex;
        GetComponentInParent<LevelDoor>().EnterDoor();
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (unlocked)
            GetComponent<RectTransform>().localScale = new Vector3(GetComponent<RectTransform>().localScale.x * 1.1f, rect.localScale.y * 1.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        if (unlocked)
            GetComponent<RectTransform>().localScale = new Vector3(GetComponent<RectTransform>().localScale.x * 0.9f, rect.localScale.y * 0.9f);
    }

    

    void Update()
    {
        if (unlocked)
            GetComponent<SpriteRenderer>().color = Color.yellow;
        else
            GetComponent<SpriteRenderer>().color = Color.gray;
    }

   
}
