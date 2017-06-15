using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public delegate void delegateSTTresult(string result);

public class PluginManager : MonoBehaviour {

    private static PluginManager instance;

    public static PluginManager Instance
    {
        get
        {
            if(instance != null)
            {
                return instance;
            }

            GameObject go = new GameObject("pluginUnity");
            instance = go.AddComponent<PluginManager>();

            #if !UNITY_EDITOR
                        AndroidJavaClass AJC = new AndroidJavaClass("com.unity3d.player.UnityPlayer");
                        instance.AJO = AJC.GetStatic<AndroidJavaObject>("currentActivity");
            #endif

            return instance;
        }
    }
    
    AndroidJavaObject AJO;

    delegateSTTresult _STTresult;

    public void call_androidSTT()
    {
        if(AJO != null)
        {
            AJO.Call("StartSpeechRecoService");
        }
        else
        {
            Debug.Log("AJO == null");
        }
    }

    public void call_androidTTS(string word)
    {
        if(AJO != null)
        {
            AJO.Call("StartTextToSpeechService", word);
        }
        else
        {
            Debug.Log("AJO==null");
        }
    }

    public void call_androidQuestion(int num, bool correct)
    {
        if(AJO != null)
        {
            AJO.Call("StartQuestionTransmissionService",num, correct);
        }
        else
        {
            Debug.Log("AJO==null");
        }
    }

    public void call_androidScoreLoad()
    {
        if (AJO != null)
        {
            AJO.Call("SendScoreToUnity");
        }
        else
        {
            Debug.Log("AJO==null");
        }
    }

    public void call_saveGame2Score(int score)
    {
        if (AJO != null)
        {
            AJO.Call("SaveGame2Score",score);
        }
        else
        {
            Debug.Log("AJO==null");
        }
    }

    public void call_getGame2Score()
    {
        if (AJO != null)
        {
            AJO.Call("GetGame2Score");
        }
        else
        {
            Debug.Log("AJO==null");
        }
    }

    public void sttUnity(string result)
    {
        if(_STTresult != null)
        {
            _STTresult(result);
        }
    }
    
    public void setcallbackSTTresult(delegateSTTresult callback) { this._STTresult = callback; }
}
