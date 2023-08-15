using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityMenu : MonoBehaviour
{
    [Header("PANELS")]
    [SerializeField] private RectTransform showCardsPanel;
    [SerializeField] private RectTransform infoPanel;
    [SerializeField] private RectTransform passiveCardsPanel;

    [Header("SLOTS")]
    [SerializeField] private float enlargmentFactor = 1.1f;
    [SerializeField] private Vector2 cardPosOveredAddition;
    [SerializeField] private Vector2 slotSize;
    [SerializeField] private Vector2 outlineSize;
    [SerializeField] private Color outlineColor;

    [Header("PASSIVE SLOTS")]
    [SerializeField] private float enlargmentFactorPassive = 1.1f;
    [SerializeField] private Vector2 posOveredAdditionPassive;
    [SerializeField] private Vector2 slotSizePassive;

    [Header("GENERAL DATA")]
    [SerializeField] private GameObject panelSlotPrefab;
    [Tooltip("The value for the Game-Time while menu open. 1 = Default")]
    [SerializeField, Range(0.1f, 2f)] private float timeDividerOnMenuOpen = 1f;
    [Tooltip("Time needed for the card to be set to new size and pos if overed")]
    [SerializeField] private float animDuration;

    [HideInInspector] public TextMeshProUGUI textName;
    [HideInInspector] public TextMeshProUGUI textDescription;
    [HideInInspector] public TextMeshProUGUI textTutorial;

    private List<AbilityMenuSlot> loadedSlots;
    private List<AbilityMenuSlot> passiveLoadedSlots;
    private float originalTimeScale;

    private void Start()
    {
        textName = infoPanel.GetChild(0).GetComponent<TextMeshProUGUI>();
        textDescription = infoPanel.GetChild(1).GetComponent<TextMeshProUGUI>();
        textTutorial = infoPanel.GetChild(2).GetComponent<TextMeshProUGUI>();
    }

    private void LoadCards()
    {
        List<WheelSlot> activeWheelSlots = new List<WheelSlot>();
        List<Ability> activeAbilities = new List<Ability>();

        if (loadedSlots == null)
            loadedSlots = new List<AbilityMenuSlot>();
        if (passiveLoadedSlots == null)
            passiveLoadedSlots = new List<AbilityMenuSlot>();

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

        List<Ability> abilitiesUnlocked = GameManager.Instance.abilityManager.GetUnlockedAbilities();

        for (int i = 0; i < abilitiesUnlocked.Count; i++)
        {
            if (abilitiesUnlocked[i].passive)
            {
                GameObject newPassiveCard = Instantiate(panelSlotPrefab, passiveCardsPanel, false);
                newPassiveCard.name = "PassivePanelSlot_" + i;

                SetupPassiveCardLoaded(newPassiveCard, abilitiesUnlocked[i]);

                SetupPassiveSlotReferences(newPassiveCard.GetComponent<AbilityMenuSlot>(), abilitiesUnlocked[i]);

                passiveLoadedSlots.Add(newPassiveCard.GetComponent<AbilityMenuSlot>());
            }    
        }
    }

    #region Setup Cards

    private void SetupPassiveCardLoaded(GameObject newCard, Ability ability)
    {
        // Setup slot size
        newCard.GetComponent<RectTransform>().sizeDelta = slotSizePassive;

        // Setup child Image for graphics
        GameObject cardImage = new GameObject("CardImage");
        cardImage.transform.SetParent(newCard.transform);
        cardImage.transform.localPosition = newCard.transform.localPosition;

        // Setup size = parent size
        cardImage.AddComponent<RectTransform>();
        cardImage.GetComponent<RectTransform>().sizeDelta = slotSizePassive;

        // Setup image icon
        cardImage.AddComponent<Image>();
        cardImage.GetComponent<Image>().sprite = ability.icon;
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
        abilityMenuSlot.textTutorial = ability.tutorial;
        abilityMenuSlot.animDuration = animDuration;
        abilityMenuSlot.passive = false;
    }

    private void SetupPassiveSlotReferences(AbilityMenuSlot abilityMenuSlot, Ability ability)
    {
        abilityMenuSlot.abMenu = this;
        abilityMenuSlot.enlargmentFactorPassive = enlargmentFactorPassive;
        abilityMenuSlot.posOveredAdditionPassive = posOveredAdditionPassive;
        abilityMenuSlot.animDuration = animDuration;
        abilityMenuSlot.passive = true;
    }

    #endregion

    #region Menu Management
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
        passiveLoadedSlots.Clear();

        for (int i = 0; i < showCardsPanel.childCount; i++)
        {
            Destroy(showCardsPanel.GetChild(i).gameObject);
        }

        for (int i = 0; i < passiveCardsPanel.childCount; i++)
        {
            Destroy(passiveCardsPanel.GetChild(i).gameObject);
        }

        Time.timeScale = originalTimeScale;
    }

    #endregion

    #region Others
    public List<AbilityMenuSlot> GetLoadedSlots()
    {
        return loadedSlots;
    }

    #endregion
}
