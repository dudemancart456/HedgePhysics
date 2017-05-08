using UnityEngine;
using System.Collections;

public enum MonitorType
{
    Ring, Shield
}

public class MonitorData : MonoBehaviour {

    public MonitorType Type;
    public int RingAmmount;

    public GameObject MonitorExplosion;

    public void DestroyMonitor()
    {
        GameObject.Instantiate(MonitorExplosion, transform.position, Quaternion.identity);
        Destroy(gameObject);
    }

}
