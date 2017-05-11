using UnityEngine;
using System.Collections;

public class RotateRing : MonoBehaviour
{
    public Vector3 RotateSpeed;
    public float InitialRandomRotationAmm;
    public bool SetBasedOnXpos = true;

    void Start()
    {
        Vector3 rand = new Vector3(0, Random.Range(0, InitialRandomRotationAmm), 0);
        transform.Rotate(rand);
        if (SetBasedOnXpos)
        {
            Vector3 set = new Vector3(0, transform.position.x + transform.position.z, 0);
            transform.Rotate(set);
        }
    }

    void Update()
    {
        transform.Rotate(RotateSpeed * Time.deltaTime);
    }
}