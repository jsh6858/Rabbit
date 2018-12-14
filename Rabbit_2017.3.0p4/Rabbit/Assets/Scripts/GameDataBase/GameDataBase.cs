using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class GameDataBase : MonoBehaviour
{
    private static GameDataBase instance = null;
    private static GameObject container;

    public static GameDataBase GetInstance()
    {
        if(!instance)
        {
            container = new GameObject();
            container.name = "GameDataBase";
            instance = container.AddComponent(typeof(GameDataBase)) as GameDataBase;
        }

        return instance;
    }

    public Dictionary<string,Vector2[]> gestureSampleDictionary = new Dictionary<string,Vector2[]>();
    public Dictionary<string,string> convertText = new Dictionary<string,string>();
    public List<GestureData> gustureList = new List<GestureData>();

    // GameData
    public int totalScore = 0;
    public int nowStage = 1;
    public string[] rankArray = null;


    private string gameDataPath = null;

    public int[] cutlineScore = new int[] { 5,10,15,20,25 };
    public float pointPercent = 0.4f;
    public int hintCount = 5;
    public int minusScore = 10;

    void Awake()
    {
        convertText.Add("Star","별");
        convertText.Add("Home","집");
        convertText.Add("Aries","양자리");
        convertText.Add("Circle","원");
        convertText.Add("Fish","물고기");
        convertText.Add("Glasses","안경");
        convertText.Add("ToothBrush","칫솔");
        convertText.Add("LegoHead","레고케빈");
        convertText.Add("RavitCap","토끼모자");

        DontDestroyOnLoad(this);

        if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            gameDataPath = Application.persistentDataPath;
        }
        else if(Application.platform == RuntimePlatform.WindowsPlayer)
        {
            gameDataPath = Application.dataPath;
        }
        else if(Application.platform == RuntimePlatform.WindowsEditor)
        {
            gameDataPath = Application.dataPath.Substring(0,Application.dataPath.LastIndexOf('/'));
        }

        gameDataPath+="/Rabbit.koz";

        LoadDataBase();
    }

    public void LoadGusture()
    {
        gestureSampleDictionary.Clear();
        gustureList.Clear();

        Object[] JSONObjects = Resources.LoadAll("Texts/"+nowStage+"/");
        TextAsset[] textArray = new TextAsset[JSONObjects.Length];

        for(int i = 0;i<JSONObjects.Length;i++)
        {
            textArray[i] = (TextAsset) JSONObjects[i];
        }

        foreach(TextAsset ta in textArray)
        {
            GestureData gImage = LitJson.JsonMapper.ToObject<GestureData>(ta.text);
            Vector2[] pointArray = System.Array.ConvertAll(gImage.vecPointArray,data => new Vector2(float.Parse(data.Split('_')[0]),float.Parse(data.Split('_')[1])));

            gestureSampleDictionary.Add(gImage.strName,pointArray);
            gustureList.Add(gImage);
        }
    }

    public void SetGameData(int _score = 0,int _stage = 1)
    {
        totalScore = _score;
        nowStage = _stage;

        SaveGameData();
    }

    public void AddGameData(int _score,int _stage)
    {
        totalScore += _score;
        nowStage += _stage;

        SaveGameData();
    }

    void LoadDataBase()
    {
        if(File.Exists(gameDataPath))
        {
            BinaryFormatter binary = new BinaryFormatter();
            FileStream file = new FileStream(gameDataPath,FileMode.Open);
            string _strJson = binary.Deserialize(file) as string;
            //StreamReader sr = new StreamReader(file);
            //string str = sr.ReadToEnd();
            //string _strJson = binary.Deserialize(str) as string;

            GameData data = LitJson.JsonMapper.ToObject<GameData>(_strJson);

            nowStage = int.Parse(data.iNowStage);
            totalScore = int.Parse(data.iTotalScore);
            rankArray = data.strRankDataArray;

            file.Close();
        }
    }

    public void SaveGameData()
    {
        BinaryFormatter binary = new BinaryFormatter();
        FileStream file = new FileStream(gameDataPath,FileMode.Create);
        //StreamWriter sw = new StreamWriter(gameDataPath+".json");
        
        GameData data = new GameData();

        data.iNowStage = nowStage.ToString();
        data.iTotalScore = totalScore.ToString();
        data.strRankDataArray = rankArray;
        
        string strJson = JsonFx.Json.JsonWriter.Serialize(data);

        //string gameData = nowStage+"_"+totalScore;

        //sw.WriteLine(strJson);

        //sw.Close();
        binary.Serialize(file,strJson);
        file.Close();
    }

    public string GetConvert(string _str)
    {
        return convertText[_str];
    }

    string ReadStringFromFile(string filename)
    {
        string path = PathForDocumentsFile(filename);

        if(File.Exists(path))
        {
            FileStream file = new FileStream(path,FileMode.Open,FileAccess.Read);
            StreamReader sr = new StreamReader(file);

            string str = null;
            str = sr.ReadToEnd();

            sr.Close();
            file.Close();

            return str;
        }

        else
        {
            return null;
        }
    }

    public void WriteStringToFile(string str,string filename)
    {
        string path = PathForDocumentsFile(filename);
        FileInfo fileInfo = new FileInfo(path);

        if(Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer)
        {
            if(fileInfo.Exists)
            {
                return;
            }
            else
            {
                FileStream file = new FileStream(path,FileMode.Create,FileAccess.Write);
                StreamWriter sw = new StreamWriter(file);
                sw.WriteLine(str);

                sw.Close();
                file.Close();
            }
        }
        else
        {
            FileStream file = new FileStream(path,FileMode.Create,FileAccess.Write);
            StreamWriter sw = new StreamWriter(file);
            sw.WriteLine(str);

            sw.Close();
            file.Close();
        }
    }

    public string PathForDocumentsFile(string filename)
    {
        if(Application.platform == RuntimePlatform.IPhonePlayer)
        {
            string path = Application.dataPath.Substring(0,Application.dataPath.Length - 5);
            path = path.Substring(0,path.LastIndexOf('/'));
            return Path.Combine(Path.Combine(path,"Documents"),filename);
        }
        else if(Application.platform == RuntimePlatform.Android)
        {
            string path = Application.persistentDataPath;
            path = path.Substring(0,path.LastIndexOf('/'));
            return Path.Combine(path,filename);
        }
        else
        {
            string path = Application.dataPath;
            path = path.Substring(0,path.LastIndexOf('/'));
            return Path.Combine(path,filename);
        }
    }

    public GestureData GetGeusture()
    {
        return gustureList[Random.Range(0,gustureList.Count)];
    }
}

public struct GameData
{
    public string iNowStage { get; set; }
    public string iTotalScore { get; set; }
    public string[] strRankDataArray { get; set; }
}

public struct GestureData
{
    public string strName { get; set; }
    public string[] vecPointArray { get; set; }
}