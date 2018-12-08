using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StartButton : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    public void StartDraw()
    {
        GameScene.InGameManager.Getinstance().gestureManager.isDrawable = true;
        Debug.Log(GameScene.InGameManager.Getinstance().gestureManager.isDrawable);
    }
}
