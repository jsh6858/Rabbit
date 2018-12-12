using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

namespace GameScene
{
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

        public int totalScore = 0;
        public int nowStage = 1;

        private OperateZone opZoneSetting = new OperateZone();

        public int[] cutlineScore;
        public float pointPercent;
        public int hintCount;
        public int minusScore;

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

            LoadOperater();
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

        public string GetConvert(string _str)
        {
            return convertText[_str];
        }

        void LoadOperater()
        {
            string path = "OperateZone.json";
            string _strJson = ReadStringFromFile(path);

            if(_strJson != null)
            {
                opZoneSetting = LitJson.JsonMapper.ToObject<OperateZone>(_strJson);
            }
            else
            {
                opZoneSetting.iCutlineScore = new string[] { "5","10","15","20","25" };
                opZoneSetting.fpointPercent = "0.4";
                opZoneSetting.iHintCount = "5";
                opZoneSetting.iMinusScore = "10";

                // json을 만들어 저장한다.
                string strJson = JsonFx.Json.JsonWriter.Serialize(opZoneSetting);

                WriteStringToFile(strJson,path);
            }
            cutlineScore = System.Array.ConvertAll(opZoneSetting.iCutlineScore,str=>int.Parse(str));
            pointPercent = float.Parse(opZoneSetting.fpointPercent);
            hintCount = int.Parse(opZoneSetting.iHintCount);
            minusScore = int.Parse(opZoneSetting.iMinusScore);
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

    public class GestureData
    {
        public string strName { get; set; }
        public string[] vecPointArray { get; set; }
    }

    public class OperateZone
    {
        public string[] iCutlineScore { get; set; }        //int      - 실패 점수
        public string fpointPercent { get; set; }           //float    - 포인트 퍼센트
        public string iHintCount { get; set; }             //int      - 힌트 갯수
        public string iMinusScore { get; set; }            //int      - 감점(힌트 사용시)
    }
}