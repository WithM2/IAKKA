using UnityEngine;

public class DebugToScreen : MonoBehaviour
{
    private static string logMessages = ""; // 화면에 출력할 로그 메시지
    private GUIStyle guiStyle = new GUIStyle(); // GUI 스타일
    public static DebugToScreen Instance = null;

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

    void OnEnable()
    {
        // 로그 이벤트에 핸들러 추가
        Application.logMessageReceived += HandleLog;
    }

    void OnDisable()
    {
        // 로그 이벤트에서 핸들러 제거
        Application.logMessageReceived -= HandleLog;
        logMessages = "";
    }

    // 로그 메시지를 수집하는 함수
    void HandleLog(string logString, string stackTrace, LogType type)
    {
        logMessages += logString + "\n"; // 메시지 추가
        if (logMessages.Length > 200) // 너무 길어지면 초기화
        {
            logMessages = logMessages.Substring(logMessages.Length - 200);
        }
    }

    void OnGUI()
    {
        guiStyle.fontSize = 40; // 글씨 크기 설정
        guiStyle.normal.textColor = Color.white; // 글씨 색상 설정
        GUI.Label(new Rect(10, 10, Screen.width, Screen.height), logMessages, guiStyle);
    }
}