using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TalkingNPC : MonoBehaviour
{
    Dialogue dialogue;

    // Start is called before the first frame update
    void Start()
    {
        dialogue = GetComponentInChildren<Dialogue>();
    }

    public void DialogueEnd()
    {
        if (dialogue.repeatable)
            return;

        GetComponent<SaveObjState>().ChangeObjectState(false);
        gameObject.SetActive(false);
    }
}
