using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class AbilityMenu : MonoBehaviour
{
    [Header("PANELS")]
    [SerializeField] private RectTransform showCardsPanel;
    [SerializeField] private RectTransform infoPanel;

    [Header("SLOT")]
    [SerializeField] private GameObject panelSlotPrefab;
    [SerializeField] private float enlargmentFactor = 1.1f;
    [SerializeField] private Vector2 cardPosOveredAddition;
    [SerializeField] private Vector2 slotSize;
    [SerializeField] private Vector2 outlineSize;
    [SerializeField] private Color outlineColor;

    [Header("GENERAL DATA")]
    [Tooltip("The value for the Game-Time while menu open. 1 = Default")]
    [SerializeField, Range(0.1f, 2f)] private float timeDividerOnMenuOpen = 1f;

    [HideInInspector] public TextMeshProUGUI textName;
    [HideInInspector] public TextMeshProUGUI textDescription;

    private List<AbilityMenuSlot> loadedSlots;
    private float originalTimeScale;

    private void Start()
    {
        textName = infoPanel.GetChild(0).GetComponent<TextMeshProUGUI>();
        textDescription = infoPanel.GetChild(1).GetComponent<TextMeshProUGUI>();
    }

    private void LoadCards()
    {
        List<WheelSlot> activeWheelSlots = new List<WheelSlot>();
        List<Ability> activeAbilities = new List<Ability>();

        if (loadedSlots == null)
            loadedSlots = new List<AbilityMenuSlot>();

        activeWheelSlots = GameManager.Instance.abilityManager.wheel.GetActiveWheelSlots();

        // Reference to abilities of the wheel in the wheel's order
        foreach (WheelSlot slot in activeWheelSlots)
        {
            if (slot.GetAttachedAbility() != null)
                activeAbilities.Add(slot.GetAttachedAbility());
        }

        for (int i = 0; i < activeAbilities.Count; i++)
        {
            GameObject newCard = Instantiate(panelSlotPrefab, showCardsPanel, false);
            newCard.name = "PanelSlot_" + i;

            SetupNewCardLoaded(activeAbilities, i, newCard);

            SetupSlotReferences(newCard.GetComponent<AbilityMenuSlot>(), activeAbilities[i]);

            loadedSlots.Add(newCard.GetComponent<AbilityMenuSlot>());
        }
    }

    private void SetupNewCardLoaded(List<Ability> activeAbilities, int i, GameObject newCard)
    {
        // Setup slot size
        newCard.GetComponent<RectTransform>().sizeDelta = slotSize;

        // Setup child Image for graphics
        GameObject cardImage = new GameObject("CardImage");
        cardImage.transform.SetParent(newCard.transform);
        cardImage.transform.localPosition = newCard.transform.localPosition;

        // Setup size = parent size
        cardImage.AddComponent<RectTransform>();
        cardImage.GetComponent<RectTransform>().sizeDelta = slotSize;

        // Setup image icon
        cardImage.AddComponent<Image>();
        cardImage.GetComponent<Image>().sprite = activeAbilities[i].icon;

        // Setup Outline
        cardImage.AddComponent<Outline>();
        cardImage.GetComponent<Outline>().effectDistance = outlineSize;
        cardImage.GetComponent<Outline>().effectColor = outlineColor;
        cardImage.GetComponent<Outline>().enabled = false;
    }

    private void SetupSlotReferences(AbilityMenuSlot abilityMenuSlot, Ability ability)
    {
        abilityMenuSlot.abMenu = this;
        abilityMenuSlot.enlargmentFactor = enlargmentFactor;
        abilityMenuSlot.posOveredAddition = cardPosOveredAddition;
        abilityMenuSlot.textName = ability.name;
        abilityMenuSlot.textDescription = ability.description;
    }

    public bool CanBeOpened()
    {
        if (GameManager.Instance.abilityManager.GetUnlockedAbilities().Count == 0)
            return false;
        else
            return true;
    }

    public void Open()
    {
        if (!gameObject.activeSelf)
            gameObject.SetActive(true);

        originalTimeScale = Time.timeScale;
        Time.timeScale = timeDividerOnMenuOpen;

        LoadCards();
    }

    public void Close()
    {
        if (gameObject.activeSelf)
            gameObject.SetActive(false);

        loadedSlots.Clear();

        for (int i = 0; i < showCardsPanel.childCount; i++)
        {
            Destroy(showCardsPanel.GetChild(i).gameObject);
        }

        Time.timeScale = originalTimeScale;
    }

    public List<AbilityMenuSlot> GetLoadedSlots()
    {
        return loadedSlots;
    }
}
