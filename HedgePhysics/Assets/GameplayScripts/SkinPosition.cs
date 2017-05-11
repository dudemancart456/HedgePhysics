using UnityEngine;
using System.Collections;

public class SkinPosition : MonoBehaviour
{
    SetPosition setPosScript;
    public PlayerBhysics Player;

    void Awake()
    {
        setPosScript.GetComponent<SetPosition>();
    }

    void Update()
    {
        setPosScript.UseDynamicOffset(Player.transform.up);
    }
}