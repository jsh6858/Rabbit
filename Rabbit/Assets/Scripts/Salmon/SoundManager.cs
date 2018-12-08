using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sound_Manager
{
    public static Sound_Manager m_instance;

    public static Sound_Manager GetInstance()
    {
        if (null == m_instance)
            m_instance = new Sound_Manager();

        return m_instance;
    }

    private AudioSource _myAudio;
    public AudioSource m_myAudio
    {
        get
        {
            if(null == _myAudio)
            {
                GameObject obj = Resources.Load("Prefabs/Audio") as GameObject;

                _myAudio = GameObject.Instantiate(obj).GetComponent<AudioSource>();
            }

            return _myAudio;
        }
    }

    Dictionary<string, AudioClip> _dicSound;
    Dictionary<string, AudioClip> m_dicSound
    {
        get
        {
            if(_dicSound == null)
            {
                _dicSound = new Dictionary<string, AudioClip>();

                AudioClip[] audios = Resources.LoadAll<AudioClip>("Sound");

                for (int i = 0; i < audios.Length; ++i)
                {
                    _dicSound.Add(audios[i].name, audios[i]);
                }
            }

            return _dicSound;
        }
    }

    public void PlaySound(string name)
    {
        if (!m_dicSound.ContainsKey(name))
            return;
        
        m_myAudio.PlayOneShot(m_dicSound[name]);
    }

    public void Stop_Sound()
    {
        m_myAudio.Stop();
    }
}


