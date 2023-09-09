using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndManager : MonoBehaviour
{
    [SerializeField] GameObject moire;
    [SerializeField] GameObject brokenEye;
    [SerializeField] SpriteRenderer cover;
    [SerializeField] PlayerController player;
    [SerializeField] float coverFadeOutDuration = 2f;

    bool startFadeOut;
    float elapsed;
    Color startColor;
    [SerializeField] Color endColor;

    private void Start()
    {
        moire.SetActive(false);
        brokenEye.SetActive(false);
        cover.gameObject.SetActive(false);
        startFadeOut = false;
        elapsed = 0;
    }


    internal void StartEnd()
    {
        moire.SetActive(true);
        brokenEye.SetActive(true);
        cover.gameObject.SetActive(true);
        startColor = cover.color;
        startFadeOut = true;
    }

    private void Update()
    {
        if(startFadeOut) CoverFadeOut();

        //Temporaneo per test
        if (Input.GetKeyDown(KeyCode.O))
        {
            StartEnd();
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
