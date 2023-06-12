using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AudioSourceGenerator : MonoBehaviour
{
    public void PlaySound(AudioClip audio)
    {
        GameObject soundObject = new GameObject();

        soundObject.AddComponent<AudioSource>();

        soundObject.GetComponent<AudioSource>().volume = 0.5f;
        soundObject.GetComponent<AudioSource>().clip = audio;

        soundObject.GetComponent<AudioSource>().Play();

        Destroy(soundObject, audio.length);
    }

}
