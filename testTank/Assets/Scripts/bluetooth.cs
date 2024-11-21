using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using ArduinoBluetoothAPI;
using TMPro;
using System.IO;


public class bluetooth : MonoBehaviour
{
    [SerializeField]
    private BattleGameManager battleGameManager; // BattleGameManager.cs 참조

    // 블루투스 관련
    private BluetoothHelper helper; // 블루투스 객체
    [SerializeField] private GameObject deviceButtonPrefab; // Bluetooth 장치 표시용 버튼 프리팹
    [SerializeField] private Transform deviceListContent;  // Scroll View의 Content 영역
    [SerializeField] private Button Scan_button;
    //

    // 로컬 data 관련
    private string filePath;
    private UserList userList;
    //

    // 카메라 제어
    //public MjpegStreamReader mjpegstreamReader; // MjpegStreamReader.cs 참조

    // json데이터 임시저장 장치
    class GameData{
        public int my_HP;  // JSON의 my_HP와 일치
        public int my_ATT; // JSON의 my_ATT와 일치
    }

    void Start()
    {
        Debug.Log("Bluetooth define");
        BluetoothHelper.BLE = false; // BLE 대신 클래식 블루투스 사용
        helper = BluetoothHelper.GetInstance(); // 인스턴스생성
        helper.OnConnected += OnConnected;     // 연결성공 시 호출할 매서드
        helper.OnConnectionFailed += OnConnectionFailed; // 연결 실패시 호출할 매서드
        helper.OnDataReceived += OnDataReceived;  // 데이터 수신 시 호출할 메서드
        helper.OnScanEnded += OnScanEnded;
        //helper.setFixedLengthBasedStream(1); // 1바이트씩 데이터 수신
        helper.setTerminatorBasedStream("\n"); // "\n"까지 수신
        //helper.setDeviceName("HC-06"); // 테스트 코드 : 연결할 블루투스 이름

        // 경로 설정 (Resources 폴더는 빌드 후에는 쓰기 불가능하므로 Application.persistentDataPath 사용)
        filePath = Path.Combine(Application.persistentDataPath, "users.json");
        LoadUserData();
        Debug.Log("Done");

    }

    void OnConnected(BluetoothHelper helper)  // 연결 성공 시
    {
        Debug.Log("Succeed to connect");
        helper.StartListening();              // 데이터 수신
        battleGameManager.ObjectDeactive(battleGameManager.Canvas_Buletooth);


        Debug.Log("FirstDataToArduino - start");
        //sendData(BattleGameManager.my_ATT); // 아두이노로 전송
        FirstDataToArduino(); //주석해제

        //mjpegstreamReader.camera_control(true); // 카레마 화면 활성화
    }

    void OnConnectionFailed(BluetoothHelper helper) // 연결 실패 시
    {
        Debug.Log("Failed to connect");
    }

    void OnDataReceived(BluetoothHelper helper)
    {
        string msg = helper.Read();

        // 데미지 받음
        if(msg.Trim() == "1"){
            Debug.Log("receive: " + msg);
            BattleGameManager.my_HP -= BattleGameManager.your_ATT;
            battleGameManager.my_HP_Slider.value -= BattleGameManager.your_ATT;
        }
        // 데미지 가함
        else if(msg.Trim() == "0"){
            Debug.Log("receive: " + msg);
            BattleGameManager.your_HP -= BattleGameManager.my_ATT;
            battleGameManager.your_HP_Slider.value -= BattleGameManager.my_ATT;
        }
        // 상대 정보 불러옴
        else if(msg.Length > 25 && msg.Length < 60){// 수신되는 데이터의 예상 길이 범위
            // Json 역직렬화
            GameData gameData = JsonUtility.FromJson<GameData>(msg); // JSON 데이터 역직렬화
            Debug.Log($"Received Data - HP: {gameData.my_HP}, ATT: {gameData.my_ATT}");
            // 상대 능력치 저장하는 코드
            BattleGameManager.your_HP = gameData.my_HP;
            BattleGameManager.your_ATT = gameData.my_ATT;

            // text 표시 내 능력치는 이미 표현함
            battleGameManager.your_ATT_text.text = $"{BattleGameManager.your_ATT}";
            // 채력바 초기 세팅
            battleGameManager.my_HP_Slider.maxValue = BattleGameManager.my_HP;
            battleGameManager.my_HP_Slider.value = BattleGameManager.my_HP;
            battleGameManager.your_HP_Slider.maxValue = BattleGameManager.your_HP;
            battleGameManager.your_HP_Slider.value = BattleGameManager.your_HP;
            
            if(BattleGameManager.my_HP > 0 && BattleGameManager.my_ATT > 0 &&
                BattleGameManager.your_HP > 0 && BattleGameManager.your_ATT > 0)
            {
                battleGameManager.BattleStart(); // 배틀 게임 시작 함수 호출
            }
            else{
                Debug.Log("Received data from arduino is not valid");
            }
        }
        else{
            Debug.Log($"Receive Error: " + msg);
        }
    }

    // private void ProcessReceivedData(string data)
    // {
    //     try
    //     {
    //         GameData gameData = JsonUtility.FromJson<GameData>(data); // JSON 데이터 역직렬화
    //         Debug.Log($"Received Data - HP: {gameData.my_HP}, ATT: {gameData.my_ATT}");
    //     }
    //     catch (System.Exception e)
    //     {
    //         Debug.LogError($"Error parsing JSON: {e.Message}"); // JSON 파싱 오류 처리
    //     }
    // }

    public void Connect() // connect 버튼 누르면 호출됨
    {
        Debug.Log("start_connecting");
        helper.Connect(); //  장치 연결시도
    }

    public void Disconnect() // disconnect 버튼 누르면 호출됨
    {
        helper.Disconnect(); // 연결 해체
        Debug.Log("disconnected");
    }

    void OnDestroy() // 오브젝트 파괴 시(주로 씬 종료 시)
    {
        if(helper != null && helper.isConnected() == true){ //블루투스기기가 연결되어있으면
            helper.Disconnect(); // 연결 해체
            Debug.Log("Bluetooth is disconnected");
        }
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



    public void FirstDataToArduino() // 아두이노로 json직렬화 문자열 전송
    {
        try{
            string userData = "";
            // json 직렬화해서 전달
            userData += "{";
            foreach (User user in userList.users)
            {
                if(user.id == BattleGameManager.ID && user.id != null){
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


    // 스캔 시작 버튼과 연결
    public void StartScan()
    {
        if (helper != null)
        {
            Scan_button.GetComponentInChildren<TMP_Text>().text = "Scanning";
            Scan_button.interactable = false;

            //devices.Clear(); // 이전 결과 초기화
            ClearDeviceListUI(); // UI 초기화
            helper.ScanNearbyDevices(); // 스캔 시작
            Debug.Log("Scanning for Bluetooth devices...");
        }
    }

    // 스캔 완료 시 호출되는 메서드
    private void OnScanEnded(BluetoothHelper helper, LinkedList<BluetoothDevice> devices)
    {
        Debug.Log("Scan completed. Found devices:");

        foreach (BluetoothDevice device in devices)
        {
            Debug.Log($"Device Name: {device.DeviceName}");
            if(device.DeviceName != null){
                AddDeviceToUI(device.DeviceName); // UI에 추가
            }
        }
        Debug.Log("Scan is done");
        Scan_button.GetComponentInChildren<TMP_Text>().text = "Scan";
        Scan_button.interactable = true;
    }

    // 스캔된 장치를 UI에 추가
    private void AddDeviceToUI(string deviceName)
    {
        try
        {
            if (deviceButtonPrefab == null)
            {
                Debug.LogError("DeviceButtonPrefab is not assigned. Please assign it in the Inspector.");
                return;
            }

            if (deviceListContent == null)
            {
                Debug.LogError("DeviceListContent is not assigned. Please assign it in the Inspector.");
                return;
            }
            GameObject deviceButton = Instantiate(deviceButtonPrefab, deviceListContent); // 프리팹 생성
            if (deviceButton == null)
            {
                Debug.LogError("Failed to instantiate the device button prefab.");
                return;
            }
            deviceButton.GetComponentInChildren<TMP_Text>().text = deviceName; // 버튼에 장치 이름 표시

            deviceButton.GetComponent<Button>().onClick.AddListener(() =>
            {
                ConnectToDevice(deviceName); // 클릭 시 장치에 연결
            });
        }
        catch (System.Exception  ex)
        {
            Debug.LogError($"Unhandled exception in AddDeviceToUI: {ex.Message}\n{ex.StackTrace}");
        }
        // 버튼 클릭 이벤트 등록
        
    }

    // UI 초기화
    private void ClearDeviceListUI()
    {
        foreach (Transform child in deviceListContent)
        {
            Destroy(child.gameObject);
        }
    }

    // Bluetooth 장치에 연결
    private void ConnectToDevice(string deviceName)
    {
        try
        {
            helper.setDeviceName(deviceName); // 연결할 장치 주소 설정
            helper.Connect(); // 연결 시도
            Debug.Log("Connecting to " + deviceName);
        }
        catch (System.Exception ex)
        {
            Debug.LogError("Connection failed: " + ex.Message);
        }
    }
}