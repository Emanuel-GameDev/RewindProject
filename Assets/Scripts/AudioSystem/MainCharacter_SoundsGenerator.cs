using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class MainCharacter_SoundsGenerator : AudioSourceGenerator
{
    [SerializeField] List<AudioClip> footStepSounds;

    

    public void PlayFootStepSound()
    {
        PlaySound(footStepSounds[Random.Range(0, footStepSounds.Count)]);
    }
}
