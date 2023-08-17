using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.EventSystems;

public class LevelSelectionButton : MenuButton
{
    [HideInInspector] public int checkpointToLoadIndex;

    [HideInInspector] public TextMeshPro buttonText;

    //public override void OnPointerEnter(PointerEventData eventData)
    //{
    //    if(!locked)
    //    buttonText.color = selectedColor;
    //}

    //public override void OnPointerExit(PointerEventData eventData)
    //{
    //    if (!locked)
    //        buttonText.color = baseColor;
    //}

    //protected override void OnEnable()
    //{
    //    buttonText = GetComponentInChildren<TextMeshPro>();
    //    buttonText.color = baseColor;

    //    if (locked)
    //        buttonText.color = Color.gray;
    //}



}
