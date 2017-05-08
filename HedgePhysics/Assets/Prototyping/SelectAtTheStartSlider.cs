using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class SelectAtTheStartSlider : MonoBehaviour {

    void OnEnable()
    {

        GetComponent<Slider>().Select();

    }
}
