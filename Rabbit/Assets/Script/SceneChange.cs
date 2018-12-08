using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour {

	// Use this for initialization
	public void ChangeScene(string SceneName)
    {
        SceneManager.LoadScene(SceneName);
    }

}
