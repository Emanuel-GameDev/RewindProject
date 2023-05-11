using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParallaxEffect : MonoBehaviour
{
    private float length;
    private float startPosX;
    private float startPosY;

    [SerializeField] GameObject cam;
    [SerializeField] float hParallaxRatio;
    [SerializeField] float vParallaxRatio;

    public bool tmp = true;


    // Start is called before the first frame update
    void Start()
    {
        startPosX = transform.position.x;
        startPosY = transform.position.y;

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

        if (temp > startPosX + length) startPosX += length;
        else if (temp < startPosX - length) startPosX -= length;
    }

    private void VerticalParallaxEffect()
    {
        if (tmp) return;

        // Move background var based
        float dist = (cam.transform.position.y * vParallaxRatio);

        transform.position = new Vector3(transform.position.x, startPosY + dist, transform.position.z);

    }
}
