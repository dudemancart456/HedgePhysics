using UnityEngine;
using System.Collections;

public class Action00_Regular : MonoBehaviour {

    public Animator CharacterAnimator;
    PlayerBhysics Player;
    ActionManager Actions;
    public SonicSoundsControl sounds;

    public float skinRotationSpeed;
    Action01_Jump JumpAction;
    Quaternion CharRot;

    public float SkiddingStartPoint;
    public float SkiddingIntensity;

    bool hasSked;

    void Awake()
    {
        Player = GetComponent<PlayerBhysics>();
        Actions = GetComponent<ActionManager>();
        JumpAction = GetComponent<Action01_Jump>();
    }

    void FixedUpdate()
    {

        //Jump

        if (Input.GetButton("A") && Player.Grounded)
        {
            JumpAction.InitialEvents();
            Actions.ChangeAction(1);
        }

        //Skidding

        if(Player.b_normalSpeed < -SkiddingStartPoint && Player.Grounded)
        {
            Player.isRolling = false;
            Player.AddVelocity(Player.rigidbody.velocity.normalized * SkiddingIntensity);
            if (!hasSked && Player.Grounded)
            {
                sounds.SkiddingSound();
                hasSked = true;
            }
            if(Player.SpeedMagnitude < 4)
            {
                Player.b_normalSpeed = 0;
                hasSked = false;
            }
        }
        else
        {
            hasSked = false;
        }


        //Set Homing attack to true
        if (Player.Grounded) { Actions.Action02.HomingAvailable = true; }

    }

    void Update()
    {

        //Set Animator Parameters
        if (Player.Grounded) { CharacterAnimator.SetInteger("Action", 0); }
        CharacterAnimator.SetFloat("YSpeed", Player.rigidbody.velocity.y);
        CharacterAnimator.SetFloat("GroundSpeed", Player.rigidbody.velocity.magnitude);
        CharacterAnimator.SetBool("Grounded", Player.Grounded);
        CharacterAnimator.SetFloat("NormalSpeed", Player.b_normalSpeed + SkiddingStartPoint);

        //Do Spindash
        if (Input.GetButton("B") && Player.Grounded) { Actions.ChangeAction(3); Actions.Action03.InitialEvents(); }

        //Check if rolling
        if (Player.Grounded && Player.isRolling) { CharacterAnimator.SetInteger("Action", 1); }
        CharacterAnimator.SetBool("isRolling", Player.isRolling);

        //Play Rolling Sound
        if (Input.GetButtonDown("R1") && Player.Grounded) { sounds.SpinningSound(); }

        //Set Character Animations and position
        CharacterAnimator.transform.parent = null;
        
        //Set Skin Rotation
        if (Player.Grounded)
        {
            Vector3 newForward = Player.rigidbody.velocity - transform.up * Vector3.Dot(Player.rigidbody.velocity, transform.up);

            if (newForward.magnitude < 0.1f)
            {
                newForward = CharacterAnimator.transform.forward;
            }

            CharRot = Quaternion.LookRotation(newForward, transform.up);
            CharacterAnimator.transform.rotation = Quaternion.Lerp(CharacterAnimator.transform.rotation, CharRot, Time.deltaTime * skinRotationSpeed);

           // CharRot = Quaternion.LookRotation( Player.rigidbody.velocity, transform.up);
           // CharacterAnimator.transform.rotation = Quaternion.Lerp(CharacterAnimator.transform.rotation, CharRot, Time.deltaTime * skinRotationSpeed);
        }
        else
        {
            Vector3 VelocityMod = new Vector3(Player.rigidbody.velocity.x, 0, Player.rigidbody.velocity.z);
            Quaternion CharRot = Quaternion.LookRotation(VelocityMod, -Player.Gravity.normalized);
            CharacterAnimator.transform.rotation = Quaternion.Lerp(CharacterAnimator.transform.rotation, CharRot, Time.deltaTime * skinRotationSpeed);
        }

        //Do a homing attack
        if (!Player.Grounded && Input.GetButtonDown("A") && Actions.Action02Control.HasTarget && Actions.Action02.HomingAvailable)
        {
            if (Actions.Action02Control.HomingAvailable) {
                sounds.HomingAttackSound();
                Actions.Action02.IsAirDash = false;
                Actions.ChangeAction(2);
                Actions.Action02.InitialEvents();
                }
        }
        //If no tgt, do air dash;
        if (!Player.Grounded && Input.GetButtonDown("A") && !Actions.Action02Control.HasTarget && Actions.Action02.HomingAvailable)
        {
            if (Actions.Action02Control.HomingAvailable)
            {
                sounds.AirDashSound();
                Actions.Action02.IsAirDash = true;
                Actions.ChangeAction(2);
                Actions.Action02.InitialEvents();
            }
        }

    }

}
