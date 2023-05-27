using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RewindData 
{
    public Vector3 position;
    public Quaternion rotation;
    public Vector3 scale;
    public Sprite sprite;
    public bool spriteFlipX;


    public RewindData(Vector3 position, Quaternion rotation, Vector3 localScale, Sprite sprite, bool flipX)
    {
        this.position = position;
        this.rotation = rotation;
        this.scale = localScale;
        this.sprite = sprite;
        this.spriteFlipX = flipX;
    }
}
