using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

public class TutorialMenu : Menu
{
    public TextMeshProUGUI descriptionBox;
    public VideoPlayer videoPlayer;
    public MenuButton videoButton;
    public VideoPlayer fullscreenVideoPlayer;

    public override void OnEnable()
    {
        base.OnEnable();
        videoPlayer.gameObject.SetActive(false);
        videoButton.gameObject.SetActive(false);
        descriptionBox.text = "";
    }

}
