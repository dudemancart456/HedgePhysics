using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine;

public class LoadSound : MonoBehaviour
{
    public AudioMixer Music;
    public AudioMixer Sfx;

    void Start()
    {
        //VOLUME SETS
        if (!PlayerPrefs.HasKey("MUSIC_VOL"))
        {
            PlayerPrefs.SetFloat("MUSIC_VOL", 1);
        }
        if (!PlayerPrefs.HasKey("SFX_VOL"))
        {
            PlayerPrefs.SetFloat("SFX_VOL", 1);
        }

        SetVolume();
    }

    public void SetVolume()
    {
        Music.SetFloat("Volume", PlayerPrefs.GetFloat("MUSIC_VOL"));
        Sfx.SetFloat("Volume", PlayerPrefs.GetFloat("SFX_VOL"));
    }
}