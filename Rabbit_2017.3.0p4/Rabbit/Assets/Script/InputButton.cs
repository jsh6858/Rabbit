using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InputButton : MonoBehaviour
{
    private Rect drawArea;
    bool ChangeButton = false;
    bool CheckTouchIn = false;
    public Sprite ChangeSprite;
    private Image ChangeImage;

    private GameScene.InGameManager inGameManager = null;
    public GameObject ScorePrintObject;

    void Start ()
    {
        inGameManager = GameScene.InGameManager.Getinstance();
        ChangeImage = GetComponent<Image>();

        BoxCollider2D box = inGameManager.gestureManager.transform.Find("DrawBox").GetComponent<BoxCollider2D>();

        drawArea = new Rect(new Vector2(box.transform.position.x-box.size.x/2.0f,box.transform.position.y-box.size.y/2.0f),box.size);

        ChangeImage.enabled = true;
        ChangeImage.transform.GetChild(0).gameObject.SetActive(false);
    }

	// Update is called once per frame
	void Update ()
    {
        if(Input.GetKeyDown(KeyCode.Mouse0))
        {
            if(drawArea.Contains(Camera.main.ScreenToWorldPoint(Input.mousePosition)))
            {
                CheckTouchIn = true;
            }
                
        }
        else if (Input.GetKeyUp(KeyCode.Mouse0) && CheckTouchIn)
        {
            ChangeButton = true;
            ChangeImage.enabled = false;
            ChangeImage.transform.GetChild(0).gameObject.SetActive(true);
        }
	}


    public void TouchButton()
    {
        if(ChangeButton)
        {
            // 제출 모양
            ScorePrintObject.SetActive(true);
            inGameManager.isSubmit = true;
        }
        else
        {
            // 힌트 모양
            inGameManager.isHint = true;
            //Sound_Manager.GetInstance().PlaySound("Hint");
        }
    }
}
