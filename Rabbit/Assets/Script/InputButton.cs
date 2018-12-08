using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputButton : MonoBehaviour
{

    bool ChangeButton = false;
    bool CheckTouchIn = false;
    public Sprite ChangeSprite;
    private Image ChangeImage;

    private GameScene.InGameManager inGameManager = null;

    // Use this for initialization
    void Start ()
    {
        inGameManager = GameScene.InGameManager.Getinstance();
        ChangeImage = GetComponent<Image>();
	}

	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if (Input.mousePosition.x > 110 && Input.mousePosition.y > 110 &&
                Input.mousePosition.x < 630 && Input.mousePosition.y < 1170)        // 임시 화면 범위 검사
                CheckTouchIn = true;
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0) && CheckTouchIn)
        {
            ChangeButton = true;
            ChangeImage.sprite = ChangeSprite;    // 이미지 제출로 변경
        }
	}


    public void TouchButton()
    {
        if(ChangeButton)
        {
            // 제출 모양
            inGameManager.isSubmit = true;
        }
        else
        {
            // 힌트 모양
        }
    }
}
