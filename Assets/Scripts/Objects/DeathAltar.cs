using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization;
using UnityEngine;

public class DeathAltar : MonoBehaviour
{
    [SerializeField] Ability deathAbility;
    PlayerTriggerCause trigger;

    private void Start()
    {
        GetComponent<PlayerTriggerCause>().enabled = false;

        DataSerializer.TryLoad("DeathAbilityPieces", out int numPieces);
        if (numPieces >= 2)
        {
            GetComponent<PlayerTriggerCause>().enabled = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DataSerializer.TryLoad("DeathAbilityPieces", out int numPieces);
        if (numPieces >= 2)
        {
             GetComponent<PlayerTriggerCause>().enabled = true;
        }
    }

    public void GiveAbility()
    {
        List<object> pickData = new List<object> { PlayerController.instance, deathAbility };

        PubSub.Instance.Notify(EMessageType.AbilityAnimStart, pickData);
    }
}
