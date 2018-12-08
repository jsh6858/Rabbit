using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace GameScene
{    
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
        public bool isSubmit = false;
        public bool isHint = false;
        public bool isStageClear = false;
        private float fadeTimer = 50.0f;
        public GAMESTATE nowState = GAMESTATE.Ready_State;

        void Start()
        {
            nowState = GAMESTATE.Ready_State;
            NextState();
            
            if(!Sound_Manager.GetInstance().IsPlaying())
                Sound_Manager.GetInstance().PlaySound("In_Game");
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

            yield return StartCoroutine(gestureManager.ShowMainPoint());

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

            Debug.Log(nowScore);

            Sound_Manager.GetInstance().PlaySound("Drum");

            string nextScene;

            if(nowScore < GameDataBase.GetInstance().cutlineScore[GameDataBase.GetInstance().nowStage])
            {
                // 실패!!
                nextScene = "BadEndingScene";
            }
            else
            {
                // 성공!! 다음 스테이지 ㄱㄱ
                GameDataBase.GetInstance().totalScore += nowScore;
                GameDataBase.GetInstance().nowStage++;

                if(5 < GameDataBase.GetInstance().nowStage)
                    nextScene = "HappyEndingScene";
                else
                {
                    nextScene = "GameScene";
                }
            }


            // 필요한 엔딩 이미지 ㄱㄱ

            // 다음 씬 ㄱㄱ

            yield return new WaitForSeconds(2.0f);
            // 점수 출력
            GameObject.Find("ScorePrint").transform.Find("Text").GetComponent<Text>().text = nowScore.ToString() + "점";
            yield return new WaitForSeconds(2.0f);
            yield return StartCoroutine(CameraFadeOut());

            GameObject.Find("Canvas").GetComponent<SceneChange>().ChangeScene(nextScene);
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
}