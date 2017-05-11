using UnityEngine;
using System.Collections;

public class PlayerPhysics : MonoBehaviour
{
    public Vector3 XYZSpeed { get; set; }
    public Vector3 XYZFloat { get; set; }
    public Vector3 Gravity;
    public bool Grounded { get; set; }

    public Transform DownRayPosition;
    public Vector3 DownRayOffset { get; set; }
    public Vector3 GroundNormal { get; set; }
    Vector3 GroundPosition;
    Ray DownRay;

    void Start()
    {
        XYZFloat = transform.position;
    }

    void FixedUpdate()
    {
        MoveCharacter();
        SetOffsets();
        DownCast();

        if (Grounded)
        {
            GroundPhysics();
        }
        else
        {
            AirPhysics();
        }
    }

    //=======

    void MoveCharacter()
    {
        XYZFloat += XYZSpeed;
        transform.position = XYZFloat;
    }

    void SetOffsets()
    {
        //DOWN
        DownRayOffset = (transform.position - DownRayPosition.GetChild(0).position);
    }

    void DownCast()
    {
        DownRay.origin = DownRayPosition.position;
        DownRay.direction = -DownRayPosition.up;
        RaycastHit RayHit;

        while (Physics.Raycast(DownRay, out RayHit, 10f))
        {
        }
    }

    void GroundPhysics()
    {
        transform.position = GroundPosition;
        SlopePhysics();
    }

    void AirPhysics()
    {
        //Apply Gravity
        XYZSpeed += Gravity;

        //ResetAngle
        transform.rotation = Quaternion.identity;
    }

    void SlopePhysics()
    {
        transform.rotation = Quaternion.FromToRotation(transform.up, GroundNormal);
    }
}