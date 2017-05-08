using UnityEngine;
using System.Collections;

public class MotobugControl : MonoBehaviour {

    EnemyBhysics Physics;
    public Animator Anim;
    public ParticleSystem DustParticles;

    public int Action { get; set; }
    Transform Player;
    
    public Vector3 Target { get; set; }
    Vector3 InitialPosition;

    [Header("Wander Variables")]

    public float WanderDistance;
    public float WanterStillAmmount;
    public float WanderMoveAmmount;
    public float WanderCalmness;
    float MoveTime;
    float RandomTime;
    float distanceToPlayer;

    [Header("Attack Variables")]

    public float PlayerNoticeDistance;
    public float PlayerLoseDistance;
    public float NoticeReactionTime;
    public float skinRotationSpeed;

    void Start () {

        Player = GameObject.FindGameObjectWithTag("Player").transform;
        Physics = GetComponent<EnemyBhysics>();
        InitialPosition = transform.position;
	
	}
	
	void FixedUpdate () {

        switch (Action)
        {
            case 0:
                Action_00_Wander();
                break;
            case 1:
                Action_01_Notice();
                break;
            case 2:
                Action_02_Run();
                break;
            default:
                break;

        }

        //Debug.Log(distanceToPlayer);
	}

    void Action_00_Wander()
    {
        MoveTime += 1;
        RandomTime += 1;

        Anim.SetFloat("GroundSpeed", Physics.b_normalSpeed);

        if (MoveTime > 0)
        {
            var dir = Target - transform.position;
            Physics.HandleGroundControl(1, dir.normalized);
            dir.y = 0;
            Quaternion CharRot = Quaternion.LookRotation(dir, transform.up);
            Anim.transform.rotation = Quaternion.Lerp(Anim.transform.rotation, CharRot, Time.deltaTime * skinRotationSpeed);
        }
        if (MoveTime > WanderMoveAmmount)
        {
            MoveTime = -WanterStillAmmount;
        }

        //ChangeTgt
        if (RandomTime > WanderCalmness)
        {
            Vector3 wander = new Vector3(Random.Range(-WanderDistance, WanderDistance), transform.position.y, Random.Range(-WanderDistance, WanderDistance));
            wander = Vector3.ClampMagnitude(wander, WanderDistance);
            Target = InitialPosition + wander;
            RandomTime = 0;
        }

        //Look for player
        distanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);
        if (distanceToPlayer < PlayerNoticeDistance)
        {
            ChangeAction(1);
            Anim.SetInteger("Action", 1);
        }
    }

    void Action_01_Notice()
    {
        MoveTime += 1;

        Target = Player.position;
        var dir = Target - transform.position;
        dir.y = 0;
        Quaternion CharRot = Quaternion.LookRotation(dir, transform.up);
        Anim.transform.rotation = Quaternion.Lerp(Anim.transform.rotation, CharRot, Time.deltaTime * 10);


        if (MoveTime > NoticeReactionTime)
        {
            ChangeAction(2);
            Anim.SetInteger("Action", 0);
        }
    }

    void Action_02_Run()
    {
        Anim.SetFloat("GroundSpeed", Physics.b_normalSpeed);

        DustParticles.Emit(1);
        Target = Player.position;
        var dir = (Target - transform.position);
        Physics.HandleGroundControl(1, dir.normalized);
        dir.y = 0;
        Quaternion CharRot = Quaternion.LookRotation(dir, transform.up);
        Anim.transform.rotation = Quaternion.Lerp(Anim.transform.rotation, CharRot, Time.deltaTime * skinRotationSpeed);

        if (MoveTime > NoticeReactionTime)
        {
            ChangeAction(2);
            Anim.SetInteger("Action", 0);
        }

        //Check if too far away
        distanceToPlayer = Vector3.Distance(Player.transform.position, transform.position);
        if (distanceToPlayer > PlayerLoseDistance)
        {
            ChangeAction(0);
        }
    }


    public void ChangeAction(int action)
    {
        //Deactivate all nescesarry variables here
        MoveTime = 0;
        RandomTime = 0;
        Action = action;
    }
}
