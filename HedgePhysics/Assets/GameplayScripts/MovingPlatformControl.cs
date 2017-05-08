using UnityEngine;
using System.Collections;

public class MovingPlatformControl : MonoBehaviour {

    public float HorizontalXDistance;
    public float HorizontalXSpeed;
    public float HorizontalZDistance;
    public float HorizontalZSpeed;
    public float VerticalDistance;
    public float VerticalSpeed;

    public Vector3 Moving { get; set; }
    public Vector3 TranslateVector { get; set; }
    public float HorSpeed { get; set; }
    public float VerSpeed { get; set; }

    Vector3 InitialPosition;

    Rigidbody rigidbody;

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        InitialPosition = transform.position;
    }

    void FixedUpdate () {

        float hx1 = Moving.x;
        float hz1 = Moving.z;
        float v1 = Moving.y;

        //rigidbody.MovePosition(InitialPosition + new Vector3(Mathf.Sin(Time.time * HorizontalSpeed) * HorizontalDistance, Mathf.Sin(Time.time * VerticalSpeed) * VerticalDistance, 0.0f));
        Moving = (InitialPosition + new Vector3(Mathf.Sin(Time.time * HorizontalXSpeed) * HorizontalXDistance, Mathf.Sin(Time.time * VerticalSpeed) * VerticalDistance, Mathf.Sin(Time.time * HorizontalZSpeed) * HorizontalZDistance));
        transform.position = Moving;

        float hx2 = Moving.x;
        float hz2 = Moving.z;
        float v2 = Moving.y;

        TranslateVector = new Vector3(hx1 - hx2, v1 - v2, hz1 - hz2);
    }
}
