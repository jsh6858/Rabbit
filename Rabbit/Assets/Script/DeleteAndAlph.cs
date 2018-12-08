using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteAndAlph : MonoBehaviour {

    float AlphCount = 1.0f;
    private RawImage image;
	// Use this for initialization
	void Start () {
        image = GetComponent<RawImage>();
    }
	
	// Update is called once per frame
	void Update () {
        AlphCount -= Time.deltaTime;
        if(AlphCount <= 0.0f)
        {
            GameObject.Destroy(this);
            return;
        }
        image.color = Color.Lerp(Color.white , Color.clear, 1.0f - AlphCount);
    }
}
