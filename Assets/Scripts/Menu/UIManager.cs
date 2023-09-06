using System;
using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class UIManager : MonoBehaviour
{
    [Header("SHOW CARD PICKED DATA")]
    [SerializeField] Image cardImage;
    [SerializeField] TextMeshProUGUI cardName;
    [SerializeField] TextMeshProUGUI cardDescription;

    private Character character;
    private Animator animator;
    private Ability cardToShow;
    private AbilityMenu abilityMenu;
    private AbilityWheel abilityWheel;
    private PlayerInputs inputs;

    private bool canShowMenu;

    private void OnEnable()
    {
        if (!PlayerController.instance.inputs.UI.enabled)
            PlayerController.instance.inputs.UI.Enable();

        PlayerController.instance.inputs.UI.ScrollWheelClick.performed += OpenAbilityMenu;
    }

    private void OnDisable()
    {
        PlayerController.instance.inputs.UI.ScrollWheelClick.performed -= OpenAbilityMenu;
    }

    private void OpenAbilityMenu(InputAction.CallbackContext obj)
    {
        if (abilityMenu == null || abilityWheel == null)
        {
            Debug.LogError("Error getting reference in either ability wheel or ability menu");
            return;
        }

        if (!abilityWheel.canSwitch) return;

        if (GameManager.Instance.debug)
            canShowMenu = true;

        if (!abilityMenu.gameObject.activeSelf && abilityMenu.CanBeOpened() && canShowMenu)
        {
            TriggerAbilityMenu(true);

        }
        else if (abilityMenu.gameObject.activeSelf)
        {
            TriggerAbilityMenu(false);
        }
    }

    public void TriggerAbilityMenu(bool mode)
    {
        if (mode)
        {
            abilityWheel.Hide();
            abilityMenu.Open();

            PlayerController.instance.inputs.Player.Disable();
        }
        else
        {
            abilityWheel.Show();
            abilityMenu.Close();

            PlayerController.instance.inputs.Player.Enable();
        }
    }

    private void Start()
    {
        animator = GetComponent<Animator>();
        PubSub.Instance.RegisterFunction(EMessageType.AbilityAnimStart, StartShowAnimation);

        cardImage.gameObject.SetActive(false);
        cardName.gameObject.SetActive(false);
        cardDescription.gameObject.SetActive(false);

        SetupReferences();

        abilityMenu.gameObject.SetActive(false);

    }

    private void SetupReferences()
    {
        for (int i = 0; i < gameObject.transform.childCount; i++)
        {
            if (gameObject.transform.GetChild(i).GetComponent<AbilityMenu>())
                abilityMenu = gameObject.transform.GetChild(i).GetComponent<AbilityMenu>();
            else if (gameObject.transform.GetChild(i).GetComponent<AbilityWheel>())
                abilityWheel = gameObject.transform.GetChild(i).GetComponent<AbilityWheel>();
        }
    }

    private void StartShowAnimation(object obj)
    {
        if (obj is List<object>)
        {
            List<object> list = (List<object>)obj;

            character = (Character)list[0];
            cardToShow = (Ability)list[1];
        }

        // Disable menu show
        canShowMenu = false;

        character.GetComponent<PlayerController>().inputs.Player.Disable();
        character.GetComponent<PlayerController>().inputs.AbilityController.Disable();
        GameManager.Instance.abilityManager.wheel.canSwitch = false;

        if (cardToShow == null)
        {
            Debug.LogError("cardToShow = null");
            return;
        }

        cardImage.sprite = cardToShow.icon;
        cardName.text = cardToShow.name;
        cardDescription.text = cardToShow.description;

        animator.SetTrigger("hasToShowCard");
    }

    internal void UpdateWheel()
    {
        AbilityWheel wheel = GameManager.Instance.abilityManager.wheel;
        List<WheelSlot> activeWheelSlots = wheel.GetActiveWheelSlots();
        List<AbilityMenuSlot> loadedSlots = abilityMenu.GetLoadedSlots();

        for (int i = 0; i < loadedSlots.Count; i++)
        {
            Image abIcon = loadedSlots[i].transform.GetChild(0).GetComponent<Image>();
            Ability ability = GameManager.Instance.abilityManager.GetAbilityFrom(abIcon.sprite);
            activeWheelSlots[i].AttachAbility(ability);
        }

        wheel.UpdateSlotsGraphic(activeWheelSlots);
    }

    public void ShowCompleted()
    {
        character.GetComponent<PlayerController>().inputs.Player.Enable();
        character.GetComponent<PlayerController>().inputs.AbilityController.Enable();

        cardToShow.Pick(character);

        GameManager.Instance.abilityManager.wheel.canSwitch = true;
        cardToShow = null;
        character = null;

        // Enable menu show
        canShowMenu = true;
    }
}
