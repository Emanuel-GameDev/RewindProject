using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraTrigger : MonoBehaviour
{
    [Tooltip("New value for orthografic size in the Lens menù inside Cinemachine")]
    [SerializeField] private float zoomAmount;

    [Tooltip("New value for offset in the Lens Body inside Cinemachine")]
    [SerializeField] private float offsetY;

    [Tooltip("New value for deadZoneWidth in the Body menù inside Cinemachine")]
    [Range(0f, 2f)]
    [SerializeField] private float deadZoneX;
    [Tooltip("New value for deadZoneHeight in the Body menù inside Cinemachine")]
    [Range(0f, 2f)]
    [SerializeField] private float deadZoneY;

    [Tooltip("New value for screenY in the Body menù inside Cinemachine")]
    [Range(0f, 1.5f)]
    [SerializeField] private float screenY;

    private List<float> newValues;

    private void Start()
    {
        newValues = new List<float>() { zoomAmount, offsetY, deadZoneX, deadZoneY, screenY};
    }
    private void OnTriggerEnter2D(Collider2D collision)
    {
        // Controllare che collision sia player
        if (collision.gameObject.GetComponent<Character>() != null)
        {
            PubSub.Instance.Notify(EMessageType.CameraSwitch, newValues);
        }
    }
}
