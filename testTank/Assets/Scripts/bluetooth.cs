using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArduinoBluetoothAPI;
using TMPro;
using System.IO;


public class bluetooth : MonoBehaviour
{
    private BluetoothHelper helper;
    public GameObject BT_connect_btn;
    public GameObject BT_disconnect_btn;
    public GameObject BTname_obj;
    public TMP_InputField BTname_txt;

    public TMP_Text my_ATT_text;
    public TMP_Text your_ATT_text;

    private string filePath;
    private UserList userList;

    class GameData{
        public int my_HP;  // JSON의 my_HP와 일치
        public int my_ATT; // JSON의 my_ATT와 일치
    }

    void Start()
    {
        BluetoothHelper.BLE = false; // BLE 대신 클래식 블루투스 사용
        helper = BluetoothHelper.GetInstance(); // 인스턴스생성
        helper.OnConnected += OnConnected;     // 연결성공 시 호출할 매서드
        helper.OnConnectionFailed += OnConnectionFailed; // 연결 실패시 호출할 매서드
        helper.OnDataReceived += OnDataReceived;  // 데이터 수신 시 호출할 메서드
        //helper.setFixedLengthBasedStream(1); // 1바이트씩 데이터 수신
        helper.setTerminatorBasedStream("\n"); // "\n"까지 수신
        //helper.setDeviceName("HC-06"); // 테스트 코드 : 연결할 블루투스 이름


        // 경로 설정 (Resources 폴더는 빌드 후에는 쓰기 불가능하므로 Application.persistentDataPath 사용)
        filePath = Path.Combine(Application.persistentDataPath, "users.json");
        LoadUserData();
    }

    void OnConnected(BluetoothHelper helper)  // 연결 성공 시
    {
        Debug.Log("Succeed to connect");
        helper.StartListening();              // 데이터 수신
        BT_connect_btn.SetActive(false);      // connect 버튼 비활성화
        BT_disconnect_btn.SetActive(true);
        BTname_obj.SetActive(false);

        Debug.Log("FirstDataToArduino - start");
        //sendData(BattleGameManager.my_ATT); // 아두이노로 전송
        FirstDataToArduino(); //주석해제
    }

    void OnConnectionFailed(BluetoothHelper helper) // 연결 실패 시
    {
        Debug.Log("Failed to connect");
        BT_connect_btn.SetActive(true);
        BT_disconnect_btn.SetActive(false);
        BTname_obj.SetActive(true);
    }

    void OnDataReceived(BluetoothHelper helper)
    {
        //임시로 코드임. 상대 HP 정보 불러오기 + 데미지 시스템
        string msg = helper.Read();
        if(msg == "1"){
            int number = int.Parse(msg); // 문자열을 int로 변환
            Debug.Log("Converted number: " + number);
            BattleGameManager.my_HP -= BattleGameManager.your_ATT;
        }
        else if(msg == "0"){
            BattleGameManager.your_HP -= BattleGameManager.my_ATT;
        }
        else if(msg.Length > 25 && msg.Length < 60){// 수신되는 데이터의 예상 길이 범위
            // Json 역직렬화
            GameData gameData = JsonUtility.FromJson<GameData>(msg); // JSON 데이터 역직렬화
            Debug.Log($"Received Data - HP: {gameData.my_HP}, ATT: {gameData.my_ATT}");

            // 상대 능력치 저장하는 코드
            BattleGameManager.your_HP = gameData.my_HP;
            BattleGameManager.your_ATT = gameData.my_ATT;

            my_ATT_text.text = $"{BattleGameManager.my_ATT}"; // text 표시
            your_ATT_text.text = $"{BattleGameManager.your_ATT}";
        }
        else{
            Debug.Log($"Receive Error");
        }
    }

    private void ProcessReceivedData(string data)
    {
        try
        {
            GameData gameData = JsonUtility.FromJson<GameData>(data); // JSON 데이터 역직렬화
            Debug.Log($"Received Data - HP: {gameData.my_HP}, ATT: {gameData.my_ATT}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"Error parsing JSON: {e.Message}"); // JSON 파싱 오류 처리
        }
    }

    public void BTname_change()
    {
        Debug.Log(BTname_txt.text);
        helper.setDeviceName(BTname_txt.text); // 연결할 블루투스 이름
    }

    public void Connect() // connect 버튼 누르면 호출됨
    {
        Debug.Log("start_connecting");
        helper.Connect(); //  장치 연결시도
    }

    public void Disconnect() // disconnect 버튼 누르면 호출됨
    {
        helper.Disconnect(); // 연결 해체
        BT_connect_btn.SetActive(true);
        BT_disconnect_btn.SetActive(false);
        BTname_obj.SetActive(true);
    }

    void OnDestroy() // 지우기
    {
        Debug.Log("OnDestroy");
    }

    public void sendData(string value)
    {
        try
        {
            if (helper != null && helper.isConnected())
            {
                helper.SendData(value); // 데이터 전송
                Debug.Log("Data sent: " + value);
           }
           else
            {
                Debug.LogWarning("Bluetooth is not connected.");
          }
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Failed to send data: " + ex.Message);
        }
    }



    public void FirstDataToArduino() // 버튼 누르면 아두이노로 상태정보 json직렬화한 문자열 형태로 전송
    {
        try{
            LoadUserData();
            string userData = "";
            // json 직렬화해서 전달
            userData += "{";
            foreach (User user in userList.users)
            {
                if(user.id == "player1" && user.id != null){
                    userData += $"\"my_HP\":\"{user.HP}\",\"my_ATT\":\"{user.ATT}\"";
                }
            }
            userData += "}\n"; // 아두이노가 데이터를 수신할 때 데이터의 마지막임을 알 수 있는 표시
            sendData(userData);
            Debug.Log("First Data is sent to Arduino");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("test Failed: " + ex.Message);
        }
    }

    void LoadUserData()
    {
        string jsonText = File.ReadAllText(filePath);
        userList = JsonUtility.FromJson<UserList>(jsonText);
    }
}