using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityMenuSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler
{
    [HideInInspector] public Vector2 overedAddition;
    [HideInInspector] public float enlargmentFactor = 1.1f;

    private Vector2 startSize;

    private void Start()
    {
        startSize = transform.localScale;
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
        transform.localScale = startSize * enlargmentFactor;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        transform.localScale = startSize;
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        throw new System.NotImplementedException();
    }
}
