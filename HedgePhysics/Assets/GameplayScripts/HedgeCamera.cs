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
    public bool LockHeight;
    public float LockHeightSpeed;
    public bool MoveHeightBasedOnSpeed;
    public float HeightToLock;
    public float HeightFollowSpeed;
    public float FallSpeedThreshold;

    public float CameraMaxDistance = -11;
    public float CameraYOffset = 3;
    public float AngleThreshold;

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
    public LayerMask CollidableLayers;
    Vector3 lookdir;
    float heighttolook;
    float lookTimer;
    float lookspeed;

    public float LockedRotationSpeed;

    //Cached variables
    Quaternion rotation;
    float InitialLockedRotationSpeed;
    float InitialRotationSpeed;
    float InitialMoveSpeed;
    float newX;
    float neoX;

    //Effects
    float lookAtCount = 0;
    Vector3 lookAtDir;

    [Header("ShakeEffects")]
    public static float Shakeforce;
    public float ShakeDampen;

    public float InvertedX { get; set; }
    public float InvertedY { get; set; }
    public float SensiX { get; set; }
    public float SensiY { get; set; }

    Vector3 HitNormal;

    void Start()
    {
        Locked = false;
        InitialLockedRotationSpeed = LockedRotationSpeed;
        InitialMoveSpeed = moveSpeed;
        InitialRotationSpeed = rotSpeed;
    }

    void LateUpdate () {

        CameraMovement();
        CameraRotation();
        CameraCollision();
        CamereApplyEffects();
        CameraSet();
        CameraHighSpeedLock();

    }

    void CameraMovement()
    {
        //LookTimer
        if (lookTimer < 0)
        {
            RotateDirection(lookdir, Mathf.RoundToInt(LockedRotationSpeed), heighttolook);
            lookTimer += Time.deltaTime;
        }

        //Copy and Lerp the player's Pos
        if (!Locked)
        {
            if (Player.GroundNormal.y < AngleThreshold && Player.Grounded || Player.b_normalSpeed > LockCamAtHighSpeed)
            {
                PlayerPosLerped.position = Target.position;
                Quaternion newrot = Player.transform.rotation;
                PlayerPosLerped.rotation = Quaternion.Lerp(PlayerPosLerped.rotation, newrot, Time.deltaTime * CameraVerticalRotationSpeed);
            }
            else
            {
                PlayerPosLerped.position = Target.position;
                Quaternion newrot = Player.transform.rotation;
                newrot.eulerAngles = new Vector3(0, newrot.eulerAngles.y, 0);
                PlayerPosLerped.rotation = Quaternion.Lerp(PlayerPosLerped.rotation, newrot, Time.deltaTime * CameraVerticalRotationSpeed);
            }

            x += (Input.GetAxis("Horizontal_right") * ((InputXSpeed) * SensiX)* InvertedX) * Time.deltaTime;
            y -= (Input.GetAxis("Vertical_right") * ((InputYSpeed) * SensiY) * InvertedY) * Time.deltaTime;
            x += (Input.GetAxis("Mouse X") * ((InputXSpeed) * SensiX) * InvertedX) * Time.deltaTime;
            y -= (Input.GetAxis("Mouse Y") * ((InputYSpeed) * SensiY) * InvertedY) * Time.deltaTime;
        }
        else
        {
            PlayerPosLerped.rotation = Quaternion.Lerp(PlayerPosLerped.rotation, Quaternion.LookRotation(Vector3.forward), Time.deltaTime * CameraVerticalRotationSpeed);
            RotateDirection(lookdir, Mathf.RoundToInt(LockedRotationSpeed), heighttolook);
        }
        z = 0;
    }

    void CameraRotation()
    {
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

        rotation = Quaternion.Euler(y, x, 0);
        rotation = PlayerPosLerped.rotation * rotation;
    }

    void CameraCollision()
    {
        //Collision

        float dist;
        RaycastHit hit;

        Debug.DrawRay(Target.position, -transform.forward, Color.blue);
        if (Physics.Raycast(Target.position, -transform.forward, out hit, -CameraMaxDistance, CollidableLayers))
        {
            dist = (-hit.distance);
            HitNormal = hit.normal;
        }
        else
        {
            HitNormal = Vector3.zero;
            dist = CameraMaxDistance;
        }


        var position = rotation * new Vector3(0, 0, dist + 0.3f) + Target.position;

        LerpedRot = rotation;
        LerpedPos = position;
    }

    void CamereApplyEffects()
    {
        lookTimer += Time.deltaTime;
        if(lookTimer < 0)
        {
            LookAt(lookAtDir);
        }
    }

    void LookAt(Vector3 dir)
    {
        RotateDirection(dir, Mathf.RoundToInt(LockedRotationSpeed), y);
    }

    void CameraSet()
    {
        if (LockHeight && Player.Grounded)
        {
            FollowDirection(HeightToLock,LockHeightSpeed);
        }

        float y = Player.rigidbody.velocity.y;
        if (MoveHeightBasedOnSpeed && !Player.Grounded && y < FallSpeedThreshold )
        {
            FollowDirection(-y,HeightFollowSpeed);
        }
        else if (y > FallSpeedThreshold && !Player.Grounded)
        {
            FollowDirection(HeightToLock, LockHeightSpeed);
        }

        moveSpeed = Mathf.Lerp(moveSpeed, CameraMoveSpeed, Time.deltaTime * MoveLerpingSpeed);
        rotSpeed = Mathf.Lerp(rotSpeed, CameraRotationSpeed, Time.deltaTime * RotationLerpingSpeed);

        transform.position = Vector3.Lerp(transform.position, LerpedPos + HitNormal, Time.deltaTime * moveSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, LerpedRot, Time.deltaTime * rotSpeed);

        //transform.position = LerpedPos + HitNormal;
        //transform.rotation = LerpedRot;


        //Shakedown!
        float noiseX = (Random.Range(-Shakeforce, Shakeforce));
        float noiseY = (Random.Range(-Shakeforce, Shakeforce));
        float shakeX = (transform.position.x + noiseX);
        float shakeY = (transform.position.y + noiseY);

        transform.position = new Vector3(shakeX, shakeY, transform.position.z);
        Shakeforce = Mathf.Lerp(Shakeforce, 0, Time.deltaTime * ShakeDampen);
    }

    void CameraHighSpeedLock()
    {
        if (Player.XZmag > LockCamAtHighSpeed && (lookTimer > 0))
        {
            FollowDirection(3, 14, -10,0);
        }
    }

    public void RotateDirection(Vector3 dir, int speed, float height)
    {

        float dot = Vector3.Dot(dir, transform.right);
        x += (dot * speed) * (Time.deltaTime * 100);
        y = Mathf.Lerp(y, height, Time.deltaTime * 5);

    }

    public void FollowDirection(float speed, float height, float distance, float Yspeed)
    {
        if (!Locked)
        {

            float dot = Vector3.Dot(Skin.forward, transform.right);
            x += (dot * speed) * (Time.deltaTime * 100);

            y = Mathf.Lerp(y, height, Time.deltaTime * Yspeed);

        }
    }
    public void FollowDirection(float height, float speed)
    {
        if (!Locked)
        {
            y = Mathf.Lerp(y, height, Time.deltaTime * speed);
        }
    }

    public float ClampAngle(float angle, float min, float max)
    {
        if (angle < -360F)
            angle += 360F;
        if (angle > 360F)
            angle -= 360F;

        return Mathf.Clamp(angle, min, max);
    }

    //Set camera and overloads (function with the same name are different options)

    public void SetCamera(Vector3 dir, float duration, float heightSet )
    {

        lookAtDir = dir;
        lookTimer = -duration;
        heighttolook = heightSet;
        LockedRotationSpeed = InitialLockedRotationSpeed * 0.006f;

    }
    public void SetCamera(Vector3 dir, float duration, float heightSet, float speed)
    {

        lookAtDir = dir;
        lookTimer = -duration;
        heighttolook = heightSet;
        LockedRotationSpeed = speed;

    }
    public void SetCamera(Vector3 dir, float duration, float heightSet, float speed, float lagSet)
    {

        lookAtDir = dir;
        lookTimer = -duration;
        heighttolook = heightSet;
        LockedRotationSpeed = speed;
        moveSpeed = lagSet;
        rotSpeed = lagSet * 0.1f;

    }
    public void SetCamera(Vector3 dir, float duration)
    {

        lookAtDir = dir;
        lookTimer = -duration;
        LockedRotationSpeed = InitialLockedRotationSpeed;

    }
    public void SetCamera(float lagSet)
    {

        moveSpeed = lagSet;
        rotSpeed = lagSet * 0.1f;

    }

}
