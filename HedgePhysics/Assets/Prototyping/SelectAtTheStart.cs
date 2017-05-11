using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SelectAtTheStart : MonoBehaviour
{
    void OnEnable()
    {
        GetComponent<Button>().Select();
    }
}