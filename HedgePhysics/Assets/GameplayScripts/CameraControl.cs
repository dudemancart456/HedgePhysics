using UnityEngine;
using System.Collections;

public class CameraControl : MonoBehaviour {

    public HedgeCamera Cam;
    float InitialDistance;

    void Awake()
    {
        InitialDistance = Cam.CameraMaxDistance;
    }

	public void OnTriggerEnter(Collider col)
    {
        if(col.tag == "CameraTrigger")
        {
            if(col.GetComponent<CameraTriggerData>() != null)
            {
                if(col.GetComponent<CameraTriggerData>().Type == TriggerType.LookTowards)
                {
                    transform.rotation = Quaternion.identity;
                    Vector3 dir = col.transform.GetChild(0).up;

                    dir.y = col.GetComponent<CameraTriggerData>().CameraAltitude;

                    Cam.CameraMaxDistance = col.GetComponent<CameraTriggerData>().ChangeDistance;
                    Cam.ChangeDirection(0.4f, 0.4f, dir);
                }
                else if (col.GetComponent<CameraTriggerData>().Type == TriggerType.LockToDirection)
                {
                    transform.rotation = Quaternion.identity;
                    Vector3 dir = col.transform.GetChild(0).up;

                    dir.y = col.GetComponent<CameraTriggerData>().CameraAltitude;
                    Cam.CameraMaxDistance = col.GetComponent<CameraTriggerData>().ChangeDistance;
                    Cam.ChangeDirection(0.4f, 0.4f, dir);
                    Cam.Locked = true;
                }
                else if (col.GetComponent<CameraTriggerData>().Type == TriggerType.SetFree)
                {
                    Cam.CameraMaxDistance = InitialDistance;
                    Cam.Locked = false;
                }
                else if (col.GetComponent<CameraTriggerData>().Type == TriggerType.SetFreeAndLookTowards)
                {
                    transform.rotation = Quaternion.identity;
                    Vector3 dir = col.transform.GetChild(0).up;


                    dir.y = col.GetComponent<CameraTriggerData>().CameraAltitude;
                    Cam.ChangeDirection(0.4f, 0.4f, dir);
                    Cam.CameraMaxDistance = InitialDistance;
                    Cam.Locked = false;
                }
            }
        }

    }

}
