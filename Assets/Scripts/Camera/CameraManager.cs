using Cinemachine;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class CameraManager : MonoBehaviour
{
    [SerializeField] CinemachineVirtualCamera mainCam;
    [SerializeField] List<CinemachineVirtualCamera> cameras = new List<CinemachineVirtualCamera>();

    private CinemachineBrain brain;

    // Per Debug
    public bool switchCam = false;

    // Start is called before the first frame update
    void Start()
    {
        brain = Camera.main.gameObject.GetComponent<CinemachineBrain>();
        Setup();
    }

    // Update is called once per frame
    void Update()
    {
        if (switchCam)
        {
            ActivateZoom();
            switchCam = false;
        }
    }

    private void ActivateZoom()
    {
        mainCam.gameObject.SetActive(false);
        cameras[0].gameObject.SetActive(true);
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
