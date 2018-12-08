using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CreateDugu : MonoBehaviour {

    float fTime = 0.0f;
    public GameObject Prefab;
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
        fTime += Time.deltaTime;

        if(fTime >= 0.3f)
        {
            fTime = 0.0f;
            Vector3 vPosition = new Vector3(Random.Range(-100.0f, 100.0f), Random.Range(-200.0f, 200.0f), 0.0f);
            Debug.Log(vPosition);

            GameObject TempObject = Instantiate(Prefab, vPosition, Quaternion.identity);
            TempObject.transform.SetParent(GameObject.Find("Canvas").transform);
            TempObject.transform.localPosition = vPosition;
            TempObject.transform.localScale = new Vector3(3.0f, 1.0f, 1.0f);
        }
	}
}
