using UnityEngine;
using System.Collections;

public class Rep : MonoBehaviour
{
    /*
    
            //DAMI's CODE
    
        void HandleGroundControl(float deltaTime, Vector3 input)
        {
            // We assume input is already in the Player's local frame...
            // If there is some input...
    
            if (input.sqrMagnitude != 0.0f)
            {
                //FORDAFUTURE Vector3 transformedInput = Quaternion.FromToRotation(cam.up, Plyr.GroundNormal) * (cam.rotation * moveImp);
                // Normalize to get input direction.
    
                var inputDirection = input.normalized;
                var inputMagnitude = input.magnitude;
    
                // Fetch velocity in the Player's local frame, decompose
                // into normal and tangential components.
    
                var velocity = Phys.velocity;
                var localVelocity = transform.InverseTransformDirection(velocity);
    
                var normalSpeed = Vector3.Dot(localVelocity, inputDirection);
                var normalVelocity = inputDirection * normalSpeed;
                var tangentVelocity = localVelocity - normalVelocity;
    
                // Note: normalSpeed is the magnitude of normalVelocity, with the added
                // bonus that it's signed. If positive, the speed goes towards the same
                // direction than the input :)
    
    
    
                if (normalSpeed < TopSpeed)
                {
                    // Accelerate towards the input direction.
    
                    normalSpeed = normalSpeed + MoveAccell * deltaTime * inputMagnitude;
                    normalSpeed = Mathf.Min(normalSpeed, TopSpeed);
    
                    // Rebuild back the normal velocity with the correct modulus.
    
                    normalVelocity = inputDirection * normalSpeed;
                }
    
                // Additionally, we can apply some drag on the tangent directions for
                // tighter control.
    
                tangentVelocity = Vector3.MoveTowards(tangentVelocity, Vector3.zero,
                    MoveDecell * deltaTime * (1 - inputMagnitude));
    
                // Compose local velocity back and compute velocity back into the Global frame.
                // You probably want to delay doing this to the end of the physics processing,
                // as transformations can incur into numerical damping of the velocities.
                // The last step is included only for the sake of completeness.
    
                localVelocity = normalVelocity + tangentVelocity;
                velocity = transform.TransformDirection(localVelocity);
                Phys.velocity = velocity;
    
                //DEBUG VARIABLES
    
                Debui.inputDirection = inputDirection;
                Debui.inputMagnitude = inputMagnitude;
                Debui.velocity = Phys.velocity;
                Debui.localVelocity = localVelocity;
                Debui.normalSpeed = normalSpeed;
                Debui.normalVelocity = normalVelocity;
                Debui.tangentVelocity = tangentVelocity;
    
            }
        }
    
        Murasaki's code
    
            Vector3 transformedInput = Quaternion.FromToRotation(cam.up, Plyr.GroundNormal) * (cam.rotation * moveImp);
    
        //DAMIS CODE, AGAIN
    
        void HandleGroundControl(float deltaTime, Vector3 input)
        {
            // We assume input is already in the Player's local frame...
            // If there is some input...
    
            if (input.sqrMagnitude != 0.0f)
            {
                //FORDAFUTURE Vector3 transformedInput = Quaternion.FromToRotation(cam.up, Plyr.GroundNormal) * (cam.rotation * moveImp);
                // Normalize to get input direction.
    
                var inputDirection = input.normalized;
                var inputMagnitude = input.magnitude;
    
                // Fetch velocity in the Player's local frame, decompose into lateral and vertical
                // motion, and decompose lateral motion further into normal and tangential components.
    
                var velocity = Phys.velocity;
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
    
                    normalVelocity = inputDirection * normalSpeed;
                }
    
                // Additionally, we can apply some drag on the tangent directions for
                // tighter control.
    
                tangentVelocity = Vector3.MoveTowards(tangentVelocity, Vector3.zero,
                    TangentialDrag * deltaTime * inputMagnitude);
    
                // Compose local velocity back and compute velocity back into the Global frame.
                // You probably want to delay doing this to the end of the physics processing,
                // as transformations can incur into numerical damping of the velocities.
                // The last step is included only for the sake of completeness.
    
                localVelocity = normalVelocity + tangentVelocity + verticalVelocity;
                velocity = transform.TransformDirection(localVelocity);
                Phys.velocity = velocity;
    
                //DEBUG VARIABLES
    
                Debui.inputDirection = inputDirection;
                Debui.inputMagnitude = inputMagnitude;
                Debui.velocity = Phys.velocity;
                Debui.localVelocity = localVelocity;
                Debui.normalSpeed = normalSpeed;
                Debui.normalVelocity = normalVelocity;
                Debui.tangentVelocity = tangentVelocity;
    
            }
        }
    
        //DAMIS INPUT
        
        Vector3 transformedInput = Quaternion.FromToRotation(MainCamera.up, GroundNormal) * (MainCamera.rotation * MoveInput);
        transformedInput = transform.InverseTransformDirection(transformedInput);
        transformedInput.y = 0.0f;
    
        //====
    
                    move = new Vector3(h, 0, v);
    
                Vector3 transformedInput = Quaternion.FromToRotation(cam.up, Plyr.GroundNormal) * (cam.rotation * move);
                transformedInput = transform.InverseTransformDirection(transformedInput);
                transformedInput.y = 0.0f;
                move = transformedInput;
                //move = Vector3.Lerp(move, transformedInput, Time.deltaTime * InputLerpSpeed);
    
        */

    /*

        Quaternion rotation = Quaternion.Euler(y, x, z);

        if (Player.Grounded)
        {
            LerpedNormal = Vector3.Lerp(LerpedNormal, Player.GroundNormal, Time.deltaTime * CameraNormalRotationSpeed);
            rotation = Quaternion.FromToRotation(Vector3.up, LerpedNormal) * rotation;
        }
        else
        {
            LerpedNormal = Vector3.Lerp(LerpedNormal, Vector3.up, Time.deltaTime * CameraNormalRotationSpeed);
            rotation = Quaternion.FromToRotation(Vector3.up, LerpedNormal) * rotation;
        }

        Vector3 negDistance = new Vector3(0.0f, 0.0f, CameraMaxDistance);
        Vector3 position = (rotation) * negDistance + Target.position;

        transform.position = Vector3.Lerp(transform.position, position, Time.deltaTime * CameraMoveSpeed);
        transform.rotation = Quaternion.Lerp(transform.rotation, rotation, Time.deltaTime * CameraRotationSpeed);
        */
}