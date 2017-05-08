using UnityEngine;
using System.Collections;

public class SetPosition : MonoBehaviour {

    public Transform TargetPosition;
    public Vector3 Offset;

    Vector3 DynamicOffset;
    bool Dynamic;

	void Update () {

        if (!Dynamic)
        {
            transform.position = TargetPosition.position + Offset;
        }
        else
        {
            transform.position = TargetPosition.position + DynamicOffset;
        }

	}

    public void UseDynamicOffset(Vector3 offset)
    {
        Dynamic = true;
        DynamicOffset = (Vector3.Scale(Offset, offset));
    }
}
