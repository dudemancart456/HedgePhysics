using UnityEngine;
using System.Collections;

public enum TriggerType
{
    LockToDirection, LookTowards, SetFree, SetFreeAndLookTowards
}

public class CameraTriggerData : MonoBehaviour {

    public TriggerType Type;
    public float CameraAltitude;
    public float ChangeDistance;

}
