﻿using System.Collections;
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
        
        for(int i=0;i<imageArray.childCount;i++)
        {
            imageArray.GetChild(i).gameObject.SetActive(false);
        }

        SceneController.GetInstance().LoadGameScene();

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

        SceneController.GetInstance().ChangeGameScene();
    }

    void FixedUpdate()
    {
        skipTimer += Time.fixedDeltaTime;
    }
}