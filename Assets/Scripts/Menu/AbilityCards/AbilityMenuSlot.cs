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
    [HideInInspector] public string textTutorial;
    [HideInInspector] public AbilityMenu abMenu;
    [HideInInspector] public float animDuration;

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
            // Set Texts
            abMenu.textName.text = textName;
            abMenu.textDescription.text = textDescription;
            abMenu.textTutorial.text = textTutorial;

            // Set Outline
            childGO.GetComponent<Outline>().enabled = true;
        }
        else
        {
            abMenu.textName.text = "";
            abMenu.textDescription.text = "";
            abMenu.textTutorial.text = "";

            childGO.GetComponent<Outline>().enabled = false;
        }

        StartCoroutine(Animation(mode));
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isDragging) return;

        if (childGO != transform.GetChild(0))
        {
            childGO = transform.GetChild(0).gameObject;
            startSize = childGO.transform.localScale;
        }

        TriggerCardOvered(true);
    }

    private IEnumerator Animation(bool mode)
    {
        if (mode)
        {
            // Setup target pos
            startPos = childGO.GetComponent<RectTransform>().localPosition;
            Vector3 targetPos = new Vector2(startPos.x + posOveredAddition.x, startPos.y + posOveredAddition.y);

            Vector3 startSize = childGO.GetComponent<RectTransform>().localScale;

            float elapsedTime = 0f;
            while (elapsedTime < animDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / animDuration;

                childGO.GetComponent<RectTransform>().localPosition = Vector2.Lerp(startPos, targetPos, t);
                childGO.GetComponent<RectTransform>().localScale = Vector2.Lerp(startSize, (startSize * enlargmentFactor), t);

                yield return null;
            }

        }
        else
        {
            Vector3 currPos = childGO.GetComponent<RectTransform>().localPosition;
            Vector3 currSize = childGO.GetComponent<RectTransform>().localScale;

            float elapsedTime = 0f;
            while (elapsedTime < animDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / animDuration;

                childGO.GetComponent<RectTransform>().localPosition = Vector2.Lerp(currPos, startPos, t);
                childGO.GetComponent<RectTransform>().localScale = Vector2.Lerp(currSize, startSize, t);

                yield return null;
            }
        }
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
        Vector3 mousePos = Input.mousePosition;
        childGO.GetComponent<RectTransform>().position = mousePos;

        //bool isPointerOutOfScreen = mousePos.x < 0 || mousePos.x > Screen.width ||
        //                    mousePos.y < 0 || mousePos.y > Screen.height;

        //if (isPointerOutOfScreen)
        //    return;
        //else
        //    childGO.GetComponent<RectTransform>().position = mousePos;
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
