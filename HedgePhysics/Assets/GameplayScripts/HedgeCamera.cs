using UnityEngine;
using System.Collections;

public class HedgeCamera : MonoBehaviour {

    public Transform Target;
    public PlayerBhysics Player;
    public Transform Skin;

    Transform CameraTrans;
    public bool UseAutoRotation;
    public bool UseCurve;
    public float AutoXRotationSpeed;
    public AnimationCurve AutoXRotationCurve;

    public float CameraMaxDistance = -11;
    public float CameraYOffset = 3;

    public float CameraRotationSpeed = 100;
    public float CameraVerticalRotationSpeed = 10;
    public float CameraNormalRotationSpeed = 2;
    public float CameraMoveSpeed = 100;

    public float InputXSpeed = 1;
    public float InputYSpeed = 0.5f;

    public float yMinLimit = -20f;
    public float yMaxLimit = 80f;

    public float LockCamAtHighSpeed = 130;

    float x = 0.0f;
    float y = 20.0f;
    float z = 0.0f;
    Vector3 LerpedNormal;

    float AutoX;
    float CurveX;

    Quaternion LerpedRot;
    Vector3 LerpedPos;

    public float MoveLerpingSpeed;
    public float RotationLerpingSpeed;

    public Transform PlayerPosLerped;

    float moveSpeed = 0;
    float rotSpeed = 0;

    public bool Locked { get; set; }
    public LayerMask NonCollidableLayers;

    void Start()
    {
        Locked = false;
    }

    void LateUpdate () {

        //Copy and Lerp the player's Pos
        if (!Locked)
        {
            PlayerPosLerped.position = Target.position;
            Quaternion newrot = Player.transform.rotation;
            PlayerPosLerped.rotation = Quaternion.Lerp(PlayerPosLerped.rotation, newrot, Time.deltaTime * CameraVerticalRotationSpeed);

            x += (Input.GetAxis("Horizontal_right") * InputXSpeed) * Time.deltaTime;
            y -= (Input.GetAxis("Vertical_right") * InputYSpeed) * Time.deltaTime;
            x += (Input.GetAxis("Mouse X") * InputXSpeed) * Time.deltaTime;
            y -= (Input.GetAxis("Mouse Y") * InputYSpeed) * Time.deltaTime;
        }
        z = 0;

        if (UseAutoRotation && !Locked)
        {
            
            if (!UseCurve)
            {
                float NormalMod = Mathf.Abs(Player.b_normalSpeed - Player.MaxSpeed);
                x += (((Input.GetAxis("Horizontal")) * NormalMod) * AutoXRotationSpeed) * Time.deltaTime;
                ;
                y -= 0;
                z = 0;
            }
            else
            {
                CurveX = AutoXRotationCurve.Evaluate((Player.rigidbody.velocity.sqrMagnitude / Player.MaxSpeed) / Player.MaxSpeed);
                CurveX = CurveX * 100;
                x += (((Input.GetAxis("Horizontal")) * CurveX) * AutoXRotationSpeed) * Time.deltaTime;
                ;
                y -= 0;
                z = 0;
            }
            
        }

        y = ClampAngle(y, yMinLimit, yMaxLimit);

        var rotation = Quaternion.Euler(y, x, 0);
        rotation = PlayerPosLerped.rotation * rotation;

        //Collision

        float dist;
        RaycastHit hit;

        Debug.DrawRay(Target.position, -transform.forward, Color.blue);
        if (Physics.Raycast(Target.position, -transform.forward, out hit, -CameraMaxDistance, NonCollidableLayers))
        {
            dist = (-hit.distance);
        }
        else
        {
            dist = CameraMaxDistance;
        }

        Debug.Log(dist);

        var position = rotation * new Vector3(0, 0, dist + 0.3f) + Target.position;

        LerpedRot = rotation;
        LerpedPos = position;
        
        moveSpeed = Mathf.Lerp(moveSpeed, CameraMoveSpeed, Time.deltaTime * MoveLerpingSpeed);
        rotSpeed = Mathf.Lerp(rotSpeed, CameraRotationSpeed, Time.deltaTime * RotationLerpingSpeed);

        transform.position = Vector3.Lerp(transform.position, LerpedPos, Time.deltaTime * moveSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, LerpedRot, Time.deltaTime * rotSpeed);

        // High Speed Lock

        if (Player.b_normalSpeed > LockCamAtHighSpeed)
        {

            FollowDirection(3, 14, -10);
        }

    }

    public void LagCamera(float move, float rotation)
    {
        moveSpeed = 0;
        rotSpeed = 0;
        MoveLerpingSpeed = move;
        RotationLerpingSpeed = rotation;
    }

    public void FollowDirection(float speed, float height, float distance)
    {
        if (!Locked)
        {

            float dot = Vector3.Dot(Skin.forward, transform.right);
            //Debug.Log(dot);
            x += dot * speed;

            if (y < height)
            {
                y += 0.3f;
            }
            else if (y > height + 0.35f)
            {
                y -= 0.3f;
            }

        }
        //CameraMaxDistance = distance;
    }

    public void ChangeDirection(float move, float rotation, Vector3 Direction)
    {
        transform.LookAt(Target);
        moveSpeed = 0;
        rotSpeed = 0;
        MoveLerpingSpeed = move;
        RotationLerpingSpeed = rotation;
        x = (Direction.x * 90);
        y = (Direction.y * 90);

    }

    public float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;

        return Mathf.Clamp(angle, min, max);
    }
}
