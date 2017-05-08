using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class AnalogDebug : MonoBehaviour {

    public RectTransform AnalogStickPos;
    public float Size;

    public PlayerBinput Inp;

    void Update()
    {
        Vector3 Inpuy = new Vector3(Inp.UtopiaInput.x, Inp.UtopiaInput.z, Inp.UtopiaInput.y);
        AnalogStickPos.localPosition = Inpuy * Size;
    }

}
