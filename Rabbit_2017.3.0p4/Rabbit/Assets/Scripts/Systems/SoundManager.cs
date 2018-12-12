using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum SoundEffect
{
    Drum_Effect = 0,
    Hint_Effect,
}

public enum BackgroundMusic
{
    BadEndingScene_Music = 0,
    HappyEndingScene_Music,
    IntroScene_Music,
    TitleScene_Music,
    GameScene_Music,
}

public class SoundManager : MonoBehaviour
{
    private static SoundManager s_instance = null;
    public static SoundManager instance
    {
        get
        {
            s_instance = FindObjectOfType(typeof(SoundManager)) as SoundManager;
            return (s_instance != null) ? s_instance : null;
        }
    }

    public GameObject soundEffectGroup = null;
    public GameObject soundMusic = null;

    public List<AudioSource> effectPoolList = new List<AudioSource>();
    private int poolLimit = 5;

    void Awake()
    {
        soundEffectGroup = new GameObject("SoundEffectGroup");
        soundEffectGroup.transform.parent = transform;

        soundMusic = new GameObject("SoundMusic");
        soundMusic.AddComponent<AudioSource>();
        soundMusic.transform.parent = transform;

        for(int i = 0;i<poolLimit;i++)
        {
            GameObject sound = new GameObject("SoundEffect");
            effectPoolList.Add(sound.AddComponent<AudioSource>());
            sound.transform.parent = soundEffectGroup.transform;
        }
    }

    public AudioSource PlayEffect(SoundEffect _effectName,bool _isLoop = false,float _volume = 1.0f)
    {
        int index = GetFirstIndex();

        if(index == -1)
            return null;
        
        AudioSource source = effectPoolList[index];
        source.clip = ResourceManager.GetInstance().GetSoundResource(_effectName.ToString());

        if(source.clip == null)
            return null;

        source.loop = _isLoop;
        source.volume = _volume;

        source.Play();

        return source;
    }

    int GetFirstIndex()
    {
        for(int i=0;i<effectPoolList.Count;i++)
        {
            if(!effectPoolList[i].isPlaying)
                return i;
        }

        return -1;
    }

    public AudioSource PlayMusic(BackgroundMusic _musicName,bool _isLoop = true,float _volume = 1.0f)
    {
        AudioSource source = soundMusic.GetComponent<AudioSource>();

        source.clip = ResourceManager.GetInstance().GetSoundResource(_musicName.ToString());
        source.loop = _isLoop;
        source.volume = _volume;

        source.Play();

        return source;
    }
}
