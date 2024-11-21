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
    public void MoveScene(string sceneName)
    {
        SceneManager.LoadScene(sceneName);

    }
    // scene이름 참고

        // "Battle_Game"
        // "Main"
        // "LoginScene"
        // "QuizScene"

    // scene이름 참고
}