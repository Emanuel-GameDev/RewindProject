using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LineRendererFlip : MonoBehaviour
{
    private LineRenderer lineRenderer;

    private void Start()
    {
        lineRenderer = GetComponent<LineRenderer>();
        FlipLineRenderer();
    }

    private void FlipLineRenderer()
    {
        lineRenderer.material.mainTextureScale = new Vector2(-1f, 1f);
    }
}
