using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScencesMove : MonoBehaviour
{
    // 상글톤 패턴 사용
    public static GameScencesMove Instance = null;
    private void Awake()
    {
        if(Instance == null){ //생성 전이면
            Instance = this;  // 생성
        }
        else if(Instance != this){  //이미 생성되어있으면
            Destroy(this.gameObject); // 새로만든 거 삭제
        }
        DontDestroyOnLoad(this.gameObject);
    }
    // 싱글톤 패턴 사용
    
    // 메인 코드 start
    public void MoveTo_Battle_Game()
    {
        SceneManager.LoadScene("Battle_Game");
        // " " 안에 이동할 씬이름 입력

        Debug.Log("Moved to Battle_Game");
    }
    public void MoveTo_Main()
    {
        SceneManager.LoadScene("Main");
        Debug.Log("Moved to Main");
    }
    public void MoveTO_LoginScene()
    {
        SceneManager.LoadScene("LoginScene");
        Debug.Log("Moved to LoginScene");
    }
    public void MoveTo_QuizScene()
    {
        SceneManager.LoadScene("QuizScene");
        Debug.Log("Moved to QuizScene");
    }
}