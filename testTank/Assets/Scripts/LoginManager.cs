using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class LoginManager : MonoBehaviour
{
    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button loginButton;
    public TextMeshProUGUI errorMessage; // ErrorMessage 텍스트 요소를 연결하기 위한 필드

    private UserList userList;

    void Start()
    {
        LoadUserData();
        loginButton.onClick.AddListener(OnLoginButtonClick);
        errorMessage.text = ""; // 초기 오류 메시지를 빈 문자열로 설정
    }

    void LoadUserData()
    {
        TextAsset jsonFile = Resources.Load<TextAsset>("users");
        if (jsonFile != null)
        {
            userList = JsonUtility.FromJson<UserList>("{\"users\":" + jsonFile.text + "}");
        }
        else
        {
            Debug.LogError("Cannot load users.json file.");
            errorMessage.text = "Cannot load user data."; // 오류 메시지 출력
        }
    }

    void OnLoginButtonClick()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (IsValidUser(username, password))
        {
            Debug.Log("Login Successful");
            errorMessage.text = ""; // 성공 시 오류 메시지 지우기
            // 로그인 성공 시 다음 화면으로 이동하거나 다른 동작을 수행합니다.
        }
        else
        {
            Debug.Log("Login Failed");
            errorMessage.text = "Invalid username or password."; // 오류 메시지 출력
        }
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
}
