using UnityEngine;
using System.Collections;

public class Action05_Rail : MonoBehaviour {

    public Animator CharacterAnimator;
    Quaternion CharRot;

    public Rail_Interaction rail;
    public float skinRotationSpeed = 1;

    void Update()
    {

        //Set Player's rotation while on rails
        if (rail.rail != null)
        {
            CharacterAnimator.SetInteger("Action", 5);
            CharRot = Quaternion.LookRotation(rail.rail.RailArray[rail.currentSeg + 1] - transform.position);
            CharacterAnimator.transform.rotation = Quaternion.Lerp(CharacterAnimator.transform.rotation, CharRot, Time.deltaTime * skinRotationSpeed);
        }
    }

}
