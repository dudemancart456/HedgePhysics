using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;

public class TitleScreenControl : MonoBehaviour
{
    public Image BlackFade;
    public Animator PressStartAnim;
    public float PressStartIncreaceSpeed;
    public UnityStandardAssets.Utility.AutoMoveAndRotate HedgeSpin;
    public float LogoRotationAccell;
    public float EndStageAt;
    public AudioSource Source;
    bool End = false;
    float counter;


    void Start()
    {
        BlackFade.enabled = true;
    }

    void Update()
    {
        if (End)
        {
            counter += Time.deltaTime;
            HedgeSpin.rotateDegreesPerSecond.value.z += LogoRotationAccell * Time.deltaTime;
            PressStartAnim.speed = PressStartIncreaceSpeed;
            if (counter > EndStageAt)
            {
                BlackFade.color = Color.Lerp(BlackFade.color, Color.black, Time.deltaTime * 8);
                if (counter > EndStageAt + 1.5f)
                {
                    Application.LoadLevel(1);
                }
            }
        }
        else
        {
            Color a = Color.black;
            a.a = 0;
            BlackFade.color = Color.Lerp(BlackFade.color, a, Time.deltaTime * 3);
        }

        if (Input.GetButtonDown("Start") || Input.GetButtonDown("A"))
        {
            if (!End)
            {
                Source.Play();
                End = true;
            }
        }
    }
}