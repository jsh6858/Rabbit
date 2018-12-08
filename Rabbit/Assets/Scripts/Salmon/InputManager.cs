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
        int iScore = Random.Range(0, 500);

        string name = m_input.text;

        Singleton_Manager.GetInstance()._rankManager.Add_Rank(name, (float)iScore);
    }
}
