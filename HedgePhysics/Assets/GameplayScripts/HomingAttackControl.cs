using UnityEngine;
using System.Collections;

public class HomingAttackControl : MonoBehaviour {

    public bool HasTarget { get; set; }
    public static GameObject TargetObject { get; set; }
    public ActionManager Actions;

    public float TargetSearchDistance = 10;
    public Transform Icon;
    public float IconScale;

    public static GameObject[] Targets;
    public GameObject[] TgtDebug;

    public Transform MainCamera;
    public float IconDistanceScaling;

    int HomingCount;
    public bool HomingAvailable { get; set; }

    void Awake()
    {
        Actions = GetComponent<ActionManager>();
    }

    void Start()
    {
        var tgt = GameObject.FindGameObjectsWithTag("HomingTarget");
        Targets = tgt;
        TgtDebug = tgt;

        Icon.parent = null;
        UpdateHomingTargets();
    }

    void FixedUpdate()
    {
        //Prevent Homing attack spamming

        HomingCount += 1;

        if(Actions.Action == 2)
        {
            HomingAvailable = false;
            HomingCount = 0;
        }
        if(HomingCount > 3)
        {
            HomingAvailable = true;
        }

        //SetIconPosition

        TargetObject = GetClosestTarget(Targets, TargetSearchDistance);

        if (HasTarget)
        {
            Icon.position = TargetObject.transform.position;
            float camDist = Vector3.Distance(transform.position, MainCamera.position);
            Icon.localScale = (Vector3.one * IconScale) + (Vector3.one * (camDist * IconDistanceScaling));
        }
        else
        {
            Icon.localScale = Vector3.zero;
        }

    }

    //This function will look for every possible homing attack target in the whole level. 
    //And you can call it from other scritps
    public static void UpdateHomingTargets()
    {
        var tgt = GameObject.FindGameObjectsWithTag("HomingTarget");
        Targets = tgt;
    }

    GameObject GetClosestTarget(GameObject[] tgts, float maxDistance)
    {
        HasTarget = false;
        GameObject[] gos = tgts;
        GameObject closest = null;
        float distance = maxDistance;
        Vector3 position = transform.position;
        foreach (GameObject go in gos)
        {
            Vector3 diff = go.transform.position - position;
            float curDistance = diff.sqrMagnitude;
            
            if (curDistance < distance)
            {
                HasTarget = true;
                closest = go;
                distance = curDistance;
            }
        }
        //Debug.Log(closest);
        return closest;
    }


}
