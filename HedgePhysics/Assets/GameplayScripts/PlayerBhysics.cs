using UnityEngine;
using System.Collections;

public class PlayerBhysics : MonoBehaviour
{
    [Header("Movement Values")] public float MoveAccell = 0.5f;
    public AnimationCurve AccellOverSpeed;
    public float AccellShiftOverSpeed;
    public float MoveDecell = 1.3f;
    public float AirDecell = 1.05f;
    public float TangentialDrag;
    public float TangentialDragShiftSpeed;
    public AnimationCurve TangDragOverSpeed;
    public float TopSpeed = 15;
    public float MaxSpeed = 30;
    public float MaxFallingSpeed = 30;
    public float m_JumpPower = 2;
    public float GroundStickingDistance = 1;
    public float GroundStickingPower = -1;
    public float SlopeStandingLimit = 0.8f;
    public float SlopePower = 0.5f;
    public float SlopeRunningAngleLimit = 0.5f;
    public float SlopeSpeedLimit = 10;
    public float UphillMultiplier = 0.5f;
    public float DownhillMultiplier = 2;
    public float StartDownhillMultiplier = -7;
    public AnimationCurve SlopePowerOverSpeed;
    public float SlopePowerShiftSpeed;
    public float LandingConversionFactor = 2;

    [Header("AirMovementExtras")] public float AirControlAmmount = 2;
    public float AirSkiddingForce = 10;
    public bool StopAirMovementIfNoInput = false;


    public bool Grounded { get; set; }
    public Vector3 GroundNormal { get; set; }
    public Vector3 CollisionPointsNormal { get; set; }

    public Rigidbody rigidbody { get; set; }

    public Vector3 Gravity;
    public Vector3 MoveInput { get; set; }

    [Header("Other Values")] public float GroundOffset;
    RaycastHit hit;
    public Transform CollisionPoint;
    public Collider CollisionSphere;
    public Collider CollisionCapsule;
    public Transform MainCamera;
    public Transform Colliders;
    public SonicSoundsControl sounds;

    public float RayToGroundDistance = 0.5f;

    public DebugUI Debui;


    [Header("Rolling Values")] public float RollingLandingBoost;
    public float RollingDownhillBoost;
    public float RollingUphillBoost;
    public float RollingStartSpeed;
    public float RollingTurningDecreace;
    public bool isRolling { get; set; }

    //Cache

    public float curvePosAcell { get; set; }
    public float curvePosTang { get; set; }
    public float curvePosSlope { get; set; }
    public float b_normalSpeed { get; set; }
    public Vector3 b_normalVelocity { get; set; }
    public Vector3 b_tangentVelocity { get; set; }

    //Etc
    [Header("Etc Values")] public bool UseSphereToGetNormal;

    Vector3 KeepNormal;
    float KeepNormalCounter;
    public bool WasOnAir { get; set; }
    public Vector3 PreviousInput { get; set; }
    public Vector3 RawInput { get; set; }
    public Vector3 PreviousRawInput { get; set; }
    public Vector3 PreviousRawInputForAnim { get; set; }
    public float SpeedMagnitude { get; set; }
    public float XZmag { get; set; }

    private void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        PreviousInput = transform.forward;
    }

    void FixedUpdate()
    {
        GeneralPhysics();
    }

    void Update()
    {
        InputChecks();
    }

    void InputChecks()
    {
        //Rolling
        if (Input.GetButton("R1") && rigidbody.velocity.sqrMagnitude > RollingStartSpeed)
        {
            isRolling = true;
        }
        if (Input.GetButtonUp("R1"))
        {
            isRolling = false;
        }
    }

    void GeneralPhysics()
    {
        //Set Previous input
        if (RawInput.sqrMagnitude >= 0.03f)
        {
            PreviousRawInputForAnim = RawInput * 90;
            PreviousRawInputForAnim = PreviousRawInputForAnim.normalized;
        }

        if (MoveInput.sqrMagnitude >= 0.9f)
        {
            PreviousInput = MoveInput;
        }
        if (RawInput.sqrMagnitude >= 0.9f)
        {
            PreviousRawInput = RawInput;
        }

        //Set Curve thingies
        curvePosAcell = Mathf.Lerp(curvePosAcell,
            AccellOverSpeed.Evaluate((rigidbody.velocity.sqrMagnitude / MaxSpeed) / MaxSpeed),
            Time.fixedDeltaTime * AccellShiftOverSpeed);
        curvePosTang = Mathf.Lerp(curvePosTang,
            TangDragOverSpeed.Evaluate((rigidbody.velocity.sqrMagnitude / MaxSpeed) / MaxSpeed),
            Time.fixedDeltaTime * TangentialDragShiftSpeed);
        curvePosSlope = Mathf.Lerp(curvePosSlope,
            SlopePowerOverSpeed.Evaluate((rigidbody.velocity.sqrMagnitude / MaxSpeed) / MaxSpeed),
            Time.fixedDeltaTime * SlopePowerShiftSpeed);

        // Apply Max Speed Limit
        XZmag = new Vector3(rigidbody.velocity.x, 0, rigidbody.velocity.z).magnitude;

        // Do it for X and Z
        if (XZmag > MaxSpeed)
        {
            Vector3 ReducedSpeed = rigidbody.velocity;
            float keepY = rigidbody.velocity.y;
            ReducedSpeed = Vector3.ClampMagnitude(ReducedSpeed, MaxSpeed);
            ReducedSpeed.y = keepY;
            rigidbody.velocity = ReducedSpeed;
        }

        //Do it for Y
        if (Mathf.Abs(rigidbody.velocity.y) > MaxFallingSpeed)
        {
            Vector3 ReducedSpeed = rigidbody.velocity;
            float keepX = rigidbody.velocity.x;
            float keepZ = rigidbody.velocity.z;
            ReducedSpeed = Vector3.ClampMagnitude(ReducedSpeed, MaxSpeed);
            ReducedSpeed.x = keepX;
            ReducedSpeed.z = keepZ;
            rigidbody.velocity = ReducedSpeed;
        }

        // Check for Ground
        if (UseSphereToGetNormal)
        {
            Debug.DrawRay(transform.position, CollisionPointsNormal, Color.blue);
            if (Physics.Raycast(transform.position, -CollisionPointsNormal, out hit, RayToGroundDistance))
            {
                GroundNormal = hit.normal;
                Grounded = true;
                GroundMovement();
            }
            else
            {
                Grounded = false;
                GroundNormal = Vector3.zero;
                AirMovement();
            }
        }
        else
        {
            if (Physics.Raycast(transform.position, -transform.up, out hit, RayToGroundDistance))
            {
                GroundNormal = hit.normal;
                Grounded = true;
                GroundMovement();
            }
            else
            {
                Grounded = false;
                GroundNormal = Vector3.zero;
                AirMovement();
            }
        }

        //Rotate Colliders
        if (Grounded)
        {
            transform.rotation = Quaternion.FromToRotation(transform.up, GroundNormal) * transform.rotation;
            KeepNormal = GroundNormal;
            KeepNormalCounter = 0;
        }
        else
        {
            //Keep the rotation after exiting the ground for a while, to avoid collision issues.
            KeepNormalCounter += 1;
            if (KeepNormalCounter < 5)
            {
                transform.rotation = Quaternion.FromToRotation(transform.up, KeepNormal) * transform.rotation;
            }
            else
            {
                //transform.rotation = Quaternion.FromToRotation(transform.up, GroundNormal) * transform.rotation;
                //transform.rotation = Quaternion.LookRotation(transform.forward, Vector3.up);
                transform.rotation = Quaternion.Euler(0, transform.eulerAngles.y, 0);
                //Debug.Log("Rots");
            }
        }
    }

    void HandleGroundControl(float deltaTime, Vector3 input)
    {
        //By Damizean

        // We assume input is already in the Player's local frame...
        // If there is some input...

        if (input.sqrMagnitude != 0.0f)
        {
            // Normalize to get input direction.

            var inputDirection = input.normalized;
            var inputMagnitude = input.magnitude;

            // Fetch velocity in the Player's local frame, decompose into lateral and vertical
            // motion, and decompose lateral motion further into normal and tangential components.

            var velocity = rigidbody.velocity;
            var localVelocity = transform.InverseTransformDirection(velocity);

            var lateralVelocity = new Vector3(localVelocity.x, 0.0f, localVelocity.z);
            var verticalVelocity = new Vector3(0.0f, localVelocity.y, 0.0f);

            var normalSpeed = Vector3.Dot(lateralVelocity, inputDirection);
            var normalVelocity = inputDirection * normalSpeed;
            var tangentVelocity = lateralVelocity - normalVelocity;

            // Note: normalSpeed is the magnitude of normalVelocity, with the added
            // bonus that it's signed. If positive, the speed goes towards the same
            // direction than the input :)


            if (normalSpeed < TopSpeed)
            {
                // Accelerate towards the input direction.
                if (!isRolling)
                {
                    normalSpeed += MoveAccell * deltaTime * inputMagnitude;
                    normalSpeed = Mathf.Min(normalSpeed, TopSpeed);
                }
                else
                {
                    normalSpeed += 0 * deltaTime * inputMagnitude;
                    normalSpeed = Mathf.Min(normalSpeed, TopSpeed);
                }

                // Rebuild back the normal velocity with the correct modulus.
                if (normalSpeed >= 0f)
                {
                    normalVelocity = inputDirection * normalSpeed;
                }
                else
                {
                    // (Reverse the inpit of inputdirection (on x and z, here)
                    normalVelocity = inputDirection * normalSpeed;
                }
            }

            // Additionally, we can apply some drag on the tangent directions for
            // tighter control.

            float curvePosTang = (rigidbody.velocity.sqrMagnitude / MaxSpeed) / MaxSpeed;

            if (!isRolling)
            {
                tangentVelocity = Vector3.MoveTowards(tangentVelocity, Vector3.zero,
                    (TangentialDrag * TangDragOverSpeed.Evaluate(curvePosTang)) * deltaTime * inputMagnitude);
            }
            else
            {
                tangentVelocity = Vector3.MoveTowards(tangentVelocity, Vector3.zero,
                    ((TangentialDrag * RollingTurningDecreace) * TangDragOverSpeed.Evaluate(curvePosTang)) * deltaTime *
                    inputMagnitude);
            }


            // Compose local velocity back and compute velocity back into the Global frame.
            // You probably want to delay doing this to the end of the physics processing,
            // as transformations can incur into numerical damping of the velocities.
            // The last step is included only for the sake of completeness.

            localVelocity = normalVelocity + tangentVelocity + verticalVelocity;
            velocity = transform.TransformDirection(localVelocity);
            rigidbody.velocity = velocity;

            //Export nescessary variables

            b_normalSpeed = normalSpeed;
            b_normalVelocity = normalVelocity;
            b_tangentVelocity = tangentVelocity;

            //DEBUG VARIABLES

            Debui.inputDirection = inputDirection;
            Debui.inputMagnitude = inputMagnitude;
            Debui.velocity = rigidbody.velocity;
            Debui.localVelocity = localVelocity;
            Debui.normalSpeed = normalSpeed;
            Debui.normalVelocity = normalVelocity;
            Debui.tangentVelocity = tangentVelocity;
        }
    }

    void GroundMovement()
    {
        //Stop Rolling
        if (rigidbody.velocity.sqrMagnitude < 20)
        {
            isRolling = false;
        }

        //Slope Physics
        SlopePlysics();

        // Call Ground Control
        HandleGroundControl(1, MoveInput * curvePosAcell);

        //Reduce speed
        if (MoveInput == Vector3.zero)
        {
            rigidbody.velocity = rigidbody.velocity / MoveDecell;
        }

        //Set magnitude reference variable
        SpeedMagnitude = rigidbody.velocity.magnitude;
    }

    void SlopePlysics()
    {
        //ApplyLandingSpeed
        if (WasOnAir && Grounded)
        {
            Vector3 Addsped;

            if (!isRolling)
            {
                Addsped = GroundNormal * LandingConversionFactor;
                StickToGround(GroundStickingPower);
            }
            else
            {
                Addsped = (GroundNormal * LandingConversionFactor) * RollingLandingBoost;
                StickToGround(GroundStickingPower * RollingLandingBoost);
                sounds.SpinningSound();
            }

            Addsped.y = 0;
            AddVelocity(Addsped);
            WasOnAir = false;
        }

        //Get out of slope if speed it too low
        if (rigidbody.velocity.sqrMagnitude < SlopeSpeedLimit && SlopeRunningAngleLimit > GroundNormal.y)
        {
            transform.rotation = Quaternion.identity;
            AddVelocity(GroundNormal * 3);
        }
        else
        {
            //Sticking to ground power
            StickToGround(GroundStickingPower);
        }

        //Apply slope power
        if (Grounded && GroundNormal.y < SlopeStandingLimit)
        {
            if (rigidbody.velocity.y > StartDownhillMultiplier)
            {
                if (!isRolling)
                {
                    Vector3 force = new Vector3(0, (SlopePower * curvePosSlope) * UphillMultiplier, 0);
                    AddVelocity(force);
                }
                else
                {
                    Vector3 force = new Vector3(0, (SlopePower * curvePosSlope) * UphillMultiplier, 0) *
                                    RollingUphillBoost;
                    AddVelocity(force);
                }
            }
            else
            {
                if (MoveInput != Vector3.zero && b_normalSpeed > 0)
                {
                    if (!isRolling)
                    {
                        Vector3 force = new Vector3(0, (SlopePower * curvePosSlope) * DownhillMultiplier, 0);
                        AddVelocity(force);
                    }
                    else
                    {
                        Vector3 force = new Vector3(0, (SlopePower * curvePosSlope) * DownhillMultiplier, 0) *
                                        RollingDownhillBoost;
                        AddVelocity(force);
                    }
                }
                else
                {
                    Vector3 force = new Vector3(0, SlopePower * curvePosSlope, 0);
                    AddVelocity(force);
                }
            }
        }
    }

    public void StickToGround(float StickingPower)
    {
        CollisionPoint.LookAt(transform.position);
        if (Physics.Raycast(CollisionPoint.position, -Colliders.up, out hit, GroundStickingDistance) &&
            !Input.GetButton("A"))
        {
            Vector3 force = hit.normal * StickingPower;
            AddVelocity(force);
        }
    }

    public void AddVelocity(Vector3 force)
    {
        rigidbody.velocity = rigidbody.velocity + force;
    }

    void AirMovement()
    {
        //AddSpeed
        HandleGroundControl(AirControlAmmount, MoveInput);

        //Get out of roll
        isRolling = false;

        //Apply Gravity
        rigidbody.velocity = rigidbody.velocity + Gravity;

        //Reduce speed
        if (MoveInput == Vector3.zero && StopAirMovementIfNoInput)
        {
            Vector3 ReducedSpeed = rigidbody.velocity;
            ReducedSpeed.x = ReducedSpeed.x / AirDecell;
            ReducedSpeed.z = ReducedSpeed.z / AirDecell;
            rigidbody.velocity = ReducedSpeed;
        }

        //Get set for landing
        WasOnAir = true;

        //Air Skidding  
        if (b_normalSpeed < 0 && !Grounded)
        {
            HandleGroundControl(1, (MoveInput * AirSkiddingForce) * MoveAccell);
        }

        //Max Falling Speed
        if (rigidbody.velocity.y < MaxFallingSpeed)
        {
            rigidbody.velocity = new Vector3(rigidbody.velocity.x, MaxFallingSpeed, rigidbody.velocity.z);
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        //Gizmos.DrawRay(DownRay);
    }

    public void OnCollisionStay(Collision col)
    {
        Vector3 Prevnormal = GroundNormal;
        foreach (ContactPoint contact in col.contacts)
        {
            Debug.DrawRay(contact.point, contact.normal, Color.white);

            //Set Middle Point
            Vector3 pointSum = Vector3.zero;
            Vector3 normalSum = Vector3.zero;
            for (int i = 0; i < col.contacts.Length; i++)
            {
                pointSum = pointSum + col.contacts[i].point;
                normalSum = normalSum + col.contacts[i].normal;
            }

            pointSum = pointSum / col.contacts.Length;
            CollisionPointsNormal = normalSum / col.contacts.Length;

            if (rigidbody.velocity.normalized != Vector3.zero)
            {
                CollisionPoint.position = pointSum;
            }
        }
    }
}