using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Video;

public class MenuVideoButton : MenuButton
{
    [SerializeField] VideoClip videoTutorial;
    [SerializeField] string videoDescription;

    TutorialMenu videoMenu;

    public override void OnEnable()
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

        videoMenu.videoPlayer.clip = videoTutorial;
        videoMenu.descriptionBox.text = videoDescription;
        videoMenu.fullscreenVideoPlayer.clip = videoTutorial;
        videoMenu.videoPlayer.Prepare();
        StartCoroutine(Wait());

    }

    public void UnlockButton()
    {
        GetComponent<SaveObjState>().ChangeObjectState(true);
    }

    IEnumerator Wait()
    {
        yield return new WaitUntil(()=>videoMenu.videoPlayer.isPrepared==true);

        videoMenu.videoPlayer.Play();
    }

}
