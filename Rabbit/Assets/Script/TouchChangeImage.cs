using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TouchChangeImage : MonoBehaviour {

    public Texture[] TextureArray;
    public bool IsChangeSceneScript;
    public string ChangeSceneName;
    private RawImage ImageTexture;
    private int MaxSize = 0;
    private int TouchCount = 0;
    private int OldTouchCount = -1;
    void Start () {
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
                GetComponent<SceneChange>().ChangeScene(ChangeSceneName);
                return;
            }
        }

        if(OldTouchCount != TouchCount)
        {
            OldTouchCount = TouchCount;
            ImageTexture.texture = TextureArray[TouchCount];
        }
    }
}
