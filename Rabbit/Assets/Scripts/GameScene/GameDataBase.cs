using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

namespace GameScene
{
    public class GameDataBase :MonoBehaviour
    {
        private static GameDataBase s_instance = null;
        public static GameDataBase instance
        {
            get
            {
                s_instance = FindObjectOfType(typeof(GameDataBase)) as GameDataBase;
                return (s_instance != null) ? s_instance : null;
            }
        }

        public Dictionary<string,Vector2[]> gestureSampleDictionary = new Dictionary<string,Vector2[]>();
        public List<GestureData> gustureList = new List<GestureData>();

        public int nowScore = 0;
        public int nowStage = 1;
        
        void Awake()
        {
            DontDestroyOnLoad(this);

            Object[] JSONObjects = Resources.LoadAll("Texts/"+nowStage+"/");
            TextAsset[] textArray = new TextAsset[JSONObjects.Length];

            for(int i = 0;i<JSONObjects.Length;i++)
            {
                textArray[i] = (TextAsset) JSONObjects[i];
            }

            foreach(TextAsset ta in textArray)
            {
                GestureData gImage = LitJson.JsonMapper.ToObject<GestureData>(ta.text);
                Vector2[] pointArray = System.Array.ConvertAll(gImage.vecPointArray,data=>new Vector2(float.Parse(data.Split('_')[0]),float.Parse(data.Split('_')[1])));

                gestureSampleDictionary.Add(gImage.strName,pointArray);                
                gustureList.Add(gImage);
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
}