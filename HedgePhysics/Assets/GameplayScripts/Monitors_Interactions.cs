using UnityEngine;
using System.Collections;

public class Monitors_Interactions : MonoBehaviour
{
    Objects_Interaction Objects;
    PlayerBhysics Player;
    ActionManager Actions;

    public GameObject RingGiver;
    public GameObject ShieldGiver;
    int Once = 0;

    public GameObject ShieldObject;
    public Material NormalShieldMaterial;
    public Vector3 ShieldOffset;
    public static bool HasShield = false;
    bool updateTgts;

    bool firstTime = false;

    void Start()
    {
        Objects = GetComponent<Objects_Interaction>();
        Player = GetComponent<PlayerBhysics>();
        Actions = GetComponent<ActionManager>();
    }

    void FixedUpdate()
    {
        Once = 0;
    }

    void Update()
    {
        if (!firstTime)
        {
            ShieldObject.SetActive(false);
            firstTime = true;
        }

        if (HasShield)
        {
            ShieldObject.SetActive(true);
            ShieldObject.transform.position = transform.position + ShieldOffset;
            ShieldObject.transform.rotation = transform.rotation;
        }
        else
        {
            if (ShieldObject)
            {
                ShieldObject.SetActive(false);
            }
        }

        if (updateTgts)
        {
            HomingAttackControl.UpdateHomingTargets();
            updateTgts = false;
        }

        NormalShieldMaterial.SetTextureOffset("_MainTex", new Vector2(0, -Time.time) * 3);
    }

    public void OnTriggerEnter(Collider col)
    {
        if (col.tag == "Monitor")
        {
            Once -= 1;
            if (col.GetComponent<MonitorData>() != null)
            {
                if (!Player.Grounded)
                {
                    Vector3 newSpeed = new Vector3(1, 0, 1);

                    if (Objects.StopOnHommingAttackHit && Actions.Action == 2)
                    {
                        newSpeed = new Vector3(0, 0, 0);
                        newSpeed = Vector3.Scale(Player.rigidbody.velocity, newSpeed);
                        newSpeed.y = Objects.BouncingPower;
                    }
                    else if (Objects.StopOnHit)
                    {
                        newSpeed = new Vector3(0, 0, 0);
                        newSpeed = Vector3.Scale(Player.rigidbody.velocity, newSpeed);
                        newSpeed.y = Objects.BouncingPower;
                    }
                    else
                    {
                        newSpeed = Vector3.Scale(Player.rigidbody.velocity, newSpeed);
                        newSpeed.y = Objects.BouncingPower;
                    }

                    Objects.updateTargets = true;
                    Actions.ChangeAction(0);
                    Player.rigidbody.velocity = newSpeed;
                }

                //Monitors data
                if (col.GetComponent<MonitorData>().Type == MonitorType.Ring)
                {
                    if (Once == -1)
                    {
                        GameObject clone = (GameObject) Instantiate(RingGiver, transform.position, transform.rotation);
                        clone.GetComponent<RingGiverControl>().Rings = col.GetComponent<MonitorData>().RingAmmount;
                        col.GetComponent<MonitorData>().DestroyMonitor();
                        updateTgts = true;
                    }
                }
                else if (col.GetComponent<MonitorData>().Type == MonitorType.Shield)
                {
                    if (Once == -1)
                    {
                        GameObject clone = (GameObject) Instantiate(ShieldGiver, transform.position, transform.rotation);
                        col.GetComponent<MonitorData>().DestroyMonitor();
                        updateTgts = true;
                    }
                }
            }
        }
    }
}