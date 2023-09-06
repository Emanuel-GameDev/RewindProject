using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.EventSystems;
using UnityEngine.Events;
using TMPro;
using UnityEditor;
using UnityEngine.UI;


public class MenuButton :  Button,ISelectHandler
{
    internal TextMeshProUGUI buttonTextUI;

    AudioSource audioSource;
    AudioClip buttonSelectedSound;
    AudioClip buttonSubmitSound;

    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        if (!buttonSelectedSound)
            return;

        if (!GetComponentInParent<MenuManager>().audioSource.isPlaying)
        {
            audioSource.volume = 0.8f;
            PlayClick(buttonSelectedSound);
        }

    }

    //public override void OnSubmit(BaseEventData eventData)
    //{
    //    base.OnSubmit(eventData);

    //    if (!GetComponentInParent<MenuManager>())
    //        return;

    //    if (!GetComponentInParent<MenuManager>().audioSource.isPlaying)
    //    {
    //        audioSource.volume = 0.2f;
    //        PlayClick(buttonSubmitSound);
    //    }
    //}

    public void PlayClick(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.Play();
    }

    public virtual void UnlockButton()
    {
        gameObject.SetActive(true);
        GetComponent<SaveUniqueObj>().ChangeObjectStateOnReload(true);
    }

    protected override void OnEnable()
    {
        base.OnEnable();
        buttonTextUI = GetComponentInChildren<TextMeshProUGUI>();


        if (GetComponentInParent<MenuManager>())
        {
            buttonSelectedSound = GetComponentInParent<MenuManager>().buttonSelectedSound;
            buttonSubmitSound = GetComponentInParent<MenuManager>().buttonClickSound;
        }

        if (GetComponent<AudioSource>())
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.volume = 0.5f;
        }
        else
        {
            gameObject.AddComponent(typeof(AudioSource));
            audioSource = GetComponent<AudioSource>();
            audioSource.playOnAwake = false;
            audioSource.volume = 0.5f;
        }
    }

    public void LoadLevel(SceneAsset levelToLoad)
    {
        LevelManager.instance.LoadLevel(levelToLoad);
    }

   
}
