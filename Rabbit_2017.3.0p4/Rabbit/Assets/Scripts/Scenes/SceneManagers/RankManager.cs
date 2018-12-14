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


        dataList.Add(GameDataBase.GetInstance().totalScore+"_"+inputText.text);

        for(int i=0;i<dataList.Count;i++)
        {
            string[] data = dataList[i].Split('_');

            rankList.Add(new RankInfo(data[1],int.Parse(data[0])));
        }
        
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
    }

    public void ReGame()
    {
        SceneController.GetInstance().ChangeScene(SCENESTATE.TitleScene);
    }
    
    struct RankInfo
    {
        public RankInfo(string _name,float _score)
        {
            userName = _name;
            score = _score;
        }

        public float score;
        public string userName;
    }
}
