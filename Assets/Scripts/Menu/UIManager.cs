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

        cardImage.gameObject.SetActive(false);
        cardName.gameObject.SetActive(false);
        cardDescription.gameObject.SetActive(false);
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

    public void ShowCompleted()
    {
        character.GetComponent<PlayerController>().inputs.Player.Enable();

        cardToShow.Pick(character);
        cardToShow = null;
        character = null;
    }
}
