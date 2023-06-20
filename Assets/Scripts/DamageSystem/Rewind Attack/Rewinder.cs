using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Rewinder : MonoBehaviour
{
     [SerializeField] UnityEvent onHit;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.isTrigger)
            return;

        if (collision.GetComponent<Rewindable>())
        {
            collision.GetComponent<Rewindable>().StartRewind();
            onHit?.Invoke();

        }
    }
}
