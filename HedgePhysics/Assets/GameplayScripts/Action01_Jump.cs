using UnityEngine;
using System.Collections;

public class Action01_Jump : MonoBehaviour
{
    public Animator CharacterAnimator;
    PlayerBhysics Player;
    ActionManager Actions;
    public SonicSoundsControl sounds;

    public float skinRotationSpeed;

    public Vector3 InitialNormal { get; set; }
    public float Counter { get; set; }
    public float JumpDuration;
    public float SlopedJumpDuration;
    public float JumpSpeed;
    public float JumpSlopeConversion;
    public float StopYSpeedOnRelease;
    public float RollingLandingBoost;

    float jumpSlopeSpeed;

    void Awake()
    {
        Player = GetComponent<PlayerBhysics>();
        Actions = GetComponent<ActionManager>();
    }

    public void InitialEvents()
    {
        //Set Initial Variables
        Counter = 0;
        jumpSlopeSpeed = 0;
        InitialNormal = Player.GroundNormal;

        //SnapOutOfGround to make sure you do jump
        transform.position += (InitialNormal * 0.3f);

        //Jump higher depending on the speed and the slope you're in
        if (Player.rigidbody.velocity.y > 0 && Player.GroundNormal.y > 0)
        {
            jumpSlopeSpeed = Player.rigidbody.velocity.y * JumpSlopeConversion;
        }
        //Sound
        sounds.JumpSound();
    }

    void Update()
    {
        //Set Animator Parameters
        CharacterAnimator.SetInteger("Action", 1);
        CharacterAnimator.SetFloat("YSpeed", Player.rigidbody.velocity.y);
        CharacterAnimator.SetFloat("GroundSpeed", Player.rigidbody.velocity.magnitude);
        CharacterAnimator.SetBool("Grounded", Player.Grounded);
        CharacterAnimator.SetBool("isRolling", false);

        //Set Animation Angle
        Vector3 VelocityMod = new Vector3(Player.rigidbody.velocity.x, 0, Player.rigidbody.velocity.z);
        Quaternion CharRot = Quaternion.LookRotation(VelocityMod, transform.up);
        CharacterAnimator.transform.rotation = Quaternion.Lerp(CharacterAnimator.transform.rotation, CharRot,
            Time.deltaTime * skinRotationSpeed);

        Actions.Action02.HomingAvailable = true;

        //Do a homing attack
        if (Counter > 0.08f && Input.GetButtonDown("A") && Actions.Action02Control.HasTarget &&
            Actions.Action02.HomingAvailable)
        {
            if (Actions.Action02Control.HomingAvailable)
            {
                sounds.HomingAttackSound();
                Actions.Action02.IsAirDash = false;
                Actions.ChangeAction(2);
                Actions.Action02.InitialEvents();
            }
        }
        //If no tgt, do air dash;
        if (Counter > 0.08f && Input.GetButtonDown("A") && !Actions.Action02Control.HasTarget &&
            Actions.Action02.HomingAvailable)
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

    void FixedUpdate()
    {
        //Jump action
        Counter += Time.deltaTime;

        if (!Input.GetButton("A") && Counter < JumpDuration)
        {
            Counter = JumpDuration;
        }

        //Keep Colliders Rotation to avoid collision Issues
        if (Counter < 0.2f)
        {
            //transform.rotation = Quaternion.FromToRotation(transform.up, InitialNormal) * transform.rotation;
        }

        //Add Jump Speed
        if (Counter < JumpDuration)
        {
            Player.isRolling = false;
            if (Counter < SlopedJumpDuration)
            {
                Player.AddVelocity(InitialNormal * (JumpSpeed));
            }
            else
            {
                Player.AddVelocity(new Vector3(0, 1, 0) * (JumpSpeed));
            }
            //Extra speed
            Player.AddVelocity(new Vector3(0, 1, 0) * (jumpSlopeSpeed));
        }

        //Cancell Jump
        if (Player.rigidbody.velocity.y > 0 && !Input.GetButton("A"))
        {
            Vector3 Velocity = new Vector3(Player.rigidbody.velocity.x, Player.rigidbody.velocity.y,
                Player.rigidbody.velocity.z);
            Velocity.y = Velocity.y - StopYSpeedOnRelease;
            Player.rigidbody.velocity = Velocity;
        }


        //End Action
        if (Player.Grounded && Counter > SlopedJumpDuration)
        {
            Actions.ChangeAction(0);
        }
    }
}