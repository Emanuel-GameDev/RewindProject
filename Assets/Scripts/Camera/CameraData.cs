using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class CameraData
{
    public float zoomAmount;
    public Vector3 offset;
    public Vector2 damping;

    public CameraData(float zoomAmount, Vector3 offset, Vector2 damping)
    {
        this.zoomAmount = zoomAmount;
        this.offset = offset;
        this.damping = damping;
    }
}
