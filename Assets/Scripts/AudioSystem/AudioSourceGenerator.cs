using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class AudioSourceGenerator : MonoBehaviour
{
    public void PlaySound(AudioClip audio)
    {
        GameObject soundObject = new GameObject();

        AudioSource source = soundObject.AddComponent<AudioSource>();
        
        source.outputAudioMixerGroup = GameManager.Instance.audioManager.mixer;
        source.clip = audio;
        source.volume = 0.5f;
        soundObject.GetComponent<AudioSource>().Play();

        Destroy(soundObject, audio.length);
    }

}
