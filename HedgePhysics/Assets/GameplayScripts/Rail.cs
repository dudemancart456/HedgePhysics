using UnityEngine;
using System;
using System.Collections.Generic;
using System.Collections;

public class Rail : MonoBehaviour
{

    public List<Vector3> RailPath;
    public List<Vector3> RailRotation;
    public Vector3[] RailArray { get; set; }
    Vector3[] RailRotationArray;

    public Transform WaypointsHolder;
    public PlayerBhysics Player;

    void Start()
    {

        //Get the path

        for (int i = 0; i < WaypointsHolder.childCount; i++)
        {
            RailPath.Add(WaypointsHolder.transform.GetChild(i).position);
        }

        RailArray = RailPath.ToArray();
        //RailRotationArray = RailRotation.ToArray();

    }

    public Vector3 LinearPosition(int seg, float speed)
    {
        Vector3 p1 = RailArray[seg];
        Vector3 p2 = RailArray[seg + 1];

        return Vector3.Lerp(p1, p2, speed);

    }

    public Vector3 LinearRotation(int seg, float speed)
    {
        Vector3 p1 = RailRotationArray[seg];
        Vector3 p2 = RailRotationArray[seg + 1];

        return Vector3.Lerp(p1, p2, speed);
    }

}