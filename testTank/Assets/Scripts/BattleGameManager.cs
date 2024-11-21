using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;
using System.Text;

// #if UNITY_EDITOR
//     using System.IO.Ports;
// #endif

public class BattleGameManager : MonoBehaviour
{
    [SerializeField]
    private DataManager dataManager;

    public static string ID = ""; // 로그인 ID 정보

    [SerializeField]
    private GameObject onGameManager;
    public GameObject Canvas_Buletooth;
    public GameObject Canvas_Main;


    // 인게임 플레이 관련
    public static int my_HP;
    public static int my_ATT;
    public static int your_HP;
    public static int your_ATT;
    public TMP_Text my_ATT_text;
    public TMP_Text your_ATT_text;
    public Slider my_HP_Slider;
    public Slider your_HP_Slider;


    // #if UNITY_EDITOR
    //     private SerialPort serialPort;
    //     private string portName = "/dev/cu.P1_unity";   // MacOS 환경이면 포트 이름을 새로 지정해야함
    //     private int baudRate = 9600;
    // #endif

    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft; // 반 시계방향으로 회전
        // filePath = Path.Combine(Application.persistentDataPath, "users.json");
        // LoadUserData();
        //dataManager.LoadUserData();

        Debug.Log($"Login ID : {ID}");

        my_ATT_text.text = $"ATT : {my_ATT}";
        
        // #if UNITY_EDITOR
        //     // Serial 포트 초기화 및 열기
        //     InitializeSerialPort();
        // #endif
    }
    // #if UNITY_EDITOR
    //     void InitializeSerialPort()
    //     {
    //         try
    //         {
    //             serialPort = new SerialPort(portName, baudRate);
    //             serialPort.Open();
    //             Debug.Log("Serial port opened successfully.");
    //         }
    //         catch (Exception e)
    //         {
    //             Debug.LogError($"Failed to open serial port: {e.Message}");
    //         }
    //     }
    //     void SendDataToArduino(int hp, int att)
    //     {
    //         if (serialPort != null && serialPort.IsOpen)
    //         {
    //             string message = $"HP:{hp},ATT:{att}\n";
    //             serialPort.WriteLine(message); // 데이터를 직렬 포트로 전송
    //             Debug.Log($"Data sent to Arduino: {message}");
    //         }
    //         else
    //         {
    //             Debug.LogError("Serial port is not open.");
    //         }
    //     }
    // #endif

    // void LoadUserData()
    // {
    //     string jsonText = File.ReadAllText(filePath);
    //     userList = JsonUtility.FromJson<UserList>(jsonText);
    // }

    public void BattleStart() // 배틀 시작 시 실행
    {
        ObjectActive(Canvas_Main);
        if(Canvas_Main.activeSelf){
            onGameManager.SetActive(true);
        }
        else{
            Debug.Log("Canvas_Main is not activated");
            StartCoroutine(ExecuteAfterDelay(1f, BattleStart));
        }
    }
    IEnumerator ExecuteAfterDelay(float delay, System.Action action)
    {
        yield return new WaitForSeconds(delay); // 지정된 시간 대기
        action?.Invoke(); // 함수 실행
    }

    public void Victory() //게임 승리 시 실행
    {
        onGameManager.SetActive(false);
        Debug.Log("Victory");
        GameScencesMove.Instance.MoveScene("Victory");
    }

    public void Lose() //게임 패배 시 실행
    {
        //onGameManager.SetActive(false);
        Debug.Log("Lose");
        GameScencesMove.Instance.MoveScene("Lose");
    }

    public void ObjectActive(GameObject Object)
    {
        Object.SetActive(true);
    }
    public void ObjectDeactive(GameObject Object)
    {
        Object.SetActive(false);
    }

}
