using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class SignupManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button signupButton;
    public Button applyButton; // Apply 버튼
    public GameObject signupPanel;
    public Button closeSignupButton;
    public TextMeshProUGUI errorMessage; // 오류 메시지를 표시할 TextMeshPro
    public GameObject popupPanel; // 팝업 패널

    private static string filePath; // static추가
    private static UserList userList; // static추가

    void Start()
    {
        // 경로 설정 (Resources 폴더는 빌드 후에는 쓰기 불가능하므로 Application.persistentDataPath 사용)
        filePath = Path.Combine(Application.persistentDataPath, "users.json");

        // 사용자 데이터 로드
        LoadUserData();

        signupButton.onClick.AddListener(OnSignupButtonClick);
        closeSignupButton.onClick.AddListener(OnCloseSignupButtonClick);
        applyButton.onClick.AddListener(OnApplyButtonClick);

        signupPanel.SetActive(false);
    }

    public static void LoadUserData() // static추가
    {
        if (File.Exists(filePath))
        {
            string jsonText = File.ReadAllText(filePath);
            Debug.Log("Loaded JSON: " + jsonText); // JSON 내용 출력
            userList = JsonUtility.FromJson<UserList>(jsonText); // JSON을 UserList로 파싱
            if (userList == null)
            {
                userList = new UserList { users = new List<User>() };
                Debug.LogWarning("User data was null, initialized a new list.");
            }
            Debug.Log("User data loaded from JSON.");
        }
        else
        {
            userList = new UserList { users = new List<User>() };
            Debug.LogWarning("User data file not found, created a new list.");
        }
    }

    void OnSignupButtonClick()
    {
        Debug.Log("Signup button clicked");
        signupPanel.SetActive(true);
    }

    void OnCloseSignupButtonClick()
    {
        Debug.Log("Close button clicked");
        signupPanel.SetActive(false);
    }

    void OnApplyButtonClick()
    {
        Debug.Log("Apply button clicked");

        // 사용자 입력 가져오기
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (string.IsNullOrWhiteSpace(username) || string.IsNullOrWhiteSpace(password))
        {
            ShowPopup("Username or password cannot be empty.");
            return;
        }

        // 이미 존재하는 사용자 이름인지 확인
        if (IsUsernameExists(username))
        {
            ShowPopup("Username already exists. Please choose a different one.");
            return;
        }

        // 기존 데이터에 새로운 사용자 추가
        userList.users.Add(new User { id = username, password = password, HP = "", ATT = ""});
        //  HP, ATT 정보추가"금교원"


        // JSON 파일에 저장
        string updatedJson = JsonUtility.ToJson(userList, true);
        File.WriteAllText(filePath, updatedJson);

        Debug.Log("User added and saved to JSON.");

        // 입력 필드 초기화
        usernameInput.text = string.Empty;
        passwordInput.text = string.Empty;

        signupPanel.SetActive(false);

        // 경로를 출력
        Debug.Log("Persistent Data Path: " + Application.persistentDataPath);
    }

    bool IsUsernameExists(string username)
    {
        foreach (User user in userList.users)
        {
            if (user.id == username)
            {
                return true;
            }
        }
        return false;
    }

    void ShowPopup(string message)
    {
        popupPanel.SetActive(true);
        errorMessage.text = message;
    }

    void ClosePopup()
    {
        popupPanel.SetActive(false);
    }
}
