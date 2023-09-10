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
        if (numPieces >= 2 && GameManager.Instance.abilityManager.abilityNameToSave.Find(a => a == deathAbility.name) == null)
        {
            GetComponent<PlayerTriggerCause>().enabled = true;
        }
        else
            GetComponent<PlayerTriggerCause>().enabled = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        DataSerializer.TryLoad("DeathAbilityPieces", out int numPieces);
        if (numPieces >= 2 && GameManager.Instance.abilityManager.abilityNameToSave.Find(a => a == deathAbility.name) == null)
        {
             GetComponent<PlayerTriggerCause>().enabled = true;
        }else
            GetComponent<PlayerTriggerCause>().enabled = false;
    }

    public void GiveAbility()
    {
        if (GameManager.Instance.abilityManager.abilityNameToSave.Find(a => a == deathAbility.name)!=null)
            return;

        List<object> pickData = new List<object> { PlayerController.instance, deathAbility };

        PubSub.Instance.Notify(EMessageType.AbilityAnimStart, pickData);
    }
}
