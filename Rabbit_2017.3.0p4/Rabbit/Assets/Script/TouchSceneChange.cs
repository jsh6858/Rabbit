using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class TouchSceneChange : MonoBehaviour {

	// Use this for initialization
	void Start () {
        //Sound_Manager.GetInstance().Stop_Sound();
        //Sound_Manager.GetInstance().PlaySound("title");
	}
	
	// Update is called once per frame
	void Update () {
		if(Input.GetKeyDown(KeyCode.Mouse0))
            SceneManager.LoadScene("Intro");
	}
}
