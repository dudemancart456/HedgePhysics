using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class LoadingScreenControl : MonoBehaviour {

    int count;
    int endCount;
    bool over;

    public Animator Anim;
    public int LoadingStart;
    public Camera Cam;

    public int LevelToLoad;
    AsyncOperation levelLoader;

    public static bool StageLoaded;

    void Start()
    {

    }

    void FixedUpdate()
    {
        count += 1;
        if (count == LoadingStart)
        {
            SceneManager.LoadSceneAsync(LevelToLoad, LoadSceneMode.Additive);
        }

        if(!over && StageLoaded)
        {
            over = true;
            StageLoaded = false;
        }

        if(over)
        {
            endCount += 1;
            if(endCount == 10)
            {
                if (Cam) { Destroy(Cam.gameObject); }
                Anim.SetInteger("Action", 1);
                SceneManager.SetActiveScene(SceneManager.GetSceneAt(1));
            }
            if(endCount > 60)
            {
                SceneManager.UnloadScene(0);
            }
        }

    }

}
