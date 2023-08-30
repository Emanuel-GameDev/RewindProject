using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class MenuVideoButton : MenuButton
{

    TutorialMenu videoMenu;

    protected override void OnEnable()
    {
        base.OnEnable();
        videoMenu = GetComponentInParent<TutorialMenu>(true);
    }

    public void ChangeVideoMenu()
    {
        if (!videoMenu.videoPlayer.gameObject.activeSelf)
        {
            videoMenu.videoPlayer.gameObject.SetActive(true);
            videoMenu.videoButton.gameObject.SetActive(true);
        }

        videoMenu.videoPlayer.clip = GetComponent<VideoMenuData>().videoTutorial;
        videoMenu.descriptionBox.text = GetComponent<VideoMenuData>().videoDescription;
        videoMenu.fullscreenVideoPlayer.clip = GetComponent<VideoMenuData>().videoTutorial;
        videoMenu.videoPlayer.Prepare();
        StartCoroutine(Wait());

    }

    public void UnlockButton()
    {
        gameObject.SetActive(true);
        GetComponent<SaveObjState>().ChangeObjectStateOnReload(true);
    }

    IEnumerator Wait()
    {
        yield return new WaitUntil(()=>videoMenu.videoPlayer.isPrepared==true);

        videoMenu.videoPlayer.Play();
    }

}
