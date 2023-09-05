using System.Collections;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;

public class HealthSection : MonoBehaviour
{
    [SerializeField] public Sprite activeSectionSprite;
    [SerializeField] public Sprite inactiveSectionSprite;

    Image healthPieceImage;

    private void OnEnable()
    {
        healthPieceImage = GetComponent<Image>();
    }

    public void ActivateSection(bool activate)
    {
        if (activate)
            healthPieceImage.sprite = activeSectionSprite;
        else
            healthPieceImage.sprite = inactiveSectionSprite;
    }
}
