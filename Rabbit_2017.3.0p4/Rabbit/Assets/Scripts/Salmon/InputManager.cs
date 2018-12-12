using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputManager : MonoBehaviour {

    Text m_input;

    private void Awake()
    {
        m_input = transform.Find("Text").GetComponent<Text>();
    }

    public void Input_Name()
    {
        GameScene.GameDataBase Instance = GameScene.GameDataBase.GetInstance();

        int iScore = 0;
        if (Instance != null)
        {
            iScore = Instance.totalScore;

            transform.Find("Text").GetComponent<Text>().text = iScore.ToString();
        }

        string name = m_input.text;

        Singleton_Manager.GetInstance()._rankManager.Add_Rank(name, (float)iScore);
    }
}
