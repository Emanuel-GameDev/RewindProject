using System;
using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [SerializeField] Image cardImage;
    [SerializeField] TextMeshProUGUI cardName;
    [SerializeField] TextMeshProUGUI cardDescription;

    private Character character;
    private Animator animator;
    private Ability cardToShow;

    private void Start()
    {
        animator = GetComponent<Animator>();
        PubSub.Instance.RegisterFunction(EMessageType.AbilityAnimStart, StartShowAnimation);
    }

    private void StartShowAnimation(object obj)
    {
        if (obj is List<object>)
        {
            List<object> list = (List<object>)obj;

            character = (Character)list[0];
            cardToShow = (Ability)list[1];
        }

        character.GetComponent<PlayerController>().inputs.Player.Disable();

        if (cardToShow == null) return;

        TriggerCard(true);

        animator.SetTrigger("hasToShowCard");
    }

    private void TriggerCard(bool v)
    {
        cardImage.sprite = cardToShow.icon;
        cardImage.gameObject.SetActive(v);

        cardName.text = cardToShow.name;
        cardName.gameObject.SetActive(v);

        cardDescription.text = cardToShow.description;
        cardDescription.gameObject.SetActive(v);
    }

    public void ShowCompleted()
    {
        if (cardToShow == null)
        {
            Debug.LogError("CardToShow = null");
            return;
        }

        character.GetComponent<PlayerController>().inputs.Player.Enable();

        TriggerCard(false);

        cardToShow.Pick(character);
        cardToShow = null;
        character = null;
    }
}
