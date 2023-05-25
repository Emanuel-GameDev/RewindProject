using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [SerializeField] private bool hasToSwitch = true;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // COntrollare che collision sia player
        if (collision.gameObject.tag == "Player")
        {
            PubSub.Instance.Notify(EMessageType.CameraSwitch, hasToSwitch);
        }
    }
}
