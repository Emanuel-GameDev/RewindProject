using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class ParallaxEffect : MonoBehaviour
{
    private float startPosX;
    private float startPosY;

    private CinemachineVirtualCamera cam;

    [SerializeField] private float length = 30f;
    [SerializeField] private float shiftOffset = 5;

    [Tooltip("Speed of the horizontal parallax effect\n" +
        "(1 = not moving and 0 = same speed of player)")]
    [Range(0f, 1f)]
    [SerializeField] float hParallaxRatio;

    [Tooltip("Speed of the vertical parallax effect\n" +
    "(1 = not moving and 0 = same speed of player)")]
    [Range(0f, 1f)]
    [SerializeField] float vParallaxRatio;

    // Start is called before the first frame update
    void Start()
    {
        cam = GameManager.Instance.cameraManager.mainCam;

        startPosX = transform.position.x;
        startPosY = transform.position.y;

        transform.position = new Vector3(cam.transform.position.x, cam.transform.position.y, 0);
    }

    // Update is called once per frame
    void LateUpdate()
    {
        // handle of the parallax in X axis
        HorizontalParallaxEffect();

        // handle of the parallax in Y axis
        VerticalParallaxEffect();
    }

    private void HorizontalParallaxEffect()
    {
        // Move background var based
        float dist = (cam.transform.position.x * hParallaxRatio);

        transform.position = new Vector3(startPosX + dist, transform.position.y, transform.position.z);

        // Make background repeat itself
        float temp = (cam.transform.position.x * (1 - hParallaxRatio));

        if (temp >= startPosX + ((length*2) - shiftOffset))
        {
            startPosX += (length*2);
        }
        else if (temp <= startPosX - ((length * 2) - shiftOffset))
        {
            startPosX -= (length*2);
        }
    }

    private void VerticalParallaxEffect()
    {
        // Move background var based
        float dist = (cam.transform.position.y * vParallaxRatio);

        transform.position = new Vector3(transform.position.x, startPosY + dist, transform.position.z);
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawLine(new Vector3(startPosX, 0), new Vector3(0, 10f));
    }
}
