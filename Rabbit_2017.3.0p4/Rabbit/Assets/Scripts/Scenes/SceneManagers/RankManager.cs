using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class RankManager : MonoBehaviour
{
    public Transform inputPanel = null;
    public Transform rankPanel = null;
    public Text scoreText = null;
    public Text inputText = null;

    public Transform rankLinePanel = null;

    private List<RankInfo> rankList = new List<RankInfo>();
    
    void Start()
    {
        inputPanel.gameObject.SetActive(true);
        rankPanel.gameObject.SetActive(false);
        scoreText = inputPanel.Find("Text").GetComponent<Text>();
        inputText = inputPanel.Find("InputField/Text").GetComponent<Text>();

        scoreText.text = GameDataBase.GetInstance().totalScore.ToString()+" 점";
    }

    public void WriteEnd()
    {
        List<string> dataList;

        if(GameDataBase.GetInstance().rankArray != null)
        {
            dataList = new List<string>(GameDataBase.GetInstance().rankArray);
        }
        else
        {
            dataList = new List<string>();
        }

        // 기존 데이터 전부 변형
        for(int i=0;i<dataList.Count;i++)
        {
            string[] data = dataList[i].Split('_');

            rankList.Add(new RankInfo(data[1],int.Parse(data[0])));
        }

        // 새로운 데이터
        rankList.Add(new RankInfo(inputText.text,GameDataBase.GetInstance().totalScore,1));

        rankList.Sort((x,y)=>y.score.CompareTo(x.score));

        if(rankList.Count > 10)
        {
            rankList.RemoveRange(10,rankList.Count-10);
        }

        string[] rank = new string[rankList.Count];

        for(int i=0;i<rankList.Count;i++)
        {
            rank[i] = rankList[i].score+"_"+rankList[i].userName;
        }

        GameDataBase.GetInstance().SetGameData(0,1);
        GameDataBase.GetInstance().rankArray = rank;
        GameDataBase.GetInstance().SaveGameData();

        inputPanel.gameObject.SetActive(false);
        rankPanel.gameObject.SetActive(true);


        // 정리
        for(int i=0;i<rankLinePanel.childCount;i++)
        {
            if( i<rankList.Count)
            {
                rankLinePanel.Find("Rank_"+(i+1)).gameObject.SetActive(true);
                rankLinePanel.Find("Rank_"+(i+1)+"/Name").GetComponent<Text>().text = rankList[i].userName;
                rankLinePanel.Find("Rank_"+(i+1)+"/Score").GetComponent<Text>().text = rankList[i].score.ToString();

                if(rankList[i].index == 1)
                {
                    // 신규 데이터 이므로 강조
                    StartCoroutine(ShowEffect(rankLinePanel.Find("Rank_"+(i+1))));
                }

            }
            else
            {
                rankLinePanel.Find("Rank_"+(i+1)).gameObject.SetActive(false);
            }
        }
    }

    IEnumerator ShowEffect(Transform _object)
    {
        bool small = false;

        while(true)
        {
            if(_object.localScale.x > 1.2f)
                small = true;
            if(_object.localScale.x < 1.0f)
                small = false;

            if(small)
                _object.localScale -= Vector3.one*Time.deltaTime/5.0f;
            else
                _object.localScale += Vector3.one*Time.deltaTime/5.0f;
            
            yield return null;
        }
    }

    public void ReGame()
    {
        SceneController.GetInstance().ChangeScene(SCENESTATE.TitleScene);
    }
    
    struct RankInfo
    {
        public RankInfo(string _name,float _score,int _index = 0)
        {
            userName = _name;
            score = _score;
            index = _index;
        }

        public float score;
        public string userName;
        public int index;
    }
}
