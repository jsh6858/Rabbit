using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SceneChange : MonoBehaviour
{
	public void ChangeScene(string SceneName)
    {
        if("GameScene" != SceneManager.GetActiveScene().name)
            Sound_Manager.GetInstance().Stop_Sound();

        SceneManager.LoadScene(SceneName);
    }
}
