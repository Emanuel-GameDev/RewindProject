using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BarSegment : MonoBehaviour
{
    [SerializeField] public Sprite disabledSegment;
    [SerializeField] public Sprite enabledSegment;

    public void SegmentAbilitated(bool abilitated)
    {
        if (abilitated)
            GetComponent<Image>().sprite = enabledSegment;
        else
            GetComponent<Image>().sprite = disabledSegment;
    }
}
