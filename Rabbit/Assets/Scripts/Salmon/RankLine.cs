using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankLine : MonoBehaviour {
    
    public bool m_bMine = false;
    bool m_bSmall = false;

    private void Awake()
    {
        _Rank = transform.Find("Rank").GetComponent<Text>();
        _name = transform.Find("Name").GetComponent<Text>();
        _Score = transform.Find("Score").GetComponent<Text>();
    }

    Text _Rank;
    Text _name;
    Text _Score;

    public void Set_Rank(int rank, string name, float score, bool bMine = false)
    {
        _Rank.text = rank.ToString();
        _name.text = name;
        _Score.text = ((int)score).ToString();

        if (rank == 1)
        {
            _Rank.color = new Color32(205, 0, 0, 255);
            _name.color = new Color32(205, 0, 0, 255);
            _Score.color = new Color32(205, 0, 0, 255);
        }
        else
        {
            _Rank.color = Color.black;
            _name.color = Color.black;
            _Score.color = Color.black;
        }

        m_bMine = bMine;
    }

    private void Update()
    {
        if (!m_bMine)
            return;

        if (transform.localScale.x > 1f)
            m_bSmall = true;
        if (transform.localScale.x < 0.9f)
            m_bSmall = false;

        if(m_bSmall)
            transform.localScale -= new Vector3(1f, 1f, 1f) * Time.deltaTime / 5f;
        else
            transform.localScale += new Vector3(1f, 1f, 1f) * Time.deltaTime / 5f;
    }
}
