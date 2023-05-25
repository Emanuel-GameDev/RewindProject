using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera mainCam;
    [SerializeField] List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();


    // Start is called before the first frame update
    void Start()
    {
        PubSub.Instance.RegisterFunction(EMessageType.CameraSwitch, TriggerZoom);
        TriggerCameras(false);
        mainCam.gameObject.SetActive(true);
    }

    private void TriggerZoom(object obj)
    {
        if (obj is not CinemachineVirtualCamera) return;

        CinemachineVirtualCamera camera = (CinemachineVirtualCamera)obj;

        if (camera == mainCam)
        {
            mainCam.gameObject.SetActive(true);
            TriggerCameras(false);
        }
        else
        {
            camera.gameObject.SetActive(true);
            mainCam.gameObject.SetActive(false);
        }
    }

    private void TriggerCameras(bool mode)
    {
        foreach (CinemachineVirtualCamera cam in cameras)
        {
            cam.gameObject.SetActive(mode);
        }
    }
}
