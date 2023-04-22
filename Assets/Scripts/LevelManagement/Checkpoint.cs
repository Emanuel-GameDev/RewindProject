using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Checkpoint : MonoBehaviour
{
    public bool taken { get; private set; } = false;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        taken = true;
    }
}
