using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HappyEndingManager : MonoBehaviour
{
    public SoundManager soundManager = null;

    void Start()
    {
        soundManager = SoundManager.instance;
        soundManager.PlayMusic(BackgroundMusic.HappyEndingScene_Music);
    }

    public void OnEndingButtonClicked()
    {
        SceneController.GetInstance().ChangeScene(SCENESTATE.RankingScene);
    }
}