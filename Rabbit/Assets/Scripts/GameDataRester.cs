using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDataRester : MonoBehaviour
{
	void Start ()
    {
        GameScene.GameDataBase.GetInstance().nowStage = 1;
        GameScene.GameDataBase.GetInstance().totalScore = 0;
	}
}
