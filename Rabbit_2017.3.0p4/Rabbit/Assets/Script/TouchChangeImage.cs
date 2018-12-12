using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TouchChangeImage : MonoBehaviour 
{
    public GameObject[] introArray;

    private float timer = 0.0f;

    void Start ()
    {
        foreach(GameObject obj in introArray)
        {
            obj.SetActive(false);
        }

        StartCoroutine(ChangeImage());
    }

    IEnumerator ChangeImage()
    {
        introArray[0].SetActive(true);

        yield return new WaitUntil(()=>Input.GetKeyDown(KeyCode.Mouse0) || timer > 4.0f);
        yield return null;

        introArray[0].SetActive(false);
        introArray[1].SetActive(true);

        timer = 0.0f;

        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0) || timer > 4.0f);
        yield return null;

        timer = 0.0f;

        introArray[1].SetActive(false);
        introArray[2].SetActive(true);
        
        yield return new WaitUntil(() => Input.GetKeyDown(KeyCode.Mouse0) || timer > 4.0f);
        yield return null;

        timer = 0.0f;

        //Sound_Manager.GetInstance().Stop_Sound();
        SceneManager.LoadScene("GameScene");
    }

    void FixedUpdate()
    {
        timer += Time.fixedDeltaTime;
    }
}
