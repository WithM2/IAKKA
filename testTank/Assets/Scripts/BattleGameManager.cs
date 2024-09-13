using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System.IO;
using System;
using System.Text;
//using System.IO.Ports;

public class BattleGameManager : MonoBehaviour
{
    public static string ID = ""; // 로그인 ID 정보


    // 인게임 플레이 관련
    public static int my_HP;
    public static int my_ATT;
    public static int your_HP;
    public static int your_ATT;
    public TMP_Text my_ATT_text; // HP_text_control에서 관리하기
    public TMP_Text your_ATT_text; // HP_text_control에서 관리하기
    public static Slider my_HP_Slider; // 코딩하기
    public static Slider your_HP_Slider; // 코딩하기

    //

    // 로컬 data 관련
    private string filePath;
    private UserList userList;
    //

    // 앱 빌드시 주석 {
    //private SerialPort serialPort;
    //private string portName = "/dev/cu.P1_unity";   // MacOS 환경이면 포트 이름을 새로 지정해야함
    //private int baudRate = 9600;
    //  앱 빌드시 주석 }



    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft; // 반 시계방향으로 회전
        filePath = Path.Combine(Application.persistentDataPath, "users.json");
        LoadUserData();
        Debug.Log($"Login ID : {ID}");

        testcode(); // json 파일에 HP와 ATT 값 저장하는 코드 (나중에 지움)

        foreach (User user in userList.users) // 로그인 정보 가져오기
        {
            if (user.id == ID)
            {
                my_HP = int.Parse(user.HP);
                my_ATT = int.Parse(user.ATT);
                Debug.Log($"\"my_HP : {my_HP}\"\n\"my_ATT : {my_ATT}\"");

                // 데이터 전송
                //SendDataToArduino(my_HP, my_ATT);
            }
        }

        my_ATT_text.text = $"ATT : {my_ATT}";
        // Serial 포트 초기화 및 열기
        //InitializeSerialPort();
    }

   /*void InitializeSerialPort()
    {
        try
        {
            serialPort = new SerialPort(portName, baudRate);
            serialPort.Open();
            Debug.Log("Serial port opened successfully.");
        }
        catch (Exception e)
        {
            Debug.LogError($"Failed to open serial port: {e.Message}");
        }
    }*/

    void LoadUserData()
    {
        string jsonText = File.ReadAllText(filePath);
        userList = JsonUtility.FromJson<UserList>(jsonText);
    }

    void OnDestroy() // ID 초기화(클리어)
    {
        ID = null;
        if (ID == null)
        {
            Debug.Log("Login information clear");
        }

        /*if (serialPort != null && serialPort.IsOpen)
        {
            serialPort.Close();
            Debug.Log("Serial port closed.");
        }*/
    }

    void testcode() // 나중에 지우기
    {
        foreach (User user in userList.users) // 로그인 정보 넣기
        {
            if (user.id == ID)
            {
                int a = 120;
                int b = 10;
                user.HP = $"{a}";
                user.ATT = $"{b}";
                //Debug.Log($"\"user.HP : {user.HP}\"\n\"user.ATT : {user.ATT}\"");

                string updatedJson = JsonUtility.ToJson(userList, true); // json에 저장
                File.WriteAllText(filePath, updatedJson);
                Debug.Log("testcode 실행 성공");
            }
        }
    }

    /*void SendDataToArduino(int hp, int att)
    {
        if (serialPort != null && serialPort.IsOpen)
        {
            string message = $"HP:{hp},ATT:{att}\n";
            serialPort.WriteLine(message); // 데이터를 직렬 포트로 전송
            Debug.Log($"Data sent to Arduino: {message}");
        }
        else
        {
            Debug.LogError("Serial port is not open.");
        }
    }*/
    public static void HP_text_control() // 
    {

    }
}
