using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugTeleporter : MonoBehaviour
{
    [SerializeField] private GameObject objToTeleport;
    [SerializeField] private bool tpAtStart = true;
    [SerializeField] private bool tpNow = false;
    [SerializeField] private bool DestroyAfterTp = false;

    private void Start()
    {
        if (objToTeleport == null) return;

        if (tpAtStart)
        {
            objToTeleport.transform.position = transform.position;

            if (DestroyAfterTp)
                Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (objToTeleport == null) return;

        if (tpNow)
        {
            objToTeleport.transform.position = transform.position;
            tpNow = false;

            if (DestroyAfterTp)
                Destroy(gameObject);
        }
    }
}
