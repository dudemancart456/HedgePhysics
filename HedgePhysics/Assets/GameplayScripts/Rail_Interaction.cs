using UnityEngine;
using System.Collections;

public class Rail_Interaction : MonoBehaviour
{
    public Rail rail { get; set; }
    PlayerBhysics Player;
    ActionManager Actions;
    public Animator CharacterAnimator;

    public int currentSeg { get; set; }
    float transition;
    bool isCompleted;
    Quaternion CharRot;
    int railActiveCount;

    void Awake()
    {
        Actions = GetComponent<ActionManager>();
        Player = GetComponent<PlayerBhysics>();
    }

    void FixedUpdate()
    {
        railActiveCount += 1;

        if (rail != null)
        {
            OnRail(Player.SpeedMagnitude / 100);
            for (float i = 0; i < Player.SpeedMagnitude; i++)
            {
                //OnRail(Player.SpeedMagnitude/100);
            }
            Vector3 resety = Player.rigidbody.velocity;
            resety.y = 0;
            Player.rigidbody.velocity = resety;

            //Get Out of rail
            if (Actions.Action == 1)
            {
                rail = null;
            }
        }
    }

    void Update()
    {
        if (rail != null)
        {
            if (Input.GetButton("A") && railActiveCount > 10)
            {
                Actions.Action01.InitialEvents();
                Actions.ChangeAction(1);
                railActiveCount = 0;
            }
        }
    }

    void OnRail(float speed)
    {
        //Get out when over

        if (currentSeg >= (rail.RailArray.Length - 1))
        {
            rail = null;
            return;
        }
        else if (currentSeg <= 0)
        {
            rail = null;
            return;
        }


        transition += speed;

        if (transition > 1)
        {
            transition = 0;
            currentSeg++;
        }
        else if (transition < 0)
        {
            transition = 1;
            currentSeg--;
        }

        transform.position = rail.LinearPosition(currentSeg, transition);
    }

    public void OnCollisionEnter(Collision col)
    {
        if (col.gameObject.tag == "Rail")
        {
            //Debug.Log("Col");
            if (col.gameObject.transform.parent.GetComponent<Rail>() != null)
            {
                if (railActiveCount > 20 && rail == null)
                {
                    rail = col.gameObject.transform.parent.GetComponent<Rail>();
                    currentSeg = GetClosestPos(col.gameObject.transform.parent.GetComponent<Rail>().RailArray,
                        transform.position);
                    Actions.ChangeAction(5);
                    railActiveCount = 0;
                }
            }
        }
    }

    public int GetClosestPos(Vector3[] pos, Vector3 playerPos)
    {
        int seg = 0;
        Vector3 tMin = Vector3.zero;
        float minDist = Mathf.Infinity;
        Vector3 currentPos = playerPos;
        for (int i = 0; i < pos.Length; i++)
        {
            float dist = Vector3.Distance(pos[i], currentPos);
            if (dist < minDist)
            {
                seg = i;
                minDist = dist;
            }
        }
        return seg;
    }
}