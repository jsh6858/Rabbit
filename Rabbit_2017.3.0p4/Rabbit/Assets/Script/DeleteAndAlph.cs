using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DeleteAndAlph : MonoBehaviour
{
    float AlphCount = 1.0f;
    private SpriteRenderer image;

	void Start ()
    {
        image = GetComponent<SpriteRenderer>();
    }

	void Update ()
    {
        AlphCount -= Time.deltaTime;

        if(AlphCount <= 0.0f)
        {
            GameObject.Destroy(this);
            return;
        }

        image.color = Color.Lerp(Color.white , Color.clear, 1.0f - AlphCount);
    }
}
