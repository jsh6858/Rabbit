using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

public enum GAMESTATE
{
    Ready_State,
    Draw_State,
    Submit_State,
}

public class InGameManager : MonoBehaviour
{
    private static InGameManager instance = null;

    public static InGameManager Getinstance()
    {
        instance = FindObjectOfType(typeof(InGameManager)) as InGameManager;

        if(instance != null)
        {
            return instance;
        }
        else
        {
            return null;
        }
    }

    public GestureManager gestureManager = null;
    public SoundManager soundManager = null;
    public bool isSubmit = false;
    public bool isHint = false;
    public bool isStageClear = false;
    private float fadeTimer = 50.0f;
    public GAMESTATE nowState = GAMESTATE.Ready_State;

    void Start()
    {
        nowState = GAMESTATE.Ready_State;
        NextState();

        soundManager = SoundManager.instance;
        soundManager.PlayMusic(BackgroundMusic.GameScene_Music);
    }

    void NextState()
    {
        //다음 스테이트를 시작함
        string methodName = "Play"+nowState.ToString().Substring(0,nowState.ToString().IndexOf('_'));

        MethodInfo info = GetType().GetMethod(methodName,BindingFlags.NonPublic | BindingFlags.Instance);
        StopCoroutine((IEnumerator) info.Invoke(this,null));
        StartCoroutine((IEnumerator) info.Invoke(this,null));
    }

    IEnumerator PlayReady()
    {
        // 게임 기본 세팅
        GameDataBase.GetInstance().LoadGusture();

        gestureManager.nowGesture = GameDataBase.GetInstance().GetGeusture();

        yield return StartCoroutine(gestureManager.SetPointGroup());

        nowState = GAMESTATE.Draw_State;
            
        yield return StartCoroutine(CameraFadeIn());

        NextState();
    }

    IEnumerator PlayDraw()
    {
        gestureManager.isDrawable = true;
            
        nowState = GAMESTATE.Submit_State;
            
        while(!isSubmit)
        {
            if(isHint)
            {
                isHint = false;
                gestureManager.ShowHint();
            }

            yield return null;
        }         

        NextState();
    }

    IEnumerator PlaySubmit()
    {
        int nowScore = gestureManager.Recognize();
        
        soundManager.PlayEffect(SoundEffect.Drum_Effect);

        string nextScene;

        if(nowScore < GameDataBase.GetInstance().cutlineScore[GameDataBase.GetInstance().nowStage-1])
        {
            // 실패!!
            nextScene = "BadEndingScene";
        }
        else
        {
            // 성공!! 다음 스테이지 ㄱㄱ
            GameDataBase.GetInstance().AddGameData(nowScore,1);

            if(5 < GameDataBase.GetInstance().nowStage)
                nextScene = "HappyEndingScene";
            else
            {
                nextScene = "GameScene";
            }
        }

        int effectCount = 2;
        
        for(int i=0;i<effectCount;i++)
        {
            yield return new WaitForSeconds(0.1f);

            GameObject ldrumObject = ResourceManager.GetInstance().GetEffect("DrumText");

            ldrumObject.transform.position =  new Vector3(Random.Range(-1.5f,0.0f),Random.Range(-3.5f,3.0f),0.0f);
            ldrumObject.transform.rotation =  Quaternion.Euler(Vector3.forward*Random.Range(-30.0f,30.0f));

            yield return new WaitForSeconds(0.4f);

            GameObject rdrumObject = ResourceManager.GetInstance().GetEffect("DrumText");

            rdrumObject.transform.position =  new Vector3(Random.Range(0.0f,-1.5f),Random.Range(-3.5f,3.0f),0.0f);
            rdrumObject.transform.rotation =  Quaternion.Euler(Vector3.forward*Random.Range(-30.0f,30.0f));

            yield return new WaitForSeconds(0.3f);
        }
        
        yield return new WaitForSeconds(1.5f);

        yield return StartCoroutine(gestureManager.ShowSampleLine());

        yield return new WaitForSeconds(1.0f);
            
        // 필요한 엔딩 이미지 ㄱㄱ
        // 점수 출력

        GameObject.Find("ScorePrint").transform.Find("Text").GetComponent<Text>().text = nowScore.ToString() + "점";

        yield return new WaitForSeconds(2.0f);

        yield return StartCoroutine(CameraFadeOut());

        SceneController.GetInstance().ChangeScene(nextScene);
    }

    IEnumerator CameraFadeIn()
    {
        SpriteRenderer image = Camera.main.transform.Find("FadeFillter").GetComponent<SpriteRenderer>();

        for(int i=0;i<fadeTimer;i++)
        {
            image.color = Color.Lerp(Color.black,Color.clear,i/(fadeTimer-1));
            yield return null;
        }
    }

    IEnumerator CameraFadeOut()
    {
        SpriteRenderer image = Camera.main.transform.Find("FadeFillter").GetComponent<SpriteRenderer>();

        for(int i=0;i<=fadeTimer;i++)
        {
            image.color = Color.Lerp(Color.clear,Color.black,i/(fadeTimer-1));
            yield return null;
        }
    }
}