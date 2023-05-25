using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindData 
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;

    public RewindData(Vector3 position, Quaternion rotation, Vector3 localScale)
    {
        this.position = position;
        this.rotation = rotation;
        this.scale = localScale;
    }
}
