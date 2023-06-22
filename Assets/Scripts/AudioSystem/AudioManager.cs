using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioManager : MonoBehaviour
{
    [SerializeField] AudioSource audioSource;

    [SerializeField] float volume = 0.75f;
    [SerializeField] float fadeDuration = 0.5f;

    [SerializeField] AudioClip themeClip;
    [SerializeField] AudioClip rewindClip;

    private AudioClip nextAudio;
    private float elapsedTime;


    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
        PubSub.Instance.RegisterFunction(EMessageType.TimeRewindStart, StartRewindClip);
        PubSub.Instance.RegisterFunction(EMessageType.TimeRewindStop, StopRewindClip);
        PlayTrack(themeClip);
    }

    private void StopRewindClip(object obj)
    {
        throw new NotImplementedException();
    }

    private void StartRewindClip(object obj)
    {
        throw new NotImplementedException();
    }

    private void PlayTrack(AudioClip clip)
    {
        audioSource.clip = clip;
        audioSource.volume = volume;
        audioSource.loop = true;
        audioSource.Play();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FadeOut()
    {

    }

}
