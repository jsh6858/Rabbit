using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopButton : MonoBehaviour {

    GameObject StopImageObject;
    GameObject StartButtonObject;
	void Start () {
        StopImageObject = GameObject.Find("StopImage");
        StartButtonObject = GameObject.Find("StartButton");
    }
	
	// Update is called once per frame
	void Update () {
	    	
	}
}
