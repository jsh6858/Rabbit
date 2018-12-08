using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace GameScene
{
    public class ButtonController : MonoBehaviour
    {
        private InGameManager inGameManager = null;

        void Awake()
        {
            inGameManager = InGameManager.Getinstance();
        }

        public void SubmitButton()
        {
            inGameManager.isSubmit = true;
        }
    }
}