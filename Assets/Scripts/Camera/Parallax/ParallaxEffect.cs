using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float length;
    private float startPosX;
    private float startPosY;

    private List<Transform> _backgrounds = new List<Transform>();

    [SerializeField] GameObject cam;

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
        startPosX = transform.position.x;
        startPosY = transform.position.y;

        for (int i = 0; i < transform.childCount; i++)
        {
            _backgrounds.Add(transform.GetChild(i));
        }

        length = GetComponent<SpriteRenderer>().bounds.size.x;
    }

    // Update is called once per frame

    void Update()
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

        if (temp >= startPosX + length) startPosX += length; // Shift right
        else if (temp <= startPosX - length) startPosX -= length;    // Shift left
    }

    private void VerticalParallaxEffect()
    {
        // Move background var based
        float dist = (cam.transform.position.y * vParallaxRatio);

        transform.position = new Vector3(transform.position.x, startPosY + dist, transform.position.z);
    }
}
