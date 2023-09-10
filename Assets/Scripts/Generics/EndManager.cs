using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    [SerializeField] GameObject moire;
    [SerializeField] GameObject brokenEye;
    [SerializeField] GameObject exitEndTrigger;
    [SerializeField] SpriteRenderer cover;
    [SerializeField] PlayerController player;
    [SerializeField] float coverFadeOutDuration = 5f;
    [SerializeField] float coverDurationBeforeFadeOut = 3f;

    bool startFadeOut;
    bool startScene;
    float elapsed;
    Color startColor;
    [SerializeField] Color endColor;

    private void Start()
    {
        moire.SetActive(false);
        brokenEye.SetActive(false);
        cover.gameObject.SetActive(false);
        startFadeOut = false;
        startScene = false;
        elapsed = 0;
    }


    internal void StartEnd()
    {
        moire.SetActive(true);
        brokenEye.SetActive(true);
        cover.gameObject.SetActive(true);
        startColor = cover.color;
        startScene = true;
    }

    private void Update()
    {
        if (startScene) Wait();
        if(startFadeOut) CoverFadeOut();

        //Temporaneo per test
        if (Input.GetKeyDown(KeyCode.O))
        {
            StartEnd();
        }

    }

    private void Wait()
    {
        if(elapsed < coverDurationBeforeFadeOut)
        {
            elapsed += Time.deltaTime;
        }
        else
        {
            startFadeOut = true;
            startScene = false;
            elapsed = 0;
        } 
    }

    private void CoverFadeOut()
    {
        if (elapsed < coverFadeOutDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / coverFadeOutDuration;
            Color newColor = Color.Lerp(startColor, endColor, t);
            cover.color = newColor;
        }
        else
        {
            player.inputs.Enable();
            startFadeOut = false;

        }
    }
}
