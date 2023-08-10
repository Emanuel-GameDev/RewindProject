using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class AbilityWheel : MonoBehaviour
{
    [Header("SWITCH CARD DATA")]
    [SerializeField] private float animDuration;
    [Tooltip("Time needed for the next card switch, minimum value = animDuration")]
    [SerializeField, Min(0f)] private float switchOffset;
    [Tooltip("These values will be added to the image position in order to obtain the animation start point")]
    [SerializeField] private Vector2 animPosAddition = new Vector2(-15f, +20f);

    private PlayerInputs inputs;

    private List<WheelSlot> slots = new List<WheelSlot>();
    private int centralSlotIndex;

    [HideInInspector] public bool canSwitch = true;

    // DEBUG

    [Header("DEBUG")]
    [SerializeField] private Character character;
    [SerializeField] private List<Ability> debugAbilities = new List<Ability>();

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

    #region UnityFunctions

    private void OnValidate()
    {
        switchOffset = Mathf.Max(switchOffset, animDuration);
    }

    private void Start()
    {
        Show();

        // Filling slots
        for (int i = 0; i < transform.childCount; i++)
        {
            WheelSlot slot = transform.GetChild(i).GetComponent<WheelSlot>();

            if (slot != null)
                slots.Add(slot);
        }

        // Get position of central slot
        for (int i = 0; i < slots.Count; i++)
        {
            if (slots[i].central)
                centralSlotIndex = i;
        }

        slots[centralSlotIndex].gameObject.transform.SetAsLastSibling();

        DebugSetup();

    }

    #endregion

    #region Scroll

    private void ScrollInput(InputAction.CallbackContext scroll)
    {
        if (!CheckCondition()) return;

        // Shifting only in between active slots
        List<WheelSlot> activeSlots = new List<WheelSlot>();

        foreach (WheelSlot slot in slots)
        {
            if (slot.GetAttachedAbility() != null)
                activeSlots.Add(slot);
        }

        // Check scroll direction
        float scrollValue = scroll.ReadValue<Vector2>().y;

        if (scrollValue > 0f)
        {
            switch (ShiftPossible(activeSlots))
            {
                case 1:
                    break;
                case 2:
                    SingleSwitch(activeSlots);

                    UpdateSlotsGraphic(activeSlots);
                    UpdateActiveAbility();

                    StartCoroutine(Animation());
                    break;
                case > 2:
                    ShiftAbilitiesRight(activeSlots);

                    UpdateSlotsGraphic(activeSlots);
                    UpdateActiveAbility();

                    StartCoroutine(Animation());
                    break;
            }
        }
        else if (scrollValue < 0f)
        {
            switch (ShiftPossible(activeSlots))
            {
                case 1:
                    break;
                case 2:
                    SingleSwitch(activeSlots);

                    UpdateSlotsGraphic(activeSlots);
                    UpdateActiveAbility();

                    StartCoroutine(Animation());
                    break;
                case > 2:
                    ShiftAbilitiesLeft(activeSlots);

                    UpdateSlotsGraphic(activeSlots);
                    UpdateActiveAbility();

                    StartCoroutine(Animation());
                    break;
            }
        }
    }


    #region Animation
    private IEnumerator Animation()
    {
        float elapsedTime = 0f;
        canSwitch = false;

        Image centralImage = slots[centralSlotIndex].transform.GetChild(0).GetComponent<Image>();
        Vector2 prevPos = centralImage.transform.localPosition;
        Vector2 startPos = new Vector2(centralImage.transform.localPosition.x + animPosAddition.x,
                                       centralImage.transform.localPosition.y + animPosAddition.y);

        Color startColor = new Color(centralImage.color.r, centralImage.color.g, centralImage.color.b, 0f);
        Color endColor = new Color(centralImage.color.r, centralImage.color.g, centralImage.color.b, 1f);

        centralImage.gameObject.transform.position = startPos;

        while (elapsedTime < animDuration)
        {
            elapsedTime += Time.deltaTime;
            float t = elapsedTime / animDuration;

            centralImage.gameObject.transform.localPosition = Vector2.Lerp(startPos, prevPos, t);
            centralImage.color = Color.Lerp(startColor, endColor, t);

            yield return null;

        }

        StartCoroutine(Cooldown());
    }

    private IEnumerator Cooldown()
    {
        yield return new WaitForSeconds(switchOffset);

        canSwitch = true;
    }

    #endregion

    #region Shift & Switch
    // Shift is possible only if at least 3 abilities were collected
    private int ShiftPossible(List<WheelSlot> shiftables)
    {
        int abilitiesAttached = 0;

        foreach (WheelSlot slot in shiftables)
        {
            if (slot.GetAttachedAbility() != null)
                abilitiesAttached++;
        }

        return abilitiesAttached;
    }

    private void ShiftAbilitiesRight(List<WheelSlot> shiftables)
    {
        Ability lastAbility = shiftables[shiftables.Count - 1].GetAttachedAbility();

        for (int i = shiftables.Count - 1; i > 0; i--)
        {
            Ability prevAbility = shiftables[i - 1].GetAttachedAbility();
            shiftables[i].AttachAbility(prevAbility);
        }

        shiftables[0].AttachAbility(lastAbility);

    }

    private void ShiftAbilitiesLeft(List<WheelSlot> shiftables)
    {
        Ability firstAbility = shiftables[0].GetAttachedAbility();

        for (int i = 0; i < shiftables.Count - 1; i++)
        {
            Ability nextAbility = shiftables[i + 1].GetAttachedAbility();
            shiftables[i].AttachAbility(nextAbility);
        }

        shiftables[shiftables.Count - 1].AttachAbility(firstAbility);
    }
    private void SingleSwitch(List<WheelSlot> activeSlots)
    {
        WheelSlot slot1 = activeSlots[centralSlotIndex];
        WheelSlot slot2 = activeSlots[(centralSlotIndex - 1) % activeSlots.Count];

        Ability ability1 = slot1.GetAttachedAbility();
        Ability ability2 = slot2.GetAttachedAbility();

        // Switch delle abilità tra i due slot
        slot1.AttachAbility(ability2);
        slot2.AttachAbility(ability1);
    }

    #endregion


    #endregion

    #region Wheel Management
    public void AddToWheel(Ability ability)
    {
        if (ability == null) return;

        // On first ability picked the middle socket will be shown
        if (slots[centralSlotIndex].GetAttachedAbility() == null)
        {
            slots[centralSlotIndex].AttachAbility(ability);

            slots[centralSlotIndex].transform.GetChild(0).GetComponent<Image>().sprite = ability.icon;
            slots[centralSlotIndex].SetSlotText(ability.name);
            slots[centralSlotIndex].transform.GetChild(0).gameObject.SetActive(true);

            UpdateActiveAbility();
            return;
        }

        for (int i = 0; i < slots.Count - 1; i++)
        {
            WheelSlot slot = slots[i];

            if (slot.GetAttachedAbility() == null)
            {
                slot.AttachAbility(ability);

                slot.transform.GetChild(0).GetComponent<Image>().sprite = ability.icon;
                slot.SetSlotText(ability.name);
                slot.transform.GetChild(0).gameObject.SetActive(true);

                break;
            }

        }

        UpdateActiveAbility();

    }

    private void UpdateSlotsGraphic(List<WheelSlot> shiftables)
    {
        foreach (WheelSlot slot in shiftables)
        {
            Image image = slot.transform.GetChild(0).GetComponent<Image>();

            image.sprite = slot.GetAttachedAbility().icon;

            if (slot.GetSlotText() != null)
                slot.SetSlotText(slot.GetAttachedAbility().name);
        }
    }

    private void UpdateActiveAbility()
    {
        PubSub.Instance.Notify(EMessageType.ActiveAbilityChanged, slots[centralSlotIndex].GetAttachedAbility().icon);
    }

    public void Show()
    {
        gameObject.SetActive(true);
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

    #endregion

    #region Others
    private bool CheckCondition()
    {
        if (slots[centralSlotIndex].GetAttachedAbility() != null)
        {
            if (slots[centralSlotIndex].GetAttachedAbility().isActive) return false;
        }

        if (!canSwitch) return false;

        return true;
    }

    private void DebugSetup()
    {
        if (debugAbilities.Count <= 0 ||
            !GameManager.Instance.debug) return;

        foreach (Ability ability in debugAbilities)
        {
            ability.Pick(character);
        }
    }

    #endregion

}
