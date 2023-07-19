using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UI.Extensions;
using UnityEngine.InputSystem;

public class AbilityWheel : MonoBehaviour
{
    //  ====== GENERAL ======
    [SerializeField] RectTransform rotPoint;

    private PlayerInputs inputs;

    private Image selectionImage;
    private List<RectTransform> images = new List<RectTransform>();
    private int cardId = 0;
    private int numCards = 0;

    //  ====== SPECIFIC ======
    [Header("ROTATION")]
    [SerializeField] private float rotationAmount;
    [SerializeField] private float rotationSpeed;

    private bool isRotating = false;

    #region Inputs
    private void OnEnable()
    {
        inputs = new PlayerInputs();
        inputs.UI.Enable();

        inputs.UI.ScrollWheel.performed += ScrollInput;
    }

    private void OnDisable()
    {
        inputs.UI.Disable();

        inputs.UI.ScrollWheel.performed -= ScrollInput;
    }

    #endregion

    #region General
    private void Awake()
    {
        images = new List<RectTransform>();
        SetUpLists();
    }

    private void Start()
    {
    }

    public void SetUpLists()
    {
        for (int i = 0; i < rotPoint.childCount; i++)
        {
            images.Add(rotPoint.GetChild(i).GetChild(0).GetComponent<RectTransform>());
        }
    }

    public void AddToWheel(Ability ability)
    {
        if (ability == null) return;
        SetupCard(ability);
        numCards++;

        UpdateSelectedAbility();
    }

    private void SetupCard(Ability ability)
    {
        switch (numCards)
        {
            // caso in cui ho preso la prima carta
            case 0:
                images[3].GetComponent<Image>().sprite = ability.icon;
                images[3].gameObject.SetActive(true);

                images[1].GetComponent<Image>().sprite = ability.icon;
                images[1].gameObject.SetActive(true);
                images[5].GetComponent<Image>().sprite = ability.icon;
                images[5].gameObject.SetActive(true);

                break;

            // caso in cui ho preso la seconda carta
            case 1:
                images[4].GetComponent<Image>().sprite = ability.icon;
                images[4].gameObject.SetActive(true);

                images[2].GetComponent<Image>().sprite = ability.icon;
                images[2].gameObject.SetActive(true);

                images[0].GetComponent<Image>().sprite = ability.icon;
                images[0].gameObject.SetActive(true);

                images[6].GetComponent<Image>().sprite = ability.icon;
                images[6].gameObject.SetActive(true);

                break;

            case 2:
                images[0].GetComponent<Image>().sprite = ability.icon;
                images[4].GetComponent<Image>().sprite = ability.icon;

                break;
        }
    }

    #endregion

    #region Scroll
    private void ScrollInput(InputAction.CallbackContext scroll)
    {
        float scrollValue = scroll.ReadValue<Vector2>().y;
        float currentRotation = rotPoint.localEulerAngles.z;

        if (scrollValue > 0f && CanRotate())
        {
            Quaternion newRotation = Quaternion.Euler(0f, 0f, currentRotation + rotationAmount);

            StartCoroutine(Rotate(newRotation, clockwise: true));

        }
        else if (scrollValue < 0f && CanRotate())
        {
            Quaternion newRotation = Quaternion.Euler(0f, 0f, currentRotation - rotationAmount);

            StartCoroutine(Rotate(newRotation, clockwise: false));
        }
    }

    private bool CanRotate()
    {
        bool canRotate = false;

        if (numCards > 1 && !isRotating)
            canRotate = true;

        return canRotate;
    }

    private IEnumerator Rotate(Quaternion newRotation, bool clockwise)
    {
        isRotating = true;

        // Loop finché l'angolo tra le due rotazioni non è vicino a 0
        while (Quaternion.Angle(rotPoint.localRotation, newRotation) > 0.01f)
        {
            // Applico rotazione z
            rotPoint.localRotation = Quaternion.Lerp(rotPoint.localRotation, newRotation, rotationSpeed * Time.deltaTime);

            yield return null;
        }

        isRotating = false;
        rotPoint.localRotation = newRotation;

        // Controllo in base al tipo di rotazione se aumentare o diminuire l'indice della carta
        cardId = clockwise ? cardId + 1 : cardId - 1;

        UpdateCards(clockwise);
    }

    private void UpdateCards(bool clockwise)
    {
        if (clockwise)
        {
            int index = (images.Count - 1 - (cardId % images.Count)) % images.Count;

            Image newImage = images[(index + 1) % images.Count].GetComponent<Image>();
            Image shiftedImage = images[index].GetComponent<Image>();

            newImage.sprite = shiftedImage.sprite;
            newImage.gameObject.SetActive(true);
            shiftedImage.gameObject.SetActive(false);

            UpdateSelectedAbility();
            
        }
        else
        {
            int index = (images.Count - 2 - (cardId % images.Count)) % images.Count;

            Image newImage = images[index % images.Count].GetComponent<Image>();
            Image shiftedImage = images[(index + 1) % images.Count].GetComponent<Image>();

            newImage.sprite = shiftedImage.sprite;
            newImage.gameObject.SetActive(true);
            shiftedImage.gameObject.SetActive(false);

            UpdateSelectedAbility();
        }
    }

    private void UpdateSelectedAbility()
    {
        int middleIndex = (images.Count - 3) / 2;
        int indexOffset = (cardId - 1) % images.Count;

        int index = (middleIndex - indexOffset + images.Count) % images.Count;

        Image newSelectedImage = images[index].GetComponent<Image>();
        if (selectionImage != newSelectedImage)
        {
            PubSub.Instance.Notify(EMessageType.ActiveAbilityChanged, newSelectedImage);
            selectionImage = newSelectedImage;
        }
    }

    #endregion

}
