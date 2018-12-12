using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadEndingManager : MonoBehaviour
{
    public SoundManager soundManager = null;

    void Start()
    {
        soundManager = SoundManager.instance;
        soundManager.PlayMusic(BackgroundMusic.BadEndingScene_Music);

        GameScene.GameDataBase.GetInstance().nowStage = 1;
        GameScene.GameDataBase.GetInstance().totalScore = 0;
    }

    public void OnEndingButtonClicked()
    {
        SceneController.GetInstance().ChangeScene(SCENESTATE.GameScene);
    }
}
