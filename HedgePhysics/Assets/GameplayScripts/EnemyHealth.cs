using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    public int MaxHealth = 1;
    int HP;

    public GameObject Explosion;
    public EnemySpawnerEternal SpawnReference { get; set; }

    void Awake()
    {
        HP = MaxHealth;
    }

    public void DealDamage(int Damage)
    {
        HP -= Damage;
        if(HP <= 0)
        {
            if(SpawnReference != null)
            {
                SpawnReference.ResartSpawner();
            }
            GameObject.Instantiate(Explosion, transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
