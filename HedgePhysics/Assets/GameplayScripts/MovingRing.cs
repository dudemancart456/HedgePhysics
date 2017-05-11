using UnityEngine;
using System.Collections;

public class MovingRing : MonoBehaviour
{
    public float CollectTime;
    float coutner;
    public bool colectable { get; set; }
    float flickCount = 0;

    public float Duration;

    void Start()
    {
        colectable = false;
    }

    void Update()
    {
        coutner += Time.deltaTime;
        if (coutner > CollectTime)
        {
            colectable = true;
        }
        if (coutner > Duration - 2)
        {
            RingFlicker();
        }
        if (coutner > Duration)
        {
            Destroy(gameObject);
        }
    }

    public void RingFlicker()
    {
        flickCount += Time.deltaTime * 180;
        if (flickCount < 0)
        {
            gameObject.GetComponent<MeshRenderer>().enabled = false;
        }
        else
        {
            gameObject.GetComponent<MeshRenderer>().enabled = true;
        }
        if (flickCount > 10)
        {
            flickCount = -10;
        }
    }
}