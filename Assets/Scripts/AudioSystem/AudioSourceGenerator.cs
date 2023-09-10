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
        GameObject soundObject = CreateSource(audio);

        Destroy(soundObject, audio.length);
    }

    private GameObject CreateSource(AudioClip audio)
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
        return soundObject;
    }

    public GameObject PlaySoundRepeat(AudioClip audio)
    {
        GameObject soundObject = CreateSource(audio);

        soundObject.GetComponent<AudioSource>().loop = true;

        return soundObject;
    }

    public void DestroyRepeatingSound(GameObject GO)
    {
        Destroy(GO);
    }
}
