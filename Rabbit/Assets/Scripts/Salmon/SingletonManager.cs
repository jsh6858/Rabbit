using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton_Manager
{
    private static Singleton_Manager m_instance;

    public static Singleton_Manager GetInstance()
    {
        if (null == m_instance)
            m_instance = new Singleton_Manager();

        return m_instance;
    }

    public RankManager _rankManager;
}
