using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class MenuCards : Menu
{
    public TextMeshProUGUI cardName;
    public TextMeshProUGUI descriptionBox;
    public Image cardImageBox;
    public Image fullscreenCardImageBox;
    public GameObject artworkSelection;
    public GameObject descriptionSelection;
    public Scrollbar scrollbar;



    public override void OnEnable()
    {
        base.OnEnable();
        cardName.text = "";
        descriptionBox.text = "";
        fullscreenCardImageBox.sprite = null;
        artworkSelection.gameObject.SetActive(false);
        descriptionSelection.gameObject.SetActive(false);
    }

    
}
