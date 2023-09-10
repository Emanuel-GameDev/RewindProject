using Mono.CompilerServices.SymbolWriter;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSourceGenerator : MonoBehaviour
{
    public AudioMixerGroup mixer;
    public float maxHearingDistance = 15f;
    public void PlaySound(AudioClip audio)
    {
        GameObject soundObject = new GameObject();

        AudioSource source = soundObject.AddComponent<AudioSource>();
        
        source.outputAudioMixerGroup = mixer;
        source.clip = audio;
        source.volume = 0.5f;
        source.maxDistance = maxHearingDistance;
        soundObject.GetComponent<AudioSource>().Play();

        Destroy(soundObject, audio.length);
    }

}
