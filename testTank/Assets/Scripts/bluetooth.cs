using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArduinoBluetoothAPI;
using TMPro;


public class bluetooth : MonoBehaviour
{
    private BluetoothHelper helper;
    public GameObject BT_connect_btn;
    public GameObject BT_disconnect_btn;
    public GameObject BTname_obj;
    public TMP_InputField BTname_txt;

    void Start()
    {
        BluetoothHelper.BLE = false; // BLE 대신 클래식 블루투스 사용
        helper = BluetoothHelper.GetInstance(); // 인스턴스생성
        helper.OnConnected += OnConnected;     // 연결성공 시 호출할 매서드
        helper.OnConnectionFailed += OnConnectionFailed; // 연결 실패시 호출할 매서드
        helper.OnDataReceived += OnDataReceived;  // 데이터 수신 시 호출할 메서드
        helper.setFixedLengthBasedStream(1); // 1바이트씩 데이터 수신
        //helper.setDeviceName("HC-06"); // 테스트 코드 : 연결할 블루투스 이름
    }

    void OnConnected(BluetoothHelper helper)  // 연결 성공 시
    {
        Debug.Log("Succeed to connect");
        helper.StartListening();              // 데이터 수신
        BT_connect_btn.SetActive(false);      // connect 버튼 비활성화
        BT_disconnect_btn.SetActive(true);
        BTname_obj.SetActive(false);


        sendData(HP_ATT.my_ATT); // 아두이노로 전송
        sendData(HP_ATT.my_HP);  // 아두이노로 전송'
        
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

    public void sendData(int value)
    {
        string stringValue = value.ToString(); // 정수를 문자열로 변환
        helper.SendData(stringValue); // 데이터 전송
    }
}