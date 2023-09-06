using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using UnityEngine.Video;

public class MenuVideoButton : MenuButton
{

    TutorialMenu videoMenu;

    protected override void OnEnable()
    {
        base.OnEnable();
        videoMenu = GetComponentInParent<TutorialMenu>();
    }

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        onClick.Invoke();
    }


    public void ChangeVideoMenu()
    {
        if (!videoMenu.videoPlayer.gameObject.activeSelf)
        {
            videoMenu.videoPlayer.gameObject.SetActive(true);
            videoMenu.videoButton.gameObject.SetActive(true);
        }
        

        videoMenu.videoPlayer.clip = GetComponent<VideoMenuData>().videoTutorial;
        videoMenu.descriptionBox.text = GetComponentInChildren<Text>().text;
        videoMenu.fullscreenVideoPlayer.clip = GetComponent<VideoMenuData>().videoTutorial;
        videoMenu.videoPlayer.Prepare();
        StartCoroutine(Wait());

        videoMenu.eventSystemDefaultButton = this;
    }

   
    IEnumerator Wait()
    {
        yield return new WaitUntil(()=>videoMenu.videoPlayer.isPrepared==true);

        videoMenu.videoPlayer.Play();
    }

}
