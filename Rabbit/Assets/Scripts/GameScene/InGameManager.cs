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
        public bool isStageClear = false;
        private float fadeTimer = 50.0f;
        public GAMESTATE nowState = GAMESTATE.Ready_State;
        //private int count = 0;

        void Start()
        {
            nowState = GAMESTATE.Ready_State;
            NextState();
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

            yield return new WaitUntil(()=>isSubmit);            

            NextState();
        }

        IEnumerator PlaySubmit()
        {
            int nowScore = gestureManager.Recognize();

            Debug.Log(nowScore);

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

                if(5 <= GameDataBase.GetInstance().nowStage)
                    nextScene = "HappyEndingScene";
                else
                {
                    nextScene = "GameScene" + GameDataBase.GetInstance().nowStage.ToString();
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
           
            NextState();
        }

        IEnumerator CameraFadeIn()
        {
            GameObject canvas = GameObject.Find("Canvas");
            Image image = canvas.transform.Find("FadeFillter").GetComponent<Image>();

            for(int i = 0;i<fadeTimer;i++)
            {
                image.color = Color.Lerp(Color.black,Color.clear,i/(fadeTimer-1));
                yield return null;
            }
        }

        IEnumerator CameraFadeOut()
        {
            GameObject canvas = GameObject.Find("Canvas");
            Image image = canvas.transform.Find("FadeFillter").GetComponent<Image>();

            for(int i = 0;i<=fadeTimer;i++)
            {
                image.color = Color.Lerp(Color.clear,Color.black,i/(fadeTimer-1));
                yield return null;
            }
        }
    }
}