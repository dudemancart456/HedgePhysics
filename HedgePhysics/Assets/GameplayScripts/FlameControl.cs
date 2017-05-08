using UnityEngine;
using System.Collections;

public class FlameControl : MonoBehaviour {

    Transform Player;
    public float Distance;
    public ParticleSystem Particle;

	void Start () {

        Player = GameObject.FindWithTag("Player").transform;
        InvokeRepeating("Flame", 0.1f, 1.0f);
    }

    void Flame()
    {
        if (Vector3.Distance(Player.position, transform.position) < Distance)
        {
            Particle.Emit(1);
        }
    }
}
