using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [Tooltip("New value for orthografic size in the Lens menù inside Cinemachine")]
    [SerializeField] private float zoomAmount;

    [Tooltip("New value for offset in the Body menù inside Cinemachine")]
    [SerializeField] private Vector3 followOffset;

    [Tooltip("New value for damping in the Body menù inside Cinemachine")]
    [SerializeField] private Vector2 damping;

    CameraData cameraData;
    private bool triggered = false;

    private void Start()
    {
        cameraData = new CameraData(zoomAmount, followOffset, damping);
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Check player collision
        if (collision.gameObject.GetComponent<Character>() != null && !triggered)
        {
            PubSub.Instance.Notify(EMessageType.CameraSwitch, cameraData);
            triggered = true;
        }
    }
}
