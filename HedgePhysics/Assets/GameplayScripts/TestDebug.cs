using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TestDebug : MonoBehaviour
{
    public Text Tex;

    public Transform Quad_01;
    public Transform Quad_02;

    public Transform MovingCube;
    public AngleTest Ang;

    void Update()
    {
        float a = 1 ^ 3;

        Quaternion dotProduct = Quad_01.rotation * Quad_02.rotation;
        float dotReal = Quaternion.Dot(Quad_01.rotation, Quad_02.rotation);

        string debug = " DEBUG \n" +
                       " Quad 1 Rot: " + Quad_01.rotation + "\n" +
                       " Quad 2 Rot: " + Quad_02.rotation + "\n" +
                       " Dot: " + dotProduct + "\n" +
                       " DorReal: " + dotReal + "\n" +
                       //" Cube Quart: " + Ang.transform.rotation  + "\n" +
                       //" Cube Euler: " + Ang.transform.eulerAngles + "\n" +
                       //" Cube Forced: " + Ang.ForcedNormal + "\n" +
                       //" Cube Sined: " + Ang.AngleSined + "\n" +
                       //" Cube AngleAxis: " + Ang.AngleToAngle + "\n" +
                       //" Cube Angle: " + Ang.Angle + "\n" +
                       //" Cube Up: " + Ang.transform.forward + "\n" +
                       " ";

        Tex.text = debug;
    }
}