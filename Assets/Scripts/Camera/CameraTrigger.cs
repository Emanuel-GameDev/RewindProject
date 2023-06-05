using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [Tooltip("New value for orthografic size in the Lens menù inside Cinemachine")]
    [SerializeField] private float zoomAmount;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Controllare che collision sia player
        if (collision.gameObject.GetComponent<Character>() != null)
        {
            PubSub.Instance.Notify(EMessageType.CameraSwitch, zoomAmount);
        }
    }
}
