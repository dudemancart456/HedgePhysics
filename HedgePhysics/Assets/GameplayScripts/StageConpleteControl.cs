using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;

public class StageConpleteControl : MonoBehaviour {

    public float End;
    float counter;
    public static int LevelToGoNext;

    public Animator Anim;

    void Update()
    {
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
