using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;


    [SerializeField] AudioSource mainAudioSource;
    [SerializeField] AudioSource rewindAudioSource;
    [SerializeField] AudioMixer mixer;

    [SerializeField] float volume = 0.75f;
    [SerializeField] float fadeDuration = 0.5f;

    [SerializeField] AudioClip themeClip;
    [SerializeField] AudioClip loopClip;
    [SerializeField] AudioClip rewindClip;

    private void Awake()
    {
        if (Instance == null) Instance = this;
        else if (Instance != this) Destroy(gameObject);
    }

    void Start()
    {
        mainAudioSource = GetComponent<AudioSource>();
        PubSub.Instance.RegisterFunction(EMessageType.TimeRewindStart, StartRewindClip);
        PubSub.Instance.RegisterFunction(EMessageType.TimeRewindStop, StopRewindClip);
        PlayTrack(themeClip);
        rewindAudioSource.clip = rewindClip;
        StopRewindClip(this);
    }

    private void StopRewindClip(object obj)
    {
        rewindAudioSource.Stop();
        rewindAudioSource.time = 0;
    }

    private void StartRewindClip(object obj)
    {
        rewindAudioSource.Play();
    }

    private void PlayTrack(AudioClip clip)
    {
        mainAudioSource.clip = clip;
        mainAudioSource.volume = volume;
        mainAudioSource.loop = true;
        mainAudioSource.Play();
    }

    private System.Collections.IEnumerator FadeOut(AudioClip clip)
    {
        float startVolume = mainAudioSource.volume;
        float timer = 0.0f;

        while (timer < fadeDuration)
        {
            timer += Time.deltaTime;
            mainAudioSource.volume = Mathf.Lerp(startVolume, 0.0f, timer / fadeDuration);
            yield return null;
        }

        mainAudioSource.Stop();
        PlayTrack(clip);
    }
    
    public void ChangeMaintTheme(AudioClip audioClip)
    {
        StartCoroutine(FadeOut(audioClip));
    }

    public void ResetMainTheme()
    {
        StartCoroutine(FadeOut(themeClip));
    }

    public AudioMixer GetMixer()
    {
        return mixer;
    }

}
