using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Ending : MonoBehaviour {
  // Use this for initialization
    void Start () {
        Invoke("of",5.0f);

        Sound_Manager.GetInstance().Stop_Sound();
        Sound_Manager.GetInstance().PlaySound("Happy Ending");
    }
	
    void of()
    {
        SceneManager.LoadScene("Ranking");
    }
}
