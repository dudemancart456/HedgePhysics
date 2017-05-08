using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LandingSound : MonoBehaviour {

    public PlayerBhysics Player;
    AudioSource source;
    bool played = false;

    void Start()
    {
        source = GetComponent<AudioSource>();
    }

    void Update () {

        if (Player.Grounded)
        {
            if (!played)
            {
                played = true;
                source.Play();
            }
        }
        else
        {
            played = false;
        }

	}
}
