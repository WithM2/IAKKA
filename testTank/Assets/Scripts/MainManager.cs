using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft; //반 시계방향으로 회전
    }

    //버튼 클릭 관련 코드
    public void StartClicked()
    {
        GameScencesMove.Instance.MoveTo_Battle_Game();
    }
    public void UpgradeClicked()
    {
        GameScencesMove.Instance.MoveTo_QuizScene();
    }
    public void LogoutClicked()
    {
        GameScencesMove.Instance.MoveTO_LoginScene();
        BattleGameManager.ID = string.Empty;
        if(BattleGameManager.ID == ""){
            Debug.Log("ID is empty");
        }
    }
    //
}
