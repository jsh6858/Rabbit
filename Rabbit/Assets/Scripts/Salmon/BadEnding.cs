using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BadEnding : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Sound_Manager.GetInstance().Stop_Sound();
        Sound_Manager.GetInstance().PlaySound("BadEnding");
    }
}
