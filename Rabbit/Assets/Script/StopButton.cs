﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StopButton : MonoBehaviour {

	void Start () {
    }
	
	// Update is called once per frame
	void Update () {
	    	
	}

    public void StopDraw()
    {
        GameScene.InGameManager.Getinstance().gestureManager.isDrawable = false;

        Debug.Log(GameScene.InGameManager.Getinstance().gestureManager.isDrawable);
    }
}
