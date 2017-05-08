using UnityEngine;
using System.Collections;

public class EnemySpawner : MonoBehaviour {

    public float Distance;
    Transform Player;
    public GameObject TeleportSparkle;

    public GameObject Enemy;

    void Start()
    {
        Player = GameObject.FindWithTag("Player").transform;
        GetComponent<MeshRenderer>().enabled = false;
    }

    void Update()
    {
        if(Vector3.Distance(Player.position,transform.position) < Distance)
        {
            SpawnInNormal();
        }
    }

    void SpawnInNormal()
    {
        Instantiate(TeleportSparkle, transform.position, transform.rotation);
        Instantiate(Enemy, transform.position, transform.rotation);
        HomingAttackControl.UpdateHomingTargets();
        Destroy(gameObject);
    }

}
