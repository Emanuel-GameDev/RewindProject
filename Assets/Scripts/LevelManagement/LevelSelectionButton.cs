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
        //GetComponentInParent<LevelDoor>().level.lastCheckpointVisitedIndex = checkpointToLoadIndex;
        GetComponentInParent<LevelDoor>().EnterDoor();
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        rect.localScale = new Vector3(rect.localScale.x * 1.1f, rect.localScale.y * 1.1f);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        rect.localScale = new Vector3(rect.localScale.x * 0.9f, rect.localScale.y * 0.9f);
    }

    void Start()
    {
        rect = GetComponent<RectTransform>();
    }

   
}
