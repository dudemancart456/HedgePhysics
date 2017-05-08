using UnityEngine;
using System.Collections;

[AddComponentMenu("Camera-Control/Mouse Orbit with zoom")]
public class MouseOrbitImproved : MonoBehaviour
{

    public Transform target;
    public float distance = 5.0f;
    public float xSpeed = 120.0f;
    public float ySpeed = 120.0f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public PlayerBhysics Player;


    public float x = 0.0f;
    public float y = 0.0f;
    public float z = 0.0f;

    // Use this for initialization
    void Start()
    {
        Vector3 angles = transform.eulerAngles;
        x = angles.y;
        y = angles.x;

    }

    void LateUpdate()
    {
        if (target)
        {

            x += Input.GetAxis("Horizontal_right") * xSpeed * 0.01f;
            y -= Input.GetAxis("Vertical_right") * ySpeed * 0.005f;
            z = 0;

            y = ClampAngle(y, yMinLimit, yMaxLimit);

            Quaternion rotation = Quaternion.Euler(y, x, 0);
            rotation = Quaternion.FromToRotation(Vector3.up, target.up) * rotation;

            Vector3 negDistance = new Vector3(0.0f, 0.0f, -distance);
            Vector3 position = (rotation) * negDistance + target.position;

            transform.rotation = rotation;
            transform.position = position;
        }
    }

    public static float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;
        return Mathf.Clamp(angle, min, max);
    }
}