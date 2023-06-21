using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpriteLineHidener : MonoBehaviour
{
    [SerializeField] List<SpriteRenderer> targetSprites;
    [SerializeField] List<LineRenderer> targetLines;

    public void Hide()
    {
        foreach (SpriteRenderer sprite in targetSprites) 
        { 
            sprite.enabled = false;
        }

        foreach (LineRenderer line in targetLines)
        {
            Color color = line.startColor;
            color.a = 0f;
            line.startColor = color;
            line.endColor = color;
        }
    }

    public void Show() 
    {
        foreach (SpriteRenderer sprite in targetSprites)
        {
            sprite.enabled = true;
        }

        foreach (LineRenderer line in targetLines)
        {
            Color color = line.startColor;
            color.a = 255f;
            line.startColor = color;
            line.endColor = color;
        }
    }


}
