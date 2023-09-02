using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public class VolumeBar : MonoBehaviour
{
    public AudioMixer audioMixer;
    SettingsMenu settingsMenu;

    float _volume = 0;

    public float Volume
    {
        get => _volume;

        set
        {
            _volume = Mathf.Clamp(value, -80, 20);
        }
    }

    BarSegment[] segments;
    int id=0;

    private void OnEnable()
    {
        settingsMenu = GetComponentInParent<SettingsMenu>();
        segments = GetComponentsInChildren<BarSegment>();

        for (int i = 0; i < 4; i++)
        {
            segments[i].SegmentAbilitated(true);
        }
        id = 4;
    }


    public void IncreaseVolume()
    {
        
        Volume += 10;

        if (Volume > 20)
            Volume = 20;

        if (Volume < -30)
            Volume = -30;
        

        if(id < segments.Length)
        {
            id++;


            segments[id].SegmentAbilitated(true);
        }
        
        settingsMenu.SetVolume(Volume);
    }

    public void DecreasecreaseVolume()
    {
        Volume -= 10;

        if (Volume < -30)
            Volume = -80;


        if (id >= 0)
        {
            segments[id].SegmentAbilitated(false);
            id--;

        }

        settingsMenu.SetVolume(Volume);
    }

}
