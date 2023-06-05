using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera mainCam;

    [Tooltip("Start value for orthografic size in the Lens menù inside Cinemachine")]
    public float StartZoomAmount = 7f;

    [Tooltip("Speed of the zoom action\n " +
        "N.B. the action is an interpolation so it scales over time")]
    [SerializeField] private float zoomSpeed;

    private float currentZoom;

    // Start is called before the first frame update
    void Start()
    {
        PubSub.Instance.RegisterFunction(EMessageType.CameraSwitch, TriggerZoom);
        mainCam.m_Lens.OrthographicSize = StartZoomAmount;
        currentZoom = StartZoomAmount;
        mainCam.gameObject.SetActive(true);
    }

    private void TriggerZoom(object obj)
    {
        if (obj is not float) return;
        float zoomAmount = (float)obj;

        if (zoomAmount <= 0 || zoomAmount == currentZoom) return; 

        StartCoroutine(Zoom(zoomAmount));
    }

    private IEnumerator Zoom(float targetZoom)
    {
        while (Mathf.Abs(currentZoom - targetZoom) > 0.01f)
        {
            currentZoom = Mathf.Lerp(currentZoom, targetZoom, zoomSpeed * Time.deltaTime);
            mainCam.m_Lens.OrthographicSize = currentZoom;
            yield return null;
        }
    }


}
