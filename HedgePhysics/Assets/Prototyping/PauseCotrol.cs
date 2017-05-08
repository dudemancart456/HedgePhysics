using System.Collections;
using System.Collections.Generic;
using UnityEngine.Audio;
using UnityEngine.UI;
using UnityEngine;

public class PauseCotrol : MonoBehaviour {

    public HedgeCamera Cam;

    public GameObject Pause;
    public GameObject OptionsMenu;

    [Header ("Options menu stuff")]

    public AudioMixer Music;
    public AudioMixer Sfx;

    public Slider MusicSlider;
    public Slider SfxSlider;

    public Button PauseMainButton;
    public Image Yinvert;
    public Image Xinvert;
    public Slider SensX;
    public Slider SensY;

    void Start () {

        Pause.SetActive(false);
        OptionsMenu.SetActive(false);

        //VOLUME SETS
        if (!PlayerPrefs.HasKey("MUSIC_VOL"))
        {
            PlayerPrefs.SetFloat("MUSIC_VOL", 1);
            SetVolume();
        }
        if (!PlayerPrefs.HasKey("SFX_VOL"))
        {
            PlayerPrefs.SetFloat("SFX_VOL", 1);
            SetVolume();
        }
        //SENSITIVITY SET
        if (!PlayerPrefs.HasKey("X_SENS"))
        {
            PlayerPrefs.SetFloat("X_SENS", 1);
        }
        if (!PlayerPrefs.HasKey("Y_SENS"))
        {
            PlayerPrefs.SetFloat("Y_SENS", 1);
        }
        //INVERT SET
        if (!PlayerPrefs.HasKey("X_INV"))
        {
            PlayerPrefs.SetInt("X_INV", 0);
        }
        if (!PlayerPrefs.HasKey("Y_INV"))
        {
            PlayerPrefs.SetInt("Y_INV", 0);
        }

        InitialSetSliders();
        InitialIconsSet();

    }

    void Update()
    {
        if (Input.GetButtonDown("Start"))
        {
            OptionsMenu.SetActive(false);
            PauseToggle();
        }
        if (Time.timeScale != 0)
        {
            OptionsMenu.SetActive(false);
        }
        SetVolume();
    }

    public void PauseToggle () {

        if (Pause.activeSelf)
        {
            Pause.SetActive(false);
            Time.timeScale = 1;
        }
        else
        {
            Pause.SetActive(true);
            Time.timeScale = 0;
        }

	}
    public void SetPauseButton()
    {
        PauseMainButton.Select();
    }

    public void PauseToggleUnscaled()
    {

        if (Pause.activeSelf)
        {
            Pause.SetActive(false);
        }
        else
        {
            Pause.SetActive(true);
        }

    }

    public void Resume()
    {
        PauseToggle();
    }
    public void Quit()
    {
        Application.Quit();
    }
    public void OptionsToggle()
    {
        if (OptionsMenu.activeSelf)
        {
            OptionsMenu.SetActive(false);
        }
        else
        {
            OptionsMenu.SetActive(true);
        }
    }

    void InitialSetSliders()
    {
        MusicSlider.value = PlayerPrefs.GetFloat("MUSIC_VOL");
        SfxSlider.value = PlayerPrefs.GetFloat("SFX_VOL");
        SensX.value = PlayerPrefs.GetFloat("X_SENS");
        SensY.value = PlayerPrefs.GetFloat("Y_SENS");

        Cam.SensiX = PlayerPrefs.GetFloat("X_SENS");
        Cam.SensiY = PlayerPrefs.GetFloat("Y_SENS");
    }
    public void SetVolume()
    {
        PlayerPrefs.SetFloat("MUSIC_VOL", MusicSlider.value);
        PlayerPrefs.SetFloat("SFX_VOL", SfxSlider.value);

        Music.SetFloat("Volume", PlayerPrefs.GetFloat("MUSIC_VOL"));
        Sfx.SetFloat("Volume", PlayerPrefs.GetFloat("SFX_VOL"));

        //Set Sensitiviy
        PlayerPrefs.SetFloat("X_SENS", SensX.value);
        Cam.SensiX = SensX.value;
        PlayerPrefs.SetFloat("Y_SENS", SensY.value);
        Cam.SensiY = SensY.value;
    }

    public void InvetInput(bool isX)
    {
        if (isX)
        {
            if (PlayerPrefs.GetInt("X_INV") == 0)
            {
                PlayerPrefs.SetInt("X_INV", 1);
                SetInvertIcons(true, true);
                Cam.InvertedX = -1;
            }
            else
            {
                PlayerPrefs.SetInt("X_INV", 0);
                SetInvertIcons(false, true);
                Cam.InvertedX = 1;
            }
        }
        else
        {
            if (PlayerPrefs.GetInt("Y_INV") == 0)
            {
                PlayerPrefs.SetInt("Y_INV", 1);
                SetInvertIcons(true, false);
                Cam.InvertedY = -1;
            }
            else
            {
                PlayerPrefs.SetInt("Y_INV", 0);
                SetInvertIcons(false, false);
                Cam.InvertedY = 1;
            }
        }
    }
    void InitialIconsSet()
    {
        Cam.SensiX = PlayerPrefs.GetFloat("X_SENS");
        Cam.SensiY = PlayerPrefs.GetFloat("Y_SENS");

        if (PlayerPrefs.GetInt("Y_INV") == 0)
        {
            Cam.InvertedY = 1;
            SetInvertIcons(false, false);
        }
        else
        {
            Cam.InvertedY = -1;
            SetInvertIcons(true, false);
        }
        if (PlayerPrefs.GetInt("X_INV") == 0)
        {
            Cam.InvertedX = 1;
            SetInvertIcons(false, true);
        }
        else
        {
            Cam.InvertedX = -1;
            SetInvertIcons(true, true);
        }

    }

    void SetInvertIcons(bool activate, bool isX)
    {
        if (isX)
        {
            Xinvert.enabled = activate;
        }
        else
        {
            Yinvert.enabled = activate;
        }
    }
}
