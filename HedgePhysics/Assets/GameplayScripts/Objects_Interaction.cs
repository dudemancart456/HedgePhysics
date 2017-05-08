using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Objects_Interaction : MonoBehaviour {

    [Header("For Rings, Srpings and so on")]

    public PlayerBhysics Player;
    public HedgeCamera Cam;
    public SonicSoundsControl Sounds;
    public ActionManager Actions;
    public PlayerBinput Inp;
    public SonicSoundsControl sounds;
    Spring_Proprieties spring;
    int springAmm;


    public GameObject RingCollectParticle;
    public Material SpeedPadTrack;
    public Material DashRingMaterial;

    [Header("Enemies")]

    public float BouncingPower;
    public bool StopOnHommingAttackHit;
    public bool StopOnHit;
    public bool updateTargets { get; set; }

    public float EnemyDamageShakeAmmount;
    public float EnemyHitShakeAmmount;

    [Header("UI objects")]

    public Text RingsCounter;

    public static int RingAmmount { get; set; }

    MovingPlatformControl Platform;
    Vector3 TranslateOnPlatform;
    public Color DashRingLightsColor;

    void Update()
    {
        RingsCounter.text = ": " + RingAmmount;
        if (updateTargets)
        {
            HomingAttackControl.UpdateHomingTargets();
            Actions.Action02.HomingAvailable = true;
            updateTargets = false;
        }

        //Set speed pad trackpad's offset
        SpeedPadTrack.SetTextureOffset("_MainTex", new Vector2(0, -Time.time) * 3);
        DashRingMaterial.SetColor("_EmissionColor", (Mathf.Sin(Time.time * 15) * 1.3f) * DashRingLightsColor);
    }

    void FixedUpdate()
    {
        if(Platform != null)
        {
            transform.position += (-Platform.TranslateVector);
        }
        if (!Player.Grounded)
        {
            Platform = null;
        }
    }

	public void OnTriggerEnter(Collider col)
    {
        //Speed Pads Collision
        if(col.tag == "SpeedPad")
        {
            if(col.GetComponent<SpeedPadData>() != null)
            {
                transform.rotation = Quaternion.identity;
                //ResetPlayerRotation

                if (col.GetComponent<SpeedPadData>().LockToDirection)
                {
                    Player.rigidbody.velocity = Vector3.zero;
                    Player.AddVelocity(col.transform.forward * col.GetComponent<SpeedPadData>().Speed);
                }
                else
                {
                    Player.AddVelocity(col.transform.forward * col.GetComponent<SpeedPadData>().Speed);
                }
                if (col.GetComponent<SpeedPadData>().Snap)
                {
                    transform.position = col.transform.position;
                }
                if (col.GetComponent<SpeedPadData>().isDashRing)
                {
                    Actions.ChangeAction(0);
                    Actions.Action00.CharacterAnimator.SetBool("Grounded", false);
                    Actions.Action00.CharacterAnimator.SetInteger("Action", 0);
                }

                if (col.GetComponent<SpeedPadData>().LockControl)
                {
                    Inp.LockInputForAWhile(col.GetComponent<SpeedPadData>().LockControlTime, true);
                }
                if (col.GetComponent<SpeedPadData>().AffectCamera)
                {
                    Vector3 dir = col.transform.forward;
                    Cam.SetCamera(dir, 2.5f, 20, 5f, 1);
                    col.GetComponent<AudioSource>().Play();
                }

            }
        }

        //Rings Collision
        if (col.tag == "Ring")
        {
            RingAmmount += 1;
            Instantiate(RingCollectParticle, col.transform.position, Quaternion.identity);
            Destroy(col.gameObject);
        }
        if (col.tag == "MovingRing")
        {
            if (col.GetComponent<MovingRing>() != null)
            {
                if (col.GetComponent<MovingRing>().colectable)
                {
                    RingAmmount += 1;
                    Instantiate(RingCollectParticle, col.transform.position, Quaternion.identity);
                    Destroy(col.gameObject);
                }
            }
        }

        //Hazard
        if(col.tag == "Hazard")
        {
            DamagePlayer();
            HedgeCamera.Shakeforce = EnemyDamageShakeAmmount;
        }

        //Enemies
        if (col.tag == "Enemy")
        {
            HedgeCamera.Shakeforce = EnemyHitShakeAmmount;
            //If 1, destroy, if not, take damage.
            if (Actions.Action == 3)
            {
                col.transform.parent.GetComponent<EnemyHealth>().DealDamage(1);
                updateTargets = true;
            }
            if (Actions.Action00.CharacterAnimator.GetInteger("Action") == 1)
            {
                
                if (col.transform.parent.GetComponent<EnemyHealth>() != null)
                {
                    if (!Player.isRolling)
                    {
                        Vector3 newSpeed = new Vector3(1, 0, 1);

                        if (StopOnHommingAttackHit && Actions.Action == 2)
                        {
                            newSpeed = new Vector3(0, 0, 0);
                            newSpeed = Vector3.Scale(Player.rigidbody.velocity, newSpeed);
                            newSpeed.y = BouncingPower;
                        }
                        else if(StopOnHit)
                        {
                            newSpeed = new Vector3(0, 0, 0);
                            newSpeed = Vector3.Scale(Player.rigidbody.velocity, newSpeed);
                            newSpeed.y = BouncingPower;
                        }
                        else
                        {
                            newSpeed = Vector3.Scale(Player.rigidbody.velocity, newSpeed);
                            newSpeed.y = BouncingPower;
                        }


                        Player.rigidbody.velocity = newSpeed;
                    }
                    col.transform.parent.GetComponent<EnemyHealth>().DealDamage(1);
                    updateTargets = true;
                    Actions.ChangeAction(0);
                    
                }
            }
            else if(Actions.Action != 3)
            {
                DamagePlayer();
            }
        }

        //Spring Collision

        if (col.tag == "Spring")
        {
            if (col.GetComponent<Spring_Proprieties>() != null)
            {
                spring = col.GetComponent<Spring_Proprieties>();
                if (spring.IsAdditive)
                {
                    transform.position = col.transform.GetChild(0).position;
                    if (col.GetComponent<AudioSource>()) { col.GetComponent<AudioSource>().Play(); }
                    Actions.Action00.CharacterAnimator.SetInteger("Action", 0);
                    Actions.Action02.HomingAvailable = true;
                    Player.rigidbody.velocity += (spring.transform.up * spring.SpringForce);
                    Actions.ChangeAction(0);
                    spring.anim.SetTrigger("Hit");
                }
                else
                {
                    transform.position = col.transform.GetChild(0).position;
                    if (col.GetComponent<AudioSource>()) { col.GetComponent<AudioSource>().Play(); }
                    Actions.Action00.CharacterAnimator.SetInteger("Action", 0);
                    Actions.Action02.HomingAvailable = true;
                    Player.rigidbody.velocity = spring.transform.up * spring.SpringForce;
                    Actions.ChangeAction(0);
                    spring.anim.SetTrigger("Hit");
                }

                if (col.GetComponent<Spring_Proprieties>().LockControl)
                {
                    Inp.LockInputForAWhile(col.GetComponent<Spring_Proprieties>().LockTime, false);
                }
            }
        }

    }

    public void OnTriggerStay(Collider col)
    {
        //Hazard
        if (col.tag == "Hazard")
        {
            DamagePlayer();
        }

        if (col.gameObject.tag == "MovingPlatform")
        {
            Platform = col.gameObject.GetComponent<MovingPlatformControl>();
        }
        else
        {
            Platform = null;
        }
    }

    public void DamagePlayer()
    {
        if (!Actions.Action04Control.IsHurt && Actions.Action != 4)
        {

            if (!Monitors_Interactions.HasShield)
            {
                if (RingAmmount > 0)
                {
                    //LoseRings
                    Sounds.RingLossSound();
                    Actions.Action04Control.GetHurt();
                    Actions.ChangeAction(4);
                    Actions.Action04.InitialEvents();
                }
                if (RingAmmount <= 0)
                {
                    //Die
                    if (!Actions.Action04Control.isDead)
                    {
                        Sounds.DieSound();
                        Actions.Action04Control.isDead = true;
                        Actions.ChangeAction(4);
                        Actions.Action04.InitialEvents();
                    }
                }
            }
            if (Monitors_Interactions.HasShield)
            {
                //Lose Shield
                Actions.Action04.sounds.SpikedSound();
                Monitors_Interactions.HasShield = false;
                Actions.ChangeAction(4);
                Actions.Action04.InitialEvents();
            }
        }
    }
}
