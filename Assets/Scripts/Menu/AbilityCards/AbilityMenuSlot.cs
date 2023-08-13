using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityMenuSlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerDownHandler, IPointerUpHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [HideInInspector] public Vector2 posOveredAddition;
    [HideInInspector] public float enlargmentFactor = 1.1f;
    [HideInInspector] public string textName;
    [HideInInspector] public string textDescription;
    [HideInInspector] public AbilityMenu abMenu;

    private GameObject childGO;
    private Vector2 startSize;
    private Vector2 startPos;
    private RectTransform parentAfterDrag;
    private bool isDragging = false;
    private bool isPointerDown = false;

    #region UnityFunctions
    private void Start()
    {
        childGO = transform.GetChild(0).gameObject;
        startSize = childGO.transform.localScale;
    }

    #endregion

    #region PointerRelated

    private void TriggerCardOvered(bool mode)
    {
        if (mode)
        {
            // Set Pos
            startPos = childGO.GetComponent<RectTransform>().localPosition;
            childGO.GetComponent<RectTransform>().localPosition = new Vector2(childGO.GetComponent<RectTransform>().localPosition.x + posOveredAddition.x,
                                                                         childGO.GetComponent<RectTransform>().localPosition.y + posOveredAddition.y);
            
            // Set Size
            childGO.transform.localScale = startSize * enlargmentFactor;

            // Set Texts
            abMenu.textName.text = textName;
            abMenu.textDescription.text = textDescription;
        }
        else
        {
            // Reset all
            childGO.GetComponent<RectTransform>().localPosition = startPos;

            childGO.transform.localScale = startSize;

            abMenu.textName.text = "";
            abMenu.textDescription.text = "";
        }
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (childGO != transform.GetChild(0))
        {
            childGO = transform.GetChild(0).gameObject;
            startSize = childGO.transform.localScale;
        }

        TriggerCardOvered(true);

    }

    public void OnPointerExit(PointerEventData eventData)
    {
        TriggerCardOvered(false);
    }

    public void OnPointerDown(PointerEventData eventData)
    {
        isPointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (!isDragging && isPointerDown)
        {
            // Card Hit
            PubSub.Instance.Notify(EMessageType.CardSelected, GetComponent<AbilityMenuSlot>());
            GameManager.Instance.uiManager.TriggerAbilityMenu(false);
        }

        isPointerDown = false;
        isDragging = false;
    }

    #endregion

    #region Drag & Drop

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (childGO != transform.GetChild(0))
        {
            childGO = transform.GetChild(0).gameObject;
        }

        parentAfterDrag = childGO.transform.parent.GetComponent<RectTransform>(); // = this.transform

        childGO.transform.SetParent(transform.root);
        childGO.transform.SetAsLastSibling();

        isDragging = true;
    }

    public void OnDrag(PointerEventData eventData)
    {
        childGO.GetComponent<RectTransform>().position = Input.mousePosition;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        AbilityMenuSlot slotHit = UIRaycastToSlot(eventData);

        if (slotHit != null)
        {
            // Dropped on a card

            GameObject cardImageHit = slotHit.transform.GetChild(0).gameObject;

            cardImageHit.transform.SetParent(parentAfterDrag.transform); // Set parent of new card to original parent
            cardImageHit.GetComponent<RectTransform>().position = parentAfterDrag.position; // Reset pos

            childGO.transform.SetParent(slotHit.transform); // Set parent of dragged card to slot hit
            childGO.GetComponent<RectTransform>().position = slotHit.GetComponent<RectTransform>().position; // Reset pos

            GameManager.Instance.uiManager.UpdateWheel();

            // Set size
            childGO.transform.localScale = startSize;

        }
        else
        {
            // Dropped on invalid pos

            childGO.transform.SetParent(parentAfterDrag.transform); // Back to original parent
            childGO.GetComponent<RectTransform>().position = parentAfterDrag.position; // Reset pos
        }

        isDragging = false;

    }

    #endregion

    private static AbilityMenuSlot UIRaycastToSlot(PointerEventData eventData)
    {
        eventData.position = Input.mousePosition;
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventData, results);

        AbilityMenuSlot slotHit = null;

        foreach (RaycastResult result in results)
        {
            if (result.gameObject.GetComponent<AbilityMenuSlot>() != null)
            {
                slotHit = result.gameObject.GetComponent<AbilityMenuSlot>();
                break;
            }
        }

        return slotHit;
    }

}
