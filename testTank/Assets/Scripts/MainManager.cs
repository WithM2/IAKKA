using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    [SerializeField]
    private DataManager dataManager;
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft; //반 시계방향으로 회전

        Debug.Log($"{BattleGameManager.ID}");

        foreach (User user in dataManager.userList.users) // 로그인 정보 가져오기
        {
            if (user.id == BattleGameManager.ID)
            {
                BattleGameManager.my_HP = int.Parse(user.HP);
                BattleGameManager.my_ATT = int.Parse(user.ATT);
                Debug.Log($"\"my_HP : {BattleGameManager.my_HP}\"\n\"my_ATT : {BattleGameManager.my_ATT}\"");
            }
        }


    }

    //버튼 클릭 관련 코드
    public void StartClicked()
    {
        GameScencesMove.Instance.MoveScene("Battle_Game");
    }
    public void UpgradeClicked()
    {
        GameScencesMove.Instance.MoveScene("QuizScene");
    }
    public void LogoutClicked()
    {
        GameScencesMove.Instance.MoveScene("LoginScene");
        BattleGameManager.ID = string.Empty;
        if(BattleGameManager.ID == ""){
            Debug.Log("ID is empty");
        }
        else{
            Debug.Log("ID is't empty");
        }
    }
    //
}
