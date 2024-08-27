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

    private string filePath;
    private UserList userList;

    void Awake()
    {
        
    }

    void Start()
    {
        BluetoothHelper.BLE = false; // BLE 대신 클래식 블루투스 사용
        helper = BluetoothHelper.GetInstance(); // 인스턴스생성
        helper.OnConnected += OnConnected;     // 연결성공 시 호출할 매서드
        helper.OnConnectionFailed += OnConnectionFailed; // 연결 실패시 호출할 매서드
        helper.OnDataReceived += OnDataReceived;  // 데이터 수신 시 호출할 메서드
        helper.setFixedLengthBasedStream(60); // 1바이트씩 데이터 수신
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


        //sendData(BattleGameManager.my_ATT); // 아두이노로 전송
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
        string msg = helper.Read();
        switch(msg)
        {
            case "1":
                break;
            case "0":
                break;
            default:
                Debug.Log($"Received unknown message [{msg}]");
                break;
            }
        }

    public void BTname_change()
    {
        Debug.Log(BTname_txt.text);
        helper.setDeviceName(BTname_txt.text); // 연결할 블루투스 이름
        //helper.setDeviceName("HC-06"); // 테스트

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

    void LoadUserData()
    {
        string jsonText = File.ReadAllText(filePath);
        userList = JsonUtility.FromJson<UserList>(jsonText);
    }

    public void testbutton() // 버튼 누르면 아두이노로 상태정보 json직렬화한 문자열 형태로 전송
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
            userData += "}\0"; // 아두이노가 데이터를 수신할 때 데이터의 마지막임을 알 수 있는 표시
            sendData(userData);
            Debug.Log("testbutton clicked");
        }
        catch (System.Exception ex)
        {
            Debug.LogError("test Failed: " + ex.Message);
        }
    }
}