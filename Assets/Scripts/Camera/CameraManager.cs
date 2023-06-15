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
    [SerializeField] private float transitionSpeed;

    private float currentZoom;
    private Vector3 currentOffset;
    private Vector2 currentDamping;


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

        currentOffset = mainCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset;
        currentDamping.x = mainCam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping;
        currentDamping.y = mainCam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping;
    }

    private void UpdateCamera(object obj)
    {
        if (obj is not CameraData) return;
        CameraData cameraData = (CameraData)obj;

        if (cameraData.zoomAmount >= 0 && cameraData.zoomAmount != currentZoom)
        {
            StartCoroutine(AdjustCamera(cameraData.zoomAmount, cameraData.offset, cameraData.damping));
        }
    }

    private IEnumerator AdjustCamera(float targetZoom, Vector3 targetOffset, Vector2 targetDamping)
    {
        while (Mathf.Abs(currentZoom - targetZoom) > 0.01f)
        {
            currentZoom = Mathf.Lerp(currentZoom, targetZoom, transitionSpeed * Time.deltaTime);

            for (int i = 0; i < 3; i++)
            {
                if (currentOffset[i] != targetOffset[i])
                {
                    currentOffset[i] = Mathf.Lerp(currentOffset[i], targetOffset[i], transitionSpeed * Time.deltaTime);
                }
                if (currentDamping[i] != targetDamping[i] && i < 3)
                {
                    currentDamping[i] = Mathf.Lerp(currentDamping[i], targetDamping[i], transitionSpeed * Time.deltaTime);
                }
            }

            mainCam.GetCinemachineComponent<CinemachineTransposer>().m_XDamping = currentDamping.x;
            mainCam.GetCinemachineComponent<CinemachineTransposer>().m_YDamping = currentDamping.y;
            mainCam.GetCinemachineComponent<CinemachineTransposer>().m_FollowOffset = currentOffset;
            mainCam.m_Lens.OrthographicSize = currentZoom;
            yield return null;

        }
    }



}
