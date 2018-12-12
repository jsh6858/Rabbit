using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum SCENESTATE
{
    TitleScene,
    IntroScene,
    GameScene,
    RankingScene,
}


public class SceneController : MonoBehaviour
{
    private static SceneController instance = null;
    private static GameObject container;

    public static SceneController GetInstance()
    {
        if(!instance)
        {
            container = new GameObject();
            container.name = "SceneController";
            instance = container.AddComponent(typeof(SceneController)) as SceneController;
        }

        return instance;
    }

    public void ChangeScene(string _name)
    {
        SceneManager.LoadScene(_name);
    }

    public void ChangeScene(SCENESTATE _state)
    {
        SceneManager.LoadScene(_state.ToString());
    }
}