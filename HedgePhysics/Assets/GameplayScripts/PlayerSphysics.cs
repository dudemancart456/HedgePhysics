using UnityEngine;
using System.Collections;

public class PlayerSphysics : MonoBehaviour
{
    public Vector3 XYZSpeed { get; set; }
    public Vector3 XYZFloat;
    public Vector3 Gravity;
    public bool Grounded { get; set; }
    public Vector3 GroundNormal { get; set; }

    public float SensorsRadius;
    public Transform DownSensorPos;
    Vector3 GroundPosition;
    Ray DownRay;

    void Start()
    {
        XYZFloat = transform.position;
    }

    void FixedUpdate()
    {
        MoveCharacter();
        DownSensor();
    }

    //=======

    void MoveCharacter()
    {
        if (Grounded)
        {
            GroundPhysics();
        }
        else
        {
            AirPhysics();
        }
    }

    void DownSensor()
    {
        Collider[] hit = Physics.OverlapSphere(DownSensorPos.position, SensorsRadius);

        Grounded = false;
        while (hit.Length > 0)
        {
            Grounded = true;
            XYZSpeed = Vector3.zero;
            XYZFloat += new Vector3(0, 0.003f, 0);
            MoveCharacter();
            Collider[] hitInside = Physics.OverlapSphere(DownSensorPos.position, SensorsRadius);
            if (hitInside.Length <= 0)
            {
                XYZFloat -= new Vector3(0, 0.003f, 0);
                break;
            }
        }
    }

    void GroundPhysics()
    {
        DownRay.origin = DownSensorPos.position;
        DownRay.direction = -DownSensorPos.up;
        RaycastHit RayHit;
        if (Physics.Raycast(DownRay, out RayHit, 20f))
        {
            GroundNormal = RayHit.normal;
        }
        transform.rotation = Quaternion.FromToRotation(Vector3.up, GroundNormal);

        SlopePhysics();
    }

    void SlopePhysics()
    {
        //Move
        XYZFloat += XYZSpeed;
        transform.position = Vector3.Scale(XYZFloat, GroundNormal);
    }

    void AirPhysics()
    {
        //Apply Gravity
        XYZSpeed += Gravity;

        //ResetAngle
        GroundNormal = new Vector3(1, 1, 1);
        transform.rotation = Quaternion.identity;

        //Move
        GroundNormal = new Vector3(1, 1, 1);
        XYZFloat += XYZSpeed;
        transform.position = XYZFloat;
    }


    //GIZMO
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(DownSensorPos.position, SensorsRadius);
        Gizmos.DrawRay(DownRay);
    }
}