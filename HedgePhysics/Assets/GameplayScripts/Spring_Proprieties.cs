using UnityEngine;
using System.Collections;

public class Spring_Proprieties : MonoBehaviour {

    public float SpringForce;
    public bool IsAdditive;
    public Animator anim { get; set; }
    public bool LockControl = false;
    public float LockTime = 60;
    
    void Start()
    {
        anim = GetComponent<Animator>();
    } 

}
