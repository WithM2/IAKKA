using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.Collections.Generic;
using System.IO;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public Button viewUserDataButton; // View User Data 버튼
    public Button deleteUserDataButton; // Delete User Data 버튼
    public TextMeshProUGUI errorMessage;
    public GameObject popupPanel;
    public Button closeButton;

    private string filePath;
    private UserList userList;

    void Start()
    {
        // 경로 설정
        filePath = Path.Combine(Application.persistentDataPath, "users.json");

        // 이벤트 리스너 설정
        loginButton.onClick.AddListener(OnLoginButtonClick);
        closeButton.onClick.AddListener(ClosePopup);
        viewUserDataButton.onClick.AddListener(OnViewUserDataClick);
        deleteUserDataButton.onClick.AddListener(OnDeleteUserDataClick);

        popupPanel.SetActive(false);
    }

    void LoadUserData()
    {
        if (File.Exists(filePath))
        {
            string jsonText = File.ReadAllText(filePath);
            userList = JsonUtility.FromJson<UserList>(jsonText);
            if (userList == null)
            {
                userList = new UserList { users = new List<User>() };
            }
            Debug.Log("User data loaded from JSON.");
        }
        else
        {
            userList = new UserList { users = new List<User>() };
            Debug.LogWarning("User data file not found, created a new list.");
        }
    }

    void OnLoginButtonClick()
    {
        // 로그인 버튼을 누를 때마다 사용자 데이터를 새로 로드
        LoadUserData();

        string username = usernameInput.text;
        string password = passwordInput.text;

        if (IsValidUser(username, password))
        {
            Debug.Log("Login Successful");
            popupPanel.SetActive(false); // 로그인 성공 시 팝업 닫기
            // 로그인 성공 시 다음 화면으로 이동하거나 다른 동작을 수행합니다.
        }
        else
        {
            Debug.Log("Login Failed");
            ShowPopup("Invalid username or password.");
        }
    }

    void OnViewUserDataClick()
    {
        // View User Data 버튼을 누를 때마다 사용자 데이터를 새로 로드
        LoadUserData();

        Debug.Log("View User Data button clicked");

        // 사용자 데이터를 문자열로 변환하여 출력
        if (userList == null || userList.users == null || userList.users.Count == 0)
        {
            ShowPopup("No user data available.");
            return;
        }

        string userData = "User Data:\n";
        foreach (User user in userList.users)
        {
            userData += $"ID: {user.id}, Password: {user.password}\n";
        }
        ShowPopup(userData);
    }

    void OnDeleteUserDataClick()
    {
        Debug.Log("Delete User Data button clicked");

        // 사용자 데이터 전체 삭제
        userList.users.Clear();
        File.WriteAllText(filePath, JsonUtility.ToJson(userList, true));
        Debug.Log("All user data deleted.");
        ShowPopup("All user data deleted.");
    }

    bool IsValidUser(string username, string password)
    {
        foreach (User user in userList.users)
        {
            if (user.id == username && user.password == password)
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
