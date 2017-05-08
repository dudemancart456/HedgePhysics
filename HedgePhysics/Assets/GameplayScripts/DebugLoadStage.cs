using UnityEngine;
using System.Collections;

public class DebugLoadStage : MonoBehaviour {

    public int Stage;

    void Start()
    {
        SceneController.LoadStageLoading(3);
    }

}
