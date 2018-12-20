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

    public Dictionary<string,GameObject> effectDic = new Dictionary<string,GameObject>();
    public Dictionary<string,AudioClip> audioDic = new Dictionary<string,AudioClip>();

    private void Awake()
    {
        DontDestroyOnLoad(this);

        // prefab 셋팅.
        List<GameObject> effectList = new List<GameObject>();

        effectList.AddRange(Resources.LoadAll<GameObject>("Prefabs/Effects"));

        foreach(GameObject obj in effectList)
        {
            effectDic.Add(obj.name,obj);
        }

        // audio 셋팅.
        List<AudioClip> audioList = new List<AudioClip>();

        audioList.AddRange(Resources.LoadAll<AudioClip>("Sounds/Effects"));
        audioList.AddRange(Resources.LoadAll<AudioClip>("Sounds/Musics"));

        foreach(AudioClip clip in audioList)
        {
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

    public GameObject GetEffect(string name)
    {
        if(effectDic.ContainsKey(name))
        {
            return Instantiate(effectDic[name]) as GameObject;
        }
        else
        {
            return null;
        }
    }
}