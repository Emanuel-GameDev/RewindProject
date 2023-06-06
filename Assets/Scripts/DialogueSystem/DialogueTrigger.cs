using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour /*, IDataPersistance*/
{
    bool dialogueTriggered = false;

    private void OnEnable()
    {
        GetComponentInChildren<Dialogue>(true).gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!dialogueTriggered)
        {
            GetComponentInChildren<Dialogue>(true).gameObject.SetActive(true);
            dialogueTriggered = true;
        }
    }

    //void IDataPersistance.LoadData(GameData data)
    //{
    //    this.dialogueTriggered = data.dialogue;
    //}

    //void IDataPersistance.SaveData(ref GameData data)
    //{
    //    data.dialogue = this.dialogueTriggered;
    //}
}
