using UnityEngine;
using System.Collections;

public class EnemySpawnerEternal : MonoBehaviour {

    public float Distance;
    Transform Player;

    public float RespawnTime;
    float counter;

    public GameObject Enemy;
    public bool HasSpawned { get; set; }
    public GameObject TeleportSparkle;


    void Start()
    {
        HasSpawned = false;
        Player = GameObject.FindWithTag("Player").transform;
        GetComponent<MeshRenderer>().enabled = false;
        counter = RespawnTime;
    }

    void LateUpdate()
    {
        counter += Time.deltaTime;
        if (Vector3.Distance(Player.position, transform.position) < Distance)
        {
            if (!HasSpawned)
            {
                if (counter > RespawnTime)
                {
                    SpawnInNormal();
                }
            }
        }
    }

    void SpawnInNormal()
    {
        HasSpawned = true;
        Instantiate(TeleportSparkle, transform.position, transform.rotation);
        GameObject em = (GameObject)Instantiate(Enemy, transform.position, transform.rotation);
        em.GetComponent<EnemyHealth>().SpawnReference = this;
        HomingAttackControl.UpdateHomingTargets();
    }

    public void ResartSpawner()
    {
        HasSpawned = false;
        counter = 0;
    }
}
