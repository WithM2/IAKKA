using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Button start_btn;
    public Button upgrade_btn;
    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft; //반 시계방향으로 회전
        start_btn.onClick.AddListener(OnStartButtonClick);
        upgrade_btn.onClick.AddListener(OnUpgradeButtonClick);
    }

    //버튼 클릭 관련 코드
    void OnStartButtonClick()
    {
        GameScencesMove.Instance.MoveTo_Battle_Game();
    }
    void OnUpgradeButtonClick()
    {
        GameScencesMove.Instance.MoveTo_QuizScene();
    }
    //
}
