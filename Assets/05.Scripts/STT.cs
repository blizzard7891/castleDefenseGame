using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class STT : MonoBehaviour {

    public string msg = "nothing";

    void OnGUI()
    {
        if(GUI.Button(new Rect(100, 0, 200, 100), "Recognizer"))
        {
            Call_STTstart();
        }

        GUI.Label(new Rect(100, 200, 200, 100), msg);
    }

	// Use this for initialization
	void Start ()
    {
        PluginManager.Instance.setcallbackSTTresult(new delegateSTTresult(STTresult));
    }

    void STTresult(string result)
    {
        makelog("output: " + result);
    }

    void makelog(string logmsg)
    {
        msg = logmsg;
    }

    public void Call_STTstart()
    {
        PluginManager.Instance.call_androidSTT();
    }
}
