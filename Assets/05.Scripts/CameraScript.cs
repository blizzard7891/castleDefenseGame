using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraScript : MonoBehaviour {
    private int screenSize = 1440;

	// Use this for initialization
	void Start () {
        Screen.SetResolution(screenSize*16/9, screenSize, true);
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
