using Mono.CompilerServices.SymbolWriter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSourceGenerator : MonoBehaviour
{
    public AudioMixerGroup mixer;
    public bool hearOnlyOnProximity = false;
    public float maxHearingDistance = 500f;
    public float volume = 0.5f;

    public void PlaySound(AudioClip audio)
    {
        GameObject soundObject = new GameObject();

        AudioSource source = soundObject.AddComponent<AudioSource>();
        
        source.outputAudioMixerGroup = mixer;
        source.clip = audio;
        source.volume = volume;
        if (hearOnlyOnProximity)
        {
            source.spatialBlend = 1;
            source.rolloffMode = AudioRolloffMode.Linear;
            source.maxDistance = maxHearingDistance;
        }
        soundObject.GetComponent<AudioSource>().Play();

        Destroy(soundObject, audio.length);
    }

}
