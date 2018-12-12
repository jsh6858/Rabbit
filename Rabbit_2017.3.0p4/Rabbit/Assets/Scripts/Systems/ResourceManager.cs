using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ResourceManager : MonoBehaviour
{
    private static ResourceManager instance = null;
    private static GameObject container;

    public static ResourceManager GetInstance()
    {
        if(!instance)
        {
            container = new GameObject();
            container.name = "ResourceManager";
            instance = container.AddComponent(typeof(ResourceManager)) as ResourceManager;
        }

        return instance;
    }

    //public InGameManager inGameMgr = null;

    public Transform monsterBox;
    public Transform monsterAliveBox;
    public Transform effectBox;

    public Dictionary<string,GameObject> dicMonster = new Dictionary<string,GameObject>();
    public Dictionary<string,GameObject> dicEffect = new Dictionary<string,GameObject>();

    public Dictionary<string,Queue<GameObject>> effectDic = new Dictionary<string,Queue<GameObject>>();
    public Dictionary<string,Queue<GameObject>> monsterDic = new Dictionary<string,Queue<GameObject>>();


    public Dictionary<string,AudioClip> audioDic = new Dictionary<string,AudioClip>();

    private void Awake()
    {
        DontDestroyOnLoad(this);
        ////inGameMgr = InGameManager.instance;

        ////bulletBox = this.transform.Find("BulletBox").transform;
        //monsterBox = this.transform.Find("MonsterBox").transform;
        //monsterAliveBox = this.transform.Find("MonsterAliveBox").transform;
        //effectBox = this.transform.Find("EffectBox").transform;

        //// Monster 셋팅.
        //object[] monsterObj = Resources.LoadAll("Prefabs/Monster");
        //for(int i = 0;i < monsterObj.Length;i++)
        //{
        //    GameObject obj = monsterObj[i] as GameObject;
        //    obj.SetActive(false);
        //    dicMonster.Add(obj.name,obj);
        //}

        //// Effect 셋팅.
        //object[] effectObj = Resources.LoadAll("Prefabs/Effect");
        //for(int i = 0;i < effectObj.Length;i++)
        //{
        //    GameObject obj = effectObj[i] as GameObject;
        //    obj.SetActive(false);
        //    dicEffect.Add(obj.name,obj);
        //}

        // audio 셋팅.

        List<AudioClip> audioList = new List<AudioClip>();

        audioList.AddRange(Resources.LoadAll<AudioClip>("Sounds/Effects"));
        audioList.AddRange(Resources.LoadAll<AudioClip>("Sounds/Musics"));

        foreach(AudioClip clip in audioList)
        {
            //Debug.Log(clip.name);

            audioDic.Add(clip.name,clip);
        }

        Resources.UnloadUnusedAssets();
    }

    public AudioClip GetSoundResource(string _audioName)
    {
        if(audioDic.ContainsKey(_audioName))
        {
            return audioDic[_audioName];
        }
        else
        {
            return null;
        }
    }


    void Update()
    {
        //inGameMgr.observeMainCam.transform.Find("MonsterPoolLog").GetChild(0).GetComponent<TextMesh>().text = String.Format("On:{0:D3}\nOff:{1:D3}\nTot:{2:D3}", monsterAliveBox.childCount, monsterBox.childCount, monsterAliveBox.childCount + monsterBox.childCount);
        //inGameMgr.observeMainCam.transform.Find("EffectPoolLog").GetChild(0).GetComponent<TextMesh>().text = effectBox.childCount.ToString();
    }

    //public GameObject CreateEffectObj(string name,Vector3 pos,float playTime = 0.0f)
    //{
    //    GameObject targetObj = null;
    //    Queue<GameObject> targetQueue = new Queue<GameObject>();

    //    if(effectDic.ContainsKey(name))
    //        targetQueue = effectDic[name];
    //    else
    //        effectDic.Add(name,targetQueue);

    //    if(targetQueue.Count > 0)
    //    {
    //        targetObj = targetQueue.Dequeue();
    //        targetObj.transform.parent = effectBox;
    //        targetObj.GetComponent<GameEffect>().targetQueue = targetQueue;
    //        targetObj.GetComponent<GameEffect>().playTime = playTime;
    //    }
    //    else
    //    {
    //        targetObj = Instantiate(dicEffect[name]) as GameObject;
    //        targetObj.transform.parent = effectBox;
    //        targetObj.GetComponent<GameEffect>().targetQueue = targetQueue;
    //        targetObj.GetComponent<GameEffect>().playTime = playTime;
    //    }

    //    targetObj.transform.position = pos;

    //    return targetObj;
    //}
}
