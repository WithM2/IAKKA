using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OnGameManager : MonoBehaviour
{
    public BattleGameManager battleGameManager;

    int my_Max_HP; // 내 최대 HP 변수
    int your_Max_HP; // 상대 최대 HP 변수

    void OnEnable()
    {
        my_Max_HP = BattleGameManager.my_HP;       // 내 최대 HP 변수
        your_Max_HP = BattleGameManager.your_HP;  // 상대 최대 HP 변수
    }

    void Update()
    {
        if(BattleGameManager.my_HP <= 0){
            battleGameManager.Victory();
        }
        if(BattleGameManager.your_HP <= 0){
            battleGameManager.Lose();
        }
    }


    void OnDisable() // 게임 종료 시 능력치 초기갑 보존
    {
        BattleGameManager.my_HP = my_Max_HP;
        BattleGameManager.your_HP = your_Max_HP;
    }
}
