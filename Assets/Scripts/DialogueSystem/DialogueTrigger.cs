using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour
{
    bool done=false;

    private void OnEnable()
    {
        GetComponentInChildren<Dialogue>(true).gameObject.SetActive(false);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (!done)
        {
            GetComponentInChildren<Dialogue>(true).gameObject.SetActive(true);
            done = true;
        }
    }
}
