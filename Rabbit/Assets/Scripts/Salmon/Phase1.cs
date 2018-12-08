using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Phase1 : MonoBehaviour {

    private void Awake()
    {
        GameScene.GameDataBase Instance = GameScene.GameDataBase.GetInstance();

        if(Instance != null)
        {
            int iScore = Instance.totalScore;

            transform.Find("Text").GetComponent<Text>().text = iScore.ToString();
        }
    }

    public IEnumerator Fade_Out()
    {
        transform.Find("InputField").gameObject.SetActive(false);

        Image back = transform.Find("Back").GetComponent<Image>();
        Text text = transform.Find("Text").GetComponent<Text>();
        
        while (true)
        {
            back.color -= new Color(0f, 0f, 0f, 1f) * Time.deltaTime / 5f;
            text.color -= new Color(0f, 0f, 0f, 1f) * Time.deltaTime;

            if (back.color.a < 0f)
                break;

            yield return null;
        }

        back.gameObject.SetActive(false);
        text.gameObject.SetActive(false);

        
    }
}
