using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEffect : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(DecreaseAlpha());
    }

    IEnumerator DecreaseAlpha()
    {
        int alphCount = 20;
        SpriteRenderer image = GetComponent<SpriteRenderer>();

        for(int i=0;i<alphCount;i++)
        {
            image.color = Color.Lerp(Color.clear,Color.white,i/(float)(alphCount-1));

            yield return null;
        }

        for(int i=0;i<alphCount;i++)
        {
            image.color = Color.Lerp(Color.white,Color.clear,i/(float)(alphCount-1));

            yield return null;
        }

        image.color = Color.clear;
    }
}
