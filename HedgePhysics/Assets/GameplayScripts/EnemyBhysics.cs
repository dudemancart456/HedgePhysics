using UnityEngine;
using System.Collections;

public class EnemyBhysics : MonoBehaviour {

    Rigidbody rigidbody;
    public float MoveAccell;
    public float MoveDecell;
    public float TangentialDrag;
    public float TopSpeed;

    public float b_normalSpeed { get; set; }
    public Vector3 b_normalVelocity { get; set; }
    public Vector3 b_tangentVelocity { get; set; }

    public Vector3 Gravity;
    public float GroundStickingPower;

    public bool Grounded { get; set; }
    public Vector3 AppliedInput { get; set; }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
    }

    void FixedUpdate()
    {
        AddVelocity(Gravity);
        GeneralPhysics();
    }

    public void GeneralPhysics()
    {
        if(AppliedInput == Vector3.zero)
        {
            rigidbody.velocity = rigidbody.velocity / MoveDecell;
            b_normalSpeed = 0;
        }

        AppliedInput = Vector3.zero;

        RaycastHit hit;
        if (Physics.Raycast(transform.position, -transform.up, out hit, 0.1f))
        {
            Grounded = true;
            GroundMovement();
        }
        else
        {
            Grounded = false;
        }
    }

    public void GroundMovement()
    {
        AddVelocity( -transform.up * GroundStickingPower);
    }
    public void AirMovement()
    {
        AddVelocity(Gravity);
    }

    public void AddVelocity(Vector3 force)
    {
        rigidbody.velocity = rigidbody.velocity + force;
    }

    public void HandleGroundControl(float deltaTime, Vector3 input)
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
                normalSpeed += MoveAccell * deltaTime * inputMagnitude;
                normalSpeed = Mathf.Min(normalSpeed, TopSpeed);

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

            tangentVelocity = Vector3.MoveTowards(tangentVelocity, Vector3.zero,
            (TangentialDrag) * deltaTime * inputMagnitude);


            // Compose local velocity back and compute velocity back into the Global frame.
            // You probably want to delay doing this to the end of the physics processing,
            // as transformations can incur into numerical damping of the velocities.
            // The last step is included only for the sake of completeness.

            localVelocity = normalVelocity + tangentVelocity + verticalVelocity;
            velocity = transform.TransformDirection(localVelocity);
            rigidbody.velocity = velocity;

            //Export nescessary variables

            AppliedInput = inputDirection;
            b_normalSpeed = normalSpeed;
            b_normalVelocity = normalVelocity;
            b_tangentVelocity = tangentVelocity;

        }
    }

}
