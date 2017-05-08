using UnityEngine;
using System.Collections;

public class AngleTest : MonoBehaviour {

    public Vector3 Speed;
    public Vector3 AngleSined;
    public Vector3 ModifiedQuart;
    public Vector3 ForcedNormal;

    public Vector3 AngleToAngle;
    public float Angle;
    Rigidbody rigid;

    public float divideAngle;



    void Start()
    {
        rigid = GetComponent<Rigidbody>();
    }

	void FixedUpdate () {

        ModifiedQuart.x = transform.rotation.x + 1;
        ModifiedQuart.y = transform.rotation.y;
        ModifiedQuart.z = transform.rotation.z + 1;

        ForcedNormal.x = transform.forward.x;
        ForcedNormal.y = transform.forward.y;
        ForcedNormal.z = transform.right.z;

        //AngleSined.x = Mathf.Cos(transform.eulerAngles.x / divideAngle);
        //AngleSined.y = Mathf.Sin(transform.eulerAngles.y / divideAngle);
        //AngleSined.z = Mathf.Cos(transform.eulerAngles.z / divideAngle);

        AngleSined.x = Mathf.Cos(transform.rotation.x / 1);
        AngleSined.y = Mathf.Sin(transform.rotation.y / 1);
        AngleSined.z = Mathf.Cos(transform.rotation.z / 1);

        transform.rotation.ToAngleAxis(out Angle, out AngleToAngle);

        rigid.velocity = Speed;

	
	}
}
