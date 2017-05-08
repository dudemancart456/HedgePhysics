using UnityEngine;
using System.Collections;

public class EnemyHealth : MonoBehaviour {

    public int MaxHealth = 1;
    int HP;

    public GameObject Explosion;

    void Awake()
    {
        HP = MaxHealth;
    }

    public void DealDamage(int Damage)
    {
        HP -= Damage;
        if(HP <= 0)
        {
            GameObject.Instantiate(Explosion, transform.position,Quaternion.identity);
            Destroy(gameObject);
        }
    }

}
