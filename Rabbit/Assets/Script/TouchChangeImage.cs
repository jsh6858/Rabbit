using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

<<<<<<< HEAD
public class TouchChangeImage : MonoBehaviour
{
=======
public class TouchChangeImage : MonoBehaviour {
>>>>>>> 969b19295627991dfb8570a39c844512579ba143
    public Texture[] TextureArray;
    public bool IsChangeSceneScript;
    public string ChangeSceneName;
    private RawImage ImageTexture;
    private int MaxSize = 0;
    private int TouchCount = 0;
    private int OldTouchCount = -1;
    public string[] IntroText;
    void Start ()
    {
        ImageTexture = GetComponent<RawImage>();
        MaxSize = TextureArray.Length;
    }
	
    void KeyCheck()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            ++TouchCount;
        }
    }
	// Update is called once per frame
	void Update () {
        KeyCheck();
        
        if(TouchCount >= MaxSize)
        {
            if (!IsChangeSceneScript)
                return;
            else
            {
                SceneManager.LoadScene(ChangeSceneName);
                return;
            }
        }

        if(OldTouchCount != TouchCount)
        {
            OldTouchCount = TouchCount;
            ImageTexture.texture = TextureArray[TouchCount];
            transform.Find("Text").GetComponent<Text>().text = IntroText[TouchCount];
        }
    }
}
