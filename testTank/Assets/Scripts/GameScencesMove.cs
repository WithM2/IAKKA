using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameScencesMove : MonoBehaviour
{
    public void MoveTo_Battle_Game()
    {
        SceneManager.LoadScene("Battle_Game");
        // " " 안에 이동할 씬이름 입력

        Debug.Log("Moved to Battle_Game");
    }
    public void MoveTo_Main()
    {
        SceneManager.LoadScene("Main");
        // " " 안에 이동할 씬이름 입력

        Debug.Log("Moved to Main");
    }
    public void MoveTO_LoginScene()
    {
        SceneManager.LoadScene("LoginScene");
        // " " 안에 이동할 씬이름 입력

        Debug.Log("Moved to LoginScene");
    }
    public void MoveTo_QuizScene()
    {
        SceneManager.LoadScene("QuizScene");
        // " " 안에 이동할 씬이름 입력

        Debug.Log("Moved to QuizScene");
    }
}
