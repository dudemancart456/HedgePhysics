using UnityEngine;
using System.Collections;

public class PlayerSkin : MonoBehaviour
{
    public Animator CharacterAnimator;
    public PlayerBhysics Player;

    public float skinRotationSpeed;

    void Update()
    {
        CharacterAnimator.SetInteger("Action", 0);
        CharacterAnimator.SetFloat("GroundSpeed", GetComponent<Rigidbody>().velocity.magnitude);

        Quaternion CharRot = Quaternion.LookRotation(GetComponent<Rigidbody>().velocity, transform.up);
        CharacterAnimator.transform.rotation = Quaternion.Lerp(CharacterAnimator.transform.rotation, CharRot,
            Time.deltaTime * skinRotationSpeed);
    }
}