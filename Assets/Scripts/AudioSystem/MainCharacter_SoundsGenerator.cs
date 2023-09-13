using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainCharacter_SoundsGenerator : AudioSourceGenerator
{
    [SerializeField] List<AudioClip> footStepSounds;
    [SerializeField] List<AudioClip> jumpSounds;
    [SerializeField] List<AudioClip> damageSounds;
    [SerializeField] List<AudioClip> dieSounds;



    public void PlayFootStepSound()
    {
        PlaySound(footStepSounds[Random.Range(0, footStepSounds.Count)]);
    }

    public void PlayJumpSound()
    {
        PlaySound(jumpSounds[Random.Range(0, jumpSounds.Count)]);
    }

    public void PlayDamagedSound()
    {
        PlaySound(damageSounds[Random.Range(0, damageSounds.Count)]);
    }

    public void PlayDieSound()
    {
        PlaySound(dieSounds[Random.Range(0, dieSounds.Count)]);
    }
}
