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

        GameDataBase.GetInstance().SetGameData(0,1);
    }

    public void OnEndingButtonClicked()
    {
        SceneController.GetInstance().ChangeScene(SCENESTATE.GameScene);
    }
}