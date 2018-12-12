using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IntroManager : MonoBehaviour
{
    public SoundManager soundManager = null;
    public Transform imageArray = null;
    private float skipTimer = 0.0f;

    void Start()
    {
        soundManager = SoundManager.instance;
        soundManager.PlayMusic(BackgroundMusic.IntroScene_Music);

        GameScene.GameDataBase.GetInstance().nowStage = 1;
        GameScene.GameDataBase.GetInstance().totalScore = 0;

        for(int i=0;i<imageArray.childCount;i++)
        {
            imageArray.GetChild(i).gameObject.SetActive(false);
        }

        StartCoroutine(ChangeImage());
    }

    IEnumerator ChangeImage()
    {
        for(int i=0;i<imageArray.childCount;i++)
        {
            skipTimer = 0.0f;
            imageArray.GetChild(i).gameObject.SetActive(true);
            
            yield return new WaitUntil(() => Input.GetMouseButtonDown(0) || skipTimer > 3.0f);
            yield return null;

            imageArray.GetChild(i).gameObject.SetActive(false);
        }

        SceneController.GetInstance().ChangeScene(SCENESTATE.GameScene);
    }

    void FixedUpdate()
    {
        skipTimer += Time.fixedDeltaTime;
    }
}
