using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class SelectionSocket : MonoBehaviour
{
    private RectTransform currentImage;
    private AbilityWheel wheel;

    private void Start()
    {
        wheel = GetComponentInParent<AbilityWheel>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<RectTransform>() != null)
        {
            GameObject childGO = collision.gameObject.transform.GetChild(0).gameObject;
            RectTransform currentRect = childGO.GetComponent<RectTransform>();

            if (childGO.activeSelf && currentRect != currentImage)
            {
                currentImage = currentRect;
                //wheel.ScaleOverTime(currentRect, increase:true);
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.GetComponent<RectTransform>() != currentImage && currentImage != null)
        {
            //wheel.ScaleOverTime(currentImage, increase:false);
        }
    }

}
