using UnityEngine;
using System.Collections;

public class RotateRing : MonoBehaviour {

    public Vector3 RotateSpeed;
    public float InitialRandomRotationAmm;

    void Start()
    {
        Vector3 rand = new Vector3(0, Random.Range(0, InitialRandomRotationAmm), 0);
        transform.Rotate(rand);
    }

	void Update () {

        transform.Rotate(RotateSpeed * Time.deltaTime);

    }
}
