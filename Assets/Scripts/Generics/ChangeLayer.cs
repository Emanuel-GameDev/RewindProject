using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChangeLayer : MonoBehaviour
{
    public SpriteRenderer spriteRenderer;
    public int newLayerid = 8;

    public void changeLayer()
    {
        spriteRenderer.sortingLayerID = newLayerid;
    }
}
