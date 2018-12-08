using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public struct RankInfo
{
    public RankInfo(string _name, float _score, bool _mine = false)
    {
        this.name = _name;
        this.fScore = _score;
        mine = _mine;
    }

    public float fScore;
    public string name;
    public bool mine;
}

public class RankManager : MonoBehaviour {

    readonly int INITIAL_POSY = 315;
    readonly int GAP_Y = 73;

    public List<RankInfo> m_listRankInfo = new List<RankInfo>();

    public GameObject _rankLine;
    public GameObject rankLine
    {
        get
        {
            if (_rankLine == null)
                _rankLine = Resources.Load( "Prefabs/Rank/RankLine") as GameObject;
            return _rankLine;
        }
    }
  
    private void Awake()
    {
        Singleton_Manager.GetInstance()._rankManager = this;

        Load();
    }

    public void Add_Rank(string name, float fScore)
    {
        StartCoroutine(GameObject.Find("Phase1").GetComponent<Phase1>().Fade_Out());

        m_listRankInfo.Add(new RankInfo(name, fScore, true));

        Sort_Rank();

        Update_Rank();

        Save();
    }

    private void Sort_Rank()
    {
        m_listRankInfo.Sort((x, y) => y.fScore.CompareTo(x.fScore));

        if(m_listRankInfo.Count == 11)
        {
            m_listRankInfo.RemoveAt(10);
        }
    }

    private void Update_Rank()
    {
        Transform trLine = transform.Find("RankLine");
        int iChildCnt = trLine.childCount;

        for (int i=0; i<m_listRankInfo.Count; ++i)
        {
            if(i >= iChildCnt)
            {
                GameObject obj = GameObject.Instantiate(rankLine) as GameObject;

                obj.transform.SetParent(trLine);
                obj.transform.localPosition = new Vector3(0f, INITIAL_POSY - GAP_Y * i, 0f);
            }

            trLine.GetChild(i).GetComponent<RankLine>().Set_Rank(i+1, m_listRankInfo[i].name, m_listRankInfo[i].fScore, m_listRankInfo[i].mine);
        }
    }

    public void ReGame()
    {
        // Title로

        SceneManager.LoadScene("Logo");
    }

    private void Save()
    {
        PlayerPrefs.DeleteAll();

        for(int i=0; i<m_listRankInfo.Count; ++i)
        {
            PlayerPrefs.SetString("name" + i, m_listRankInfo[i].name);
            PlayerPrefs.SetFloat("score" + i, m_listRankInfo[i].fScore);
        }
    }

    private void Load()
    {
        int i = 0;

        while(true)
        {
            if (!PlayerPrefs.HasKey("name" + i) || !PlayerPrefs.HasKey("score" + i))
                break;

            m_listRankInfo.Add(new RankInfo(PlayerPrefs.GetString("name" + i), PlayerPrefs.GetFloat("score" + i)));
            Sort_Rank();
            Update_Rank();
            
            i++;
        }
    }

    // 더미 데이터
    private void Update()
    {
        return;

        if (Input.GetKeyDown(KeyCode.A))
        {
            Add_Rank("abc", Random.Range(0, 100));
        }
    }

    public void Cheat()
    {
        PlayerPrefs.DeleteAll();
    }
}
