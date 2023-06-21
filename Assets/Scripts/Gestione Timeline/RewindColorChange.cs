using System;
using System.Collections;
using System.Collections.Generic;
using ToolBox.Serialization.OdinSerializer.Utilities;
using UnityEngine;
using UnityEngine.UI;

public class RewindColorChange : MonoBehaviour
{
    [SerializeField] Color rewindColor1;
    [SerializeField] Color rewindColor2;
    [SerializeField] Image image;

    [SerializeField] Sprite sprite1;
    [SerializeField] Sprite sprite2;

    [SerializeField] float fadeDuration = 0.5f;
    [SerializeField] float loopDuration = 1.5f;

    private Color startColor;
    private Color tempColor;

    private float elapsedTime = 0;

    private bool fadeIn = false;
    private bool fadeOut = false;
    private bool colorLoopActive = false;
    private bool from1to2 = false;

    private void Start()
    {
        image = GetComponent<Image>();
        startColor = new Color(1f, 1f, 1f, 0f);
        image.color = startColor;
        PubSub.Instance.RegisterFunction(EMessageType.TimeRewindStart, TimeRewindStart);
        PubSub.Instance.RegisterFunction(EMessageType.TimeRewindStop, TimeRewindStop);
    }

    private void Update()
    {
        if (fadeIn) FadeIn();
        if (fadeOut) FadeOut();
        if (colorLoopActive) LoopColor();
    }

    private void LoopColor()
    {
        if (from1to2)
        {
            if (!ChangeColor(rewindColor1, rewindColor2, loopDuration))
            {
                elapsedTime = 0;
                from1to2 = false;
            }
        }
        else
        {
            if (!ChangeColor(rewindColor2, rewindColor1, loopDuration))
            {
                elapsedTime = 0;
                from1to2 = true;
            }
        }
    }

    private void FadeOut()
    {
        if(!ChangeColor(tempColor, startColor, fadeDuration))
        {
            fadeOut = false;
            elapsedTime = 0;
        }
    }

    private void FadeIn()
    {
        if(!ChangeColor(startColor, rewindColor1,fadeDuration))
        {
            colorLoopActive = true;
            fadeIn = false;
            elapsedTime = 0;
            from1to2 = true;
        }
    }

    private bool ChangeColor(Color initialColor, Color endColor, float duration)
    {
        elapsedTime += Time.deltaTime;
        if (elapsedTime < duration)
        {
            float t = elapsedTime / duration;
            Color currentColor = Color.Lerp(initialColor, endColor, t);
            image.color = currentColor;
            return true;
        }
        else
        {
            return false;
        }
    }

    private void TimeRewindStop(object obj)
    {
        tempColor = image.color;
        FadeInOn(false);
    }

    private void TimeRewindStart(object obj)
    {
        if (obj is Rewindable)
            image.sprite = sprite1;
        else
            image.sprite = sprite2;
        FadeInOn(true);
    }

    private void FadeInOn(bool v)
    {
        fadeIn = v;
        fadeOut = !v;
        colorLoopActive = false;
        elapsedTime = 0;
    }
}
