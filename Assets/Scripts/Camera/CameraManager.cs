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
        Setup();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void TriggerZoom(object obj)
    {
        if (obj is not bool) return;
        bool mode = (bool)obj;

        mainCam.gameObject.SetActive(!mode);
        cameras[0].gameObject.SetActive(mode);
    }

    private void Setup()
    {
        foreach (CinemachineVirtualCamera cam in cameras)
        {
            cam.gameObject.SetActive(false);
        }

        mainCam.gameObject.SetActive(true);
    }
}
