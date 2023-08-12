using System.Collections;
using System.Collections.Generic;
using System.Drawing;
using ToolBox.Serialization;
using UnityEngine;
using UnityEngine.SceneManagement;

public class DialogueTrigger : MonoBehaviour
{
    public bool dialogueTriggered = false;

    private void OnEnable()
    {
        DataSerializer.TryLoad(SceneManager.GetActiveScene().name + name, out dialogueTriggered);
        GetComponentInChildren<Dialogue>(true).gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.layer != LayerMask.NameToLayer("Player"))
            return;

        if (!dialogueTriggered || GetComponentInChildren<Dialogue>(true).repeatable)
        {
            dialogueTriggered = true;
            DataSerializer.Save(SceneManager.GetActiveScene().name + name, dialogueTriggered);
            GetComponentInChildren<Dialogue>(true).gameObject.SetActive(true);
        }
    }
}
