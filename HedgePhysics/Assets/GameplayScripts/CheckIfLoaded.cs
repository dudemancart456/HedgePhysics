using UnityEngine;
using System.Collections;

public class CheckIfLoaded : MonoBehaviour
{
    void Start()
    {
        LoadingScreenControl.StageLoaded = true;
    }
}