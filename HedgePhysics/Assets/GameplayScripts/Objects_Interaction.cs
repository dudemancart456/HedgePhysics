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

    [Header("Enemies")]

    public float BouncingPower;
    public bool StopOnHommingAttackHit;
    public bool StopOnHit;
    bool updateTargets;

    [Header("UI objects")]

    public Text RingsCounter;

    public int RingAmmount { get; set; }

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

    }

	public void OnTriggerEnter(Collider col)
    {
        //Speed Pads Collision
        if(col.tag == "SpeedPad")
        {
            if(col.GetComponent<SpeedPadData>() != null)
            {
                if (col.GetComponent<SpeedPadData>().LockToDirection)
                {
                    Player.rigidbody.velocity = Vector3.zero;
                    Player.AddVelocity(col.transform.up * col.GetComponent<SpeedPadData>().Speed);
                }
                else
                {
                    Player.AddVelocity(col.transform.forward * col.GetComponent<SpeedPadData>().Speed);
                }

                if (col.GetComponent<SpeedPadData>().LockControl)
                {
                    Inp.LockInputForAWhile(col.GetComponent<SpeedPadData>().LockControlTime, true);
                }
                Cam.LagCamera(0.5f, 0.5f);
                col.GetComponent<AudioSource>().Play();

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

        //Enemies
        if (col.tag == "Enemy")
        {
            //If 1, destroy, if not, take damage.
            if(Actions.Action == 3)
            {
                col.transform.parent.GetComponent<EnemyHealth>().DealDamage(1);
                updateTargets = true;
            }
            if (Actions.Action00.CharacterAnimator.GetInteger("Action") == 1)
            {
                
                if (col.transform.parent.GetComponent<EnemyHealth>() != null)
                {
                    if (Player.rigidbody.velocity.y < 0 && !Player.isRolling)
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
            else
            {
                if (!Actions.Action04Control.IsHurt)
                {
                    if (RingAmmount > 0)
                    {
                        //LoseRings
                        Sounds.RingLossSound();
                        Actions.Action04Control.GetHurt();
                        Actions.ChangeAction(4);
                        Actions.Action04.InitialEvents();
                    }
                    else
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

}
