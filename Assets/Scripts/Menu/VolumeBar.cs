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
            _volume = Mathf.Clamp(value, -80, 10);
        }
    }

    BarSegment[] segments;
    public int id=3;

    private void OnEnable()
    {
        settingsMenu = GetComponentInParent<SettingsMenu>();
        segments = GetComponentsInChildren<BarSegment>();

        id = PlayerPrefs.GetInt("VolumeId", 2);
        Volume = PlayerPrefs.GetFloat("Volume", -5);

        for (int i = 0; i <= id; i++)
        {
            segments[i].SegmentAbilitated(true);
        }

        for (int i = id+1; i < segments.Length; i++)
        {
            segments[i].SegmentAbilitated(false);
        }

    }


    public void IncreaseVolume()
    {
        
        Volume += 5;

        if (Volume > 10)
            Volume = 10;

        if (Volume < -15)
            Volume = -15;
        

        if(id < segments.Length-1)
        {
            id++;


            segments[id].SegmentAbilitated(true);
        }
        
        settingsMenu.SetVolume(Volume);
        PlayerPrefs.SetInt("VolumeId", id);
        PlayerPrefs.SetFloat("Volume", Volume);
    }

    public void DecreasecreaseVolume()
    {
        Volume -= 5;

        if (Volume < -15)
            Volume = -80;


        if (id >= 0)
        {
            segments[id].SegmentAbilitated(false);
            id--;

        }

        settingsMenu.SetVolume(Volume);

        PlayerPrefs.SetInt("VolumeId", id);
        PlayerPrefs.SetFloat("Volume", Volume);
    }

    private void OnDisable()
    {
        PlayerPrefs.Save();
    }

}
