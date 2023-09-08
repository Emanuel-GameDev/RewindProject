using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.Universal;

public class BossDoor : MonoBehaviour
{
    [SerializeField] Ability deathAbility;

    [SerializeField] Sprite doorOpenSprite;
    [SerializeField] Sprite doorClosedSprite;

    bool doorOpen = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!collision.gameObject.GetComponent<PlayerController>())
            return;

        PlayerController player = collision.gameObject.GetComponent<PlayerController>();

        if (!doorOpen)
        {
            player.buttonReminder.SetActive(false);
        }
       
         
    }

    private void OpenDoor()
    {
        GetComponent<PlayerTriggerCause>().enabled = true;
        GetComponentInChildren<SpriteRenderer>().sprite = doorOpenSprite;
        GetComponentInChildren<Light2D>().gameObject.SetActive(true);
        doorOpen = true;
    }

    private void CloseDoor()
    {
        GetComponent<PlayerTriggerCause>().enabled = false;

        GetComponentInChildren<SpriteRenderer>().sprite = doorClosedSprite;
        GetComponentInChildren<Light2D>().gameObject.SetActive(false);
        doorOpen = false;
    }

    private void Start()
    {
        if (GameManager.Instance.abilityManager._abilities.Find(a => a == deathAbility))
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }

    }
}
