using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelSelectionButton : MenuButton
{
    [HideInInspector] public int checkpointToLoadIndex;
     public bool unlocked =false;
    TextMeshPro buttonText;

    

    public override void OnPointerEnter(PointerEventData eventData)
    {
        buttonText.color = selectedColor;
    }

    public override void OnPointerExit(PointerEventData eventData)
    {
        buttonText.color = baseColor;
    }

    public override void OnEnable()
    {
        buttonText = GetComponentInChildren<TextMeshPro>();
        buttonText.color = baseColor;
    }



    // da rivedere
    void Update()
    {
        if (!unlocked)
            buttonText.color = Color.gray;
    }


}
