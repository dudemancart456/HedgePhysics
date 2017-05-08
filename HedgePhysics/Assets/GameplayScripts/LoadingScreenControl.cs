using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class LoadingScreenControl : MonoBehaviour {

    int count;
    int endCount;
    bool over;

    public Animator Anim;
    public int LoadingStart;
    public Camera Cam;

    int LevelToLoad;
    AsyncOperation levelLoader;

    public static bool StageLoaded;
    public static string StageName1;
    public static string StageName2;

    public Text StageName1txt;
    public Text StageName2txt;

    void Start()
    {
        LevelToLoad = SceneController.LevelToLoad;
        StageName1txt.text = StageName1;
        StageName2txt.text = StageName2;
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
                SceneManager.UnloadSceneAsync(SceneManager.GetSceneByName("LoadingScreen"));
            }
        }

    }

}
