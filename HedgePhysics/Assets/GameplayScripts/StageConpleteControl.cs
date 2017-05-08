using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StageConpleteControl : MonoBehaviour {

    //WHERE EVEN IS THIS SCRIPT AND WHY IS IT HERE?


    public float End;
    float counter;
    public static int LevelToGoNext;

    public Animator Anim;

    void Update()
    {
        //Debug.Log("i'm here");
        counter += Time.deltaTime;
        if(counter > End)
        {
            Anim.SetInteger("Action", 1);
            if(counter > End + 2.3f)
            {
                SceneManager.LoadScene(LevelToGoNext);
            }
        }
    }

}
