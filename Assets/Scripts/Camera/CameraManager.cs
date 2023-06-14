using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : MonoBehaviour
{
    public CinemachineVirtualCamera mainCam;

    [Tooltip("Start value for orthografic size in the Lens menù inside Cinemachine")]
    public float StartZoomAmount = 7f;

    [Tooltip("Speed of the transition\n " +
        "N.B. the action is an interpolation so it scales over time")]
    [SerializeField] private float zoomSpeed;

    private float currentZoom;

    // Start is called before the first frame update
    void Start()
    {
        PubSub.Instance.RegisterFunction(EMessageType.CameraSwitch, UpdateCamera);
        mainCam.gameObject.SetActive(true);

        Initilize();
    }

    private void Initilize()
    {
        mainCam.m_Lens.OrthographicSize = StartZoomAmount;
        currentZoom = StartZoomAmount;

    }    

    private void UpdateCamera(object obj)
    {
        if (obj is not List<float>) return;
        List<float> newValues = (List<float>)obj;

        float zoomAmount = newValues[0];
        if (zoomAmount >= 0 || zoomAmount != currentZoom)
        {
            StartCoroutine(Zoom(newValues));
        }

        //float offsetAmountY = newValues[1];
        //float offsetDashMultiplier = newValues[2];
        //float deadZoneXAmount = newValues[3];
        //float deadZoneYAmount = newValues[4];
        //float screenY = newValues[4];

        //CinemachineFramingTransposer transposer = mainCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        //transposer.m_DeadZoneWidth = deadZoneXAmount;
        //transposer.m_DeadZoneHeight = deadZoneYAmount;
        //transposer.m_ScreenY = screenY;

        //OffsetUpdate(offsetAmountY, offsetDashMultiplier);

    }

    private void OffsetUpdate(float offsetAmountY, float offsetDash)
    {
        CinemachineFramingTransposer transposer = mainCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        transposer.m_TrackedObjectOffset.y = offsetAmountY * offsetDash;
        transposer.m_TrackedObjectOffset.y = offsetAmountY;
    }

    private IEnumerator Zoom(List<float> newValues)
    {
        while (Mathf.Abs(currentZoom - newValues[0]) > 0.01f)
        {
            currentZoom = Mathf.Lerp(currentZoom, newValues[0], zoomSpeed * Time.deltaTime);
            mainCam.m_Lens.OrthographicSize = currentZoom;
        }

        float offsetAmountY = newValues[1];
        float offsetDashMultiplier = newValues[2];
        OffsetUpdate(offsetAmountY, offsetDashMultiplier);

        float deadZoneXAmount = newValues[3];
        float deadZoneYAmount = newValues[4];
        float screenY = newValues[4];

        CinemachineFramingTransposer transposer = mainCam.GetCinemachineComponent<CinemachineFramingTransposer>();
        transposer.m_DeadZoneWidth = deadZoneXAmount;
        transposer.m_DeadZoneHeight = deadZoneYAmount;
        transposer.m_ScreenY = screenY;


        yield return null;
    }


}
