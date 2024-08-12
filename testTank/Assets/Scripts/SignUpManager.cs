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

    private string filePath;

    void Start()
    {
        // 경로 설정 (Resources 폴더는 빌드 후에는 쓰기 불가능하므로 Application.persistentDataPath 사용)
        filePath = Path.Combine(Application.persistentDataPath, "users.json");

        // 만약 Resources 폴더의 초기 파일을 사용하여 persistentDataPath로 복사하는 작업이 필요할 수 있음
        if (!File.Exists(filePath))
        {
            TextAsset jsonFile = Resources.Load<TextAsset>("users");
            if (jsonFile != null)
            {
                File.WriteAllText(filePath, jsonFile.text);
            }
            else
            {
                Debug.LogError("Initial users.json not found in Resources.");
            }
        }

        signupButton.onClick.AddListener(OnSignupButtonClick);
        closeSignupButton.onClick.AddListener(OnCloseSignupButtonClick);
        applyButton.onClick.AddListener(OnApplyButtonClick);

        signupPanel.SetActive(false);
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
            Debug.LogError("Username or password cannot be empty.");
            return;
        }

        // JSON 파일 읽기
        UserList userList;
        if (File.Exists(filePath))
        {
            string jsonText = File.ReadAllText(filePath);

            // JSON을 UserList로 파싱
            userList = JsonUtility.FromJson<UserList>("{\"users\":" + jsonText + "}");
        }
        else
        {
            userList = new UserList { users = new List<User>() };
        }

        // 사용자 추가
        userList.users.Add(new User { id = username, password = password });

        // JSON 파일에 저장
        string updatedJson = JsonUtility.ToJson(userList, true);
        File.WriteAllText(filePath, updatedJson);

        Debug.Log("User added and saved to JSON.");
        signupPanel.SetActive(false);

        // 경로를 출력
        Debug.Log("Persistent Data Path: " + Application.persistentDataPath);
    }
}
