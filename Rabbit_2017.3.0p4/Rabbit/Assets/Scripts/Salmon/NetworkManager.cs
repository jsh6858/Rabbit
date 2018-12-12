using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NetworkManager : MonoBehaviour {
    
    public string url = "http://images.earthcam.com/ec_metros/ourcams/fridays.jpg";
    
    IEnumerator Start()
    {
        Debug.Log("hi");

        WWW www = new WWW(url);
        yield return www;
        SpriteRenderer renderer = GetComponent<SpriteRenderer>();
        renderer.material.mainTexture = www.texture;
    }
}

