using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TitleManager : MonoBehaviour
{
    public SoundManager soundManager = null;

    void Start()
    {
        soundManager = SoundManager.instance;
        soundManager.PlayMusic(BackgroundMusic.TitleScene_Music);
    }

    public void OnTitleButtonClicked()
    {
        SceneController.GetInstance().ChangeScene(SCENESTATE.IntroScene);
    }
}
