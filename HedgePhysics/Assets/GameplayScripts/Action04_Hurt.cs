using UnityEngine;
using System.Collections;

public class Action04_Hurt : MonoBehaviour {

    public Animator CharacterAnimator;
    PlayerBhysics Player;
    ActionManager Actions;
    public SonicSoundsControl sounds;

    public float KnockbackUpwardsForce = 10;
    int counter;
    public float deadCounter { get; set; }

    public bool ResetSpeedOnHit = false;
    public float KnockbackForce = 10;

    void Awake()
    {
        Player = GetComponent<PlayerBhysics>();
        Actions = GetComponent<ActionManager>();
    }

    public void InitialEvents()
    {
        //Change Velocity
        if (!ResetSpeedOnHit)
        {
            Vector3 newSpeed = new Vector3((Player.rigidbody.velocity.x / 2), KnockbackUpwardsForce, (Player.rigidbody.velocity.z / 2));
            newSpeed.y = KnockbackUpwardsForce;
            Player.rigidbody.velocity = newSpeed;
        }
        else
        {
            Vector3 newSpeed = -transform.forward * KnockbackForce;
            newSpeed.y = KnockbackUpwardsForce;
            Player.rigidbody.velocity = newSpeed;
        }

    }

    void FixedUpdate () {

        //Get out of Action
        counter += 1;

        if (Player.Grounded && counter > 20)
        {
            if (!Actions.Action04Control.isDead)
            { 
                Actions.ChangeAction(0);
                Debug.Log("What");
                counter = 0;
            }
        }

    }

    void Update()
    {

        //Set Animator Parameters
        CharacterAnimator.SetInteger("Action", 4);

        //Dead
        if (Actions.Action04Control.isDead)
        {
            deadCounter += Time.deltaTime;
            if (Player.Grounded && deadCounter > 0.3f)
            {
                CharacterAnimator.SetBool("Dead", true);
            }
        }

    }
}
