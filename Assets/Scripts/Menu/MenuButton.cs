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

    public bool locked = false;
    AudioSource audioSource;
    AudioClip buttonSelectedSound;
    
    public override void OnSelect(BaseEventData eventData)
    {
        base.OnSelect(eventData);

        if (!buttonSelectedSound)
            return;

        if (GetComponentInParent<MenuManager>().audioSource.isPlaying)
            return;

        //PlayClick();
    }

    public void PlayClick()
    {
        audioSource.clip = buttonSelectedSound;
        audioSource.Play();
    }

    //public override void OnPointerClick(PointerEventData eventData)
    //{
    //    if (!locked)
    //        OnClick.Invoke();
    //}

    //public override void OnPointerEnter(PointerEventData eventData)
    //{
    //    if (!locked)
    //    {
    //        SelectButton();
    //    }
    //}

    //private void SelectButton()
    //{
    //    if (buttonTextUI)
    //        buttonTextUI.color = selectedColor;
    //    if (icon)
    //        icon.color = selectedColor;
    //}

    //public override void OnPointerExit(PointerEventData eventData)
    //{
    //    if (!locked)
    //    {
    //        DeselectButton();
    //    }

    //}

    //private void DeselectButton()
    //{
    //    if (buttonTextUI)
    //        buttonTextUI.color = baseColor;
    //    if (icon)
    //        icon.color = baseColor;
    //}

    protected override void OnEnable()
    {
        base.OnEnable();
        buttonTextUI = GetComponentInChildren<TextMeshProUGUI>();

        if(GetComponentInParent<MenuManager>())
            buttonSelectedSound = GetComponentInParent<MenuManager>().buttonSelectedSound;

        if (GetComponent<AudioSource>())
        {
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = 20;
        }
        else
        {
            gameObject.AddComponent(typeof(AudioSource));
            audioSource = GetComponent<AudioSource>();
            audioSource.volume = 20;
        }
        //if (locked)
        //{
        //    if (buttonTextUI)
        //        buttonTextUI.color = Color.gray;
        //    if (icon)
        //        icon.color = Color.gray;
        //}
        //else
        //{
        //    //DeselectButton();
        //}
    }

    public void LoadLevel(SceneAsset levelToLoad)
    {
        SceneManager.LoadScene(levelToLoad.name);
    }
}
