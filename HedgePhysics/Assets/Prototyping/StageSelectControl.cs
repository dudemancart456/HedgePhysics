using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class StageSelectControl : MonoBehaviour
{
    bool End;
    float Counter;
    public Image BlackFade;
    public float EndAt;
    int LevelToGo;

    public AudioSource MenuSounds;
    public AudioClip move;
    public AudioClip Click;

    void Start()
    {
        BlackFade.enabled = true;
    }

    void Update()
    {
        if (End)
        {
            Counter += Time.deltaTime;

            Color a = Color.black;
            a.a = 1;
            BlackFade.color = Color.Lerp(BlackFade.color, a, Time.deltaTime * 4);

            if (Counter > EndAt)
            {
                Application.LoadLevel(LevelToGo);
            }
        }
        else
        {
            Color a = Color.black;
            a.a = 0;
            BlackFade.color = Color.Lerp(BlackFade.color, a, Time.deltaTime * 4);
        }
    }

    public void GoToLoading(int loadingscreen)
    {
        LevelToGo = loadingscreen;
        End = true;
    }

    public void GoToLevel(int levelToLoad)
    {
        SceneController.LevelToLoad = levelToLoad;
        End = true;
    }

    public void SetStageNameLeft(string name)
    {
        LoadingScreenControl.StageName1 = name;
    }

    public void SetStageNameRight(string name)
    {
        LoadingScreenControl.StageName2 = name;
    }

    public void MoveSound()
    {
        MenuSounds.clip = move;
        MenuSounds.Play();
    }

    public void ClickSound()
    {
        MenuSounds.clip = Click;
        MenuSounds.Play();
    }
}