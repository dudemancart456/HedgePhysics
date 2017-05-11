using UnityEngine;
using System.Collections;

public class Action03_SpinDash : MonoBehaviour
{
    public Animator CharacterAnimator;
    public Animator BallAnimator;
    CameraControl Cam;
    public float BallAnimationSpeedMultiplier;

    ActionManager Actions;
    PlayerBhysics Player;
    public SonicSoundsControl sounds;
    public SonicEffectsControl effects;
    public float SpinDashChargedEffectAmm;

    public SkinnedMeshRenderer[] PlayerSkin;
    public SkinnedMeshRenderer SpinDashBall;
    public Transform PlayerSkinTransform;


    public float SpinDashChargingSpeed = 0.3f;
    public float MinimunCharge = 10;
    public float MaximunCharge = 100;
    public float SpinDashStillForce = 1.05f;
    float charge;
    bool isSpinDashing;
    Vector3 RawPrevInput;
    Quaternion CharRot;

    public float ReleaseShakeAmmount;

    void Start()
    {
        Player = GetComponent<PlayerBhysics>();
        Actions = GetComponent<ActionManager>();
        Cam = GetComponent<CameraControl>();
    }

    public void InitialEvents()
    {
        sounds.SpinDashSound();
        charge = 0;
    }

    void FixedUpdate()
    {
        charge += SpinDashChargingSpeed;

        //Lock camera on behind
        Cam.Cam.FollowDirection(3, 14f, -10, 0);

        if (Player.RawInput.sqrMagnitude > 0.9f)
        {
            //RawPrevInput = Player.RawInput;
            RawPrevInput = CharacterAnimator.transform.forward;
        }
        else
        {
            //RawPrevInput = Vector3.Scale(PlayerSkinTransform.forward, Player.GroundNormal);
            //RawPrevInput = Player.PreviousRawInput;
            RawPrevInput = CharacterAnimator.transform.forward;
        }

        effects.DoSpindashDust(1, SpinDashChargedEffectAmm * charge);

        Player.rigidbody.velocity /= SpinDashStillForce;

        if (!Input.GetButton("B"))
        {
            Release();
        }

        if (charge > MaximunCharge)
        {
            charge = MaximunCharge;
        }

        //Stop if not grounded
        if (!Player.Grounded)
        {
            Actions.ChangeAction(0);
        }
    }

    void Release()
    {
        HedgeCamera.Shakeforce = (ReleaseShakeAmmount * charge) / 100;
        if (charge < MinimunCharge)
        {
            sounds.Source2.Stop();
            Actions.ChangeAction(0);
        }
        else
        {
            Player.isRolling = true;
            sounds.SpinDashReleaseSound();
            Player.rigidbody.velocity = charge * (RawPrevInput);
            Actions.ChangeAction(0);
        }
    }

    void Update()
    {
        //Set Animator Parameters
        CharacterAnimator.SetInteger("Action", 0);
        CharacterAnimator.SetFloat("YSpeed", Player.rigidbody.velocity.y);
        CharacterAnimator.SetFloat("GroundSpeed", 0);
        CharacterAnimator.SetBool("Grounded", true);
        CharacterAnimator.SetFloat("NormalSpeed", 0);
        BallAnimator.SetFloat("SpinCharge", charge);
        BallAnimator.speed = charge * BallAnimationSpeedMultiplier;

        //Check if rolling
        //if (Player.Grounded && Player.isRolling) { CharacterAnimator.SetInteger("Action", 1); }
        //CharacterAnimator.SetBool("isRolling", Player.isRolling);

        //Rotation

        if (Player.RawInput.sqrMagnitude < 0.9f)
        {
            CharRot = Quaternion.LookRotation(Player.PreviousRawInput, Vector3.up);
        }
        else
        {
            CharRot = Quaternion.LookRotation(Player.rigidbody.velocity, Player.GroundNormal);
        }
        CharacterAnimator.transform.rotation = Quaternion.Lerp(CharacterAnimator.transform.rotation, CharRot,
            Time.deltaTime * Actions.Action00.skinRotationSpeed);


        for (int i = 0; i < PlayerSkin.Length; i++)
        {
            PlayerSkin[i].enabled = false;
        }
        SpinDashBall.enabled = true;
    }

    public void ResetSpinDashVariables()
    {
        for (int i = 0; i < PlayerSkin.Length; i++)
        {
            PlayerSkin[i].enabled = true;
        }
        SpinDashBall.enabled = false;
        charge = 0;
    }
}