using UnityEngine;
using System.Collections;

public class Action02_Homing : MonoBehaviour
{
    public ActionManager Action;
    public Animator CharacterAnimator;
    PlayerBhysics Player;

    public bool isAdditive;
    public float HomingAttackSpeed;
    public float AirDashSpeed;
    public float HomingTimerLimit;
    public float FacingAmmount;
    float Timer;
    float Speed;
    float Aspeed;
    Vector3 direction;

    public Transform Target { get; set; }
    public float skinRotationSpeed;
    public bool HomingAvailable { get; set; }
    public bool IsAirDash { get; set; }

    void Awake()
    {
        HomingAvailable = true;
        Player = GetComponent<PlayerBhysics>();
    }

    public void InitialEvents()
    {
        if (Action.Action02Control.HasTarget)
        {
            Target = HomingAttackControl.TargetObject.transform;
        }

        Timer = 0;
        HomingAvailable = false;

        if (isAdditive)
        {
            // Apply Max Speed Limit
            float XZmag = new Vector3(Player.rigidbody.velocity.x, 0, Player.rigidbody.velocity.z).magnitude;

            if (XZmag < HomingAttackSpeed)
            {
                Speed = HomingAttackSpeed;
            }
            else
            {
                Speed = XZmag;
            }

            if (XZmag < AirDashSpeed)
            {
                Aspeed = AirDashSpeed;
            }
            else
            {
                Aspeed = XZmag;
            }
        }
        else
        {
            Aspeed = AirDashSpeed;
            Speed = HomingAttackSpeed;
        }

        //Check if not facing Object
        if (!IsAirDash)
        {
            Vector3 TgyXY = HomingAttackControl.TargetObject.transform.position.normalized;
            TgyXY.y = 0;
            float facingAmmount = Vector3.Dot(Player.PreviousRawInput.normalized, TgyXY);
            //Debug.Log(facingAmmount);
            if (facingAmmount < FacingAmmount)
            {
                IsAirDash = true;
            }
        }
    }

    void Update()
    {
        //Set Animator Parameters
        CharacterAnimator.SetInteger("Action", 1);
        CharacterAnimator.SetFloat("YSpeed", Player.rigidbody.velocity.y);
        CharacterAnimator.SetFloat("GroundSpeed", Player.rigidbody.velocity.magnitude);
        CharacterAnimator.SetBool("Grounded", Player.Grounded);

        //Set Animation Angle
        Vector3 VelocityMod = new Vector3(Player.rigidbody.velocity.x, 0, Player.rigidbody.velocity.z);
        Quaternion CharRot = Quaternion.LookRotation(VelocityMod, transform.up);
        CharacterAnimator.transform.rotation = Quaternion.Lerp(CharacterAnimator.transform.rotation, CharRot,
            Time.deltaTime * skinRotationSpeed);
    }

    void FixedUpdate()
    {
        Timer += 1;

        CharacterAnimator.SetInteger("Action", 1);

        if (IsAirDash)
        {
            if (Player.RawInput != Vector3.zero)
            {
                Player.rigidbody.velocity = transform.TransformDirection(Player.RawInput) * Aspeed;
            }
            else
            {
                Debug.Log("prev");
                Player.rigidbody.velocity = transform.TransformDirection(Player.PreviousRawInput) * Aspeed;
            }
            Timer = HomingTimerLimit + 10;
        }
        else
        {
            direction = Target.position - transform.position;
            Player.rigidbody.velocity = direction.normalized * Speed;
        }

        //End homing attck if on air for too long
        if (Timer > HomingTimerLimit)
        {
            Action.ChangeAction(0);
        }
    }

    public void ResetHomingVariables()
    {
        Timer = 0;
        //IsAirDash = false;
    }
}