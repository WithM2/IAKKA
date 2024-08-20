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
}
