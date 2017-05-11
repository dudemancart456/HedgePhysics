using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DebugUI : MonoBehaviour
{
    public PlayerBhysics phys;
    public ActionManager action;
    public PlayerBinput bimp;

    public Vector3 inputDirection;
    public float inputMagnitude;

    public Vector3 velocity;
    public Vector3 localVelocity;

    public float normalSpeed;
    public Vector3 normalVelocity;
    public Vector3 tangentVelocity;

    public Vector3 modTangent;


    void Update()
    {
        string debug = " DEBUG \n" +
                       " Speed: " + phys.rigidbody.velocity + "\n" +
                       " Speed Magnitude: " + phys.rigidbody.velocity.magnitude + "\n" +
                       " SlopePower: " + phys.curvePosSlope + "\n" +
                       " Grounded: " + phys.Grounded + "\n" +
                       " TangentialDragOver: " + phys.curvePosTang + "\n" +
                       " inputDirection: " + inputDirection + "\n" +
                       " inputMagnitude: " + inputMagnitude + "\n" +
                       " velocity: " + velocity + "\n" +
                       " localVelocity: " + localVelocity + "\n" +
                       " normalSpeed: " + normalSpeed + "\n" +
                       " normalVelocity: " + normalVelocity + "\n" +
                       " tangentVelocity: " + tangentVelocity + "\n" +
                       " TangentMod: " + modTangent + "\n" +
                       " action: " + action.Action + "\n" +
                       " Input A: " + Input.GetButton("A") + "\n" +
                       " Input APress: " + Input.GetButtonDown("A") + "\n" +
                       " Normal: " + phys.GroundNormal;

        gameObject.GetComponent<Text>().text = debug;
    }
}