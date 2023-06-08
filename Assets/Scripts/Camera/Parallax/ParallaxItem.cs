using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ParallaxItem : MonoBehaviour
{
    private CinemachineVirtualCamera mainCam;

    [SerializeField] private Vector2 backgroundSpeed;

    private Vector3 lastCamPos;
    private float lenght;

    private void Start()
    {
        mainCam = GameManager.Instance.cameraManager.mainCam;
        lastCamPos = mainCam.transform.position;
        lenght = GetComponent<SpriteRenderer>().size.x;
    }


    private void LateUpdate()
    {
    //    // Di quanto si sta spostando la telecamera
    //    Vector3 deltaMovement = mainCam.transform.position - lastCamPos;

    //    // 1 si muove come telecamera 0 sta fermo
    //    _backgrounds[1].position += new Vector3(deltaMovement.x * backgroundSpeed.x, deltaMovement.y * backgroundSpeed.y);
    //    lastCamPos = mainCam.transform.position;

    //    if (mainCam.transform.position.x - _backgrounds[1].position.x > lenght)
    //    {
    //        //transform.position = new Vector3(transform.position.x + lenght, transform.position.y);
    //        Debug.Log("shift right");
    //    }
    //    else if (mainCam.transform.position.x - transform.position.x < -lenght)
    //    {
    //        //transform.position = new Vector3(transform.position.x - lenght, transform.position.y);
    //        Debug.Log("Shift left");
    //    }
    }
}
