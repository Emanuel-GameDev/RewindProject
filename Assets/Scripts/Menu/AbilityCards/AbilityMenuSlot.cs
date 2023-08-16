using System.Collections;
using System.Collections.Generic;
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

    // Vars for passive slot
    [HideInInspector] public bool passive;
    [HideInInspector] public float enlargmentFactorPassive = 1.1f;
    [HideInInspector] public Vector2 posOveredAdditionPassive;

    private GameObject childGO;
    private Vector2 startSize;
    private Vector2 startPos;
    private RectTransform parentAfterDrag;
    private bool isDragging = false;
    private bool isPointerDown = false;
    private bool canBeOvered = true;
    private bool isPointerInside = false;
    private bool isPointerOutOfScreen = false;
    private IEnumerator animationCoroutine;

    #region UnityFunctions

    #endregion

    #region PointerRelated

    #region Overing
    public void OnPointerEnter(PointerEventData eventData)
    {
        if (isDragging || !canBeOvered) return;

        isPointerInside = true;

        childGO = transform.GetChild(0).gameObject;
        startSize = childGO.transform.localScale;
        startPos = childGO.transform.localPosition;

        TriggerCardOvered(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        if (!isPointerInside) return;

        TriggerCardOvered(false);

        isPointerInside = false;
    }

    private IEnumerator Animation(bool mode)
    {
        if (mode)
        {
            canBeOvered = false;

            // Setup target pos
            Vector2 targetPos;
            float actualEnlargmentFactor;

            if (!passive)
            {
                targetPos = new Vector2(startPos.x + posOveredAddition.x, startPos.y + posOveredAddition.y);
                actualEnlargmentFactor = enlargmentFactor;
            }
            else
            {
                targetPos = new Vector2(startPos.x + posOveredAdditionPassive.x, startPos.y + posOveredAdditionPassive.y);
                actualEnlargmentFactor = enlargmentFactorPassive;
            }

            float elapsedTime = 0f;
            while (elapsedTime < animDuration)
            {
                elapsedTime += Time.deltaTime;
                float t = elapsedTime / animDuration;

                childGO.GetComponent<RectTransform>().localPosition = Vector2.Lerp(startPos, targetPos, t);
                childGO.GetComponent<RectTransform>().localScale = Vector2.Lerp(startSize, (startSize * actualEnlargmentFactor), t);

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

            canBeOvered = true;
        }

    }

    private void TriggerCardOvered(bool mode)
    {
        if (mode)
        {

            // Set Texts
            abMenu.textName.text = textName;
            abMenu.textDescription.text = textDescription;
            abMenu.textTutorial.text = textTutorial;

            // Set Outline
            if (!passive)
            {
                childGO.GetComponent<Outline>().enabled = true;
            }
        }
        else
        {

            abMenu.textName.text = "";
            abMenu.textDescription.text = "";
            abMenu.textTutorial.text = "";

            if (!passive)
                childGO.GetComponent<Outline>().enabled = false;
        }

        if (animationCoroutine != null)
            StopCoroutine(animationCoroutine);

        animationCoroutine = Animation(mode);
        StartCoroutine(animationCoroutine);
    }

    #endregion

    #region Clicking
    public void OnPointerDown(PointerEventData eventData)
    {
        if (passive || isPointerOutOfScreen) return;

        isPointerDown = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        if (passive || isPointerOutOfScreen) return;

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

    #endregion

    #region Drag & Drop

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (passive) return;

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
        if (passive) return;

        Vector3 mousePos = Input.mousePosition;

        isPointerOutOfScreen = mousePos.x < 0 || mousePos.x > Screen.width ||
                            mousePos.y < 0 || mousePos.y > Screen.height;

        if (isPointerOutOfScreen)
        {
            childGO.transform.SetParent(parentAfterDrag);
            return;
        }
        else
            childGO.GetComponent<RectTransform>().position = mousePos;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (passive) return;

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

    #region Others
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

    #endregion

}
