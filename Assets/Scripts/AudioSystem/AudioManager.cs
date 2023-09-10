using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [SerializeField] public float volume = 0.75f;
    [SerializeField] float fadeDuration = 0.5f;

    [SerializeField] AudioClip themeClip;
    [SerializeField] AudioClip rewindClip;

    private AudioClip nextAudio;
    private float elapsedTime;

    [HideInInspector] public AudioMixerGroup mixer;

    private void OnEnable()
    {
        audioSource = GetComponent<AudioSource>();
        mixer = audioSource.outputAudioMixerGroup;
    }

    void Start()
    {
        PubSub.Instance.RegisterFunction(EMessageType.TimeRewindStart, StartRewindClip);
        PubSub.Instance.RegisterFunction(EMessageType.TimeRewindStop, StopRewindClip);
        PlayTrack(themeClip);
    }

    private void StopRewindClip(object obj)
    {
        StartCoroutine(FadeOut(themeClip));
    }

    private void StartRewindClip(object obj)
    {
        StartCoroutine(FadeOut(rewindClip));
    }

    private void PlayTrack(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.Play();
    }

    private System.Collections.IEnumerator FadeOut(AudioClip clip)
    {
        float startVolume = audioSource.volume;
        float timer = 0.0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            audioSource.volume = Mathf.Lerp(startVolume, 0.0f, timer / fadeDuration);
            yield return null;
        }

        audioSource.Stop();
        PlayTrack(clip);
    }

    ////Test
    //private void Update()
    //{
    //    if (Input.GetKeyDown(KeyCode.O))
    //    {
    //        StartRewindClip(this);
    //        Debug.Log("Start");
    //    }
    //    if (Input.GetKeyDown(KeyCode.P))
    //    {
    //        StopRewindClip(this);
    //        Debug.Log("Stop");
    //    }
    //}


}
