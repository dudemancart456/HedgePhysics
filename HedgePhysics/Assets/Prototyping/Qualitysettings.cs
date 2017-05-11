using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Qualitysettings : MonoBehaviour
{
    public UnityStandardAssets.CinematicEffects.Bloom bloom;

    void Start()
    {
        SetQuality();
    }

    public void SetQuality()
    {
        int QS = QualitySettings.GetQualityLevel();

        switch (QS)
        {
            case 0:
                bloom.enabled = true;
                break;
            case 1:
                bloom.enabled = false;
                break;
            default:
                break;
        }
    }
}