using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using System.IO;

public class SignupManager : MonoBehaviour
{
    [SerializeField]
    private DataManager dataManager;


    public TMP_InputField usernameInput;
    public TMP_InputField passwordInput;
    public Button signupButton;
    public Button applyButton; // Apply 버튼
    public GameObject signupPanel;
    public Button closeSignupButton;
    public TextMeshProUGUI errorMessage; // 오류 메시지를 표시할 TextMeshPro
    public GameObject popupPanel; // 팝업 패널

    private string filePath;

    void Start()
    {

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
        dataManager.AddData(username, password, "50", "10");

        // JSON 파일에 저장
        dataManager.JsonDataWrite();

        // 입력 필드 초기화
        usernameInput.text = string.Empty;
        passwordInput.text = string.Empty;

        signupPanel.SetActive(false);

        // 경로를 출력
        Debug.Log("Persistent Data Path: " + Application.persistentDataPath);
    }

    bool IsUsernameExists(string username)
    {
        foreach (User user in dataManager.userList.users)
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

    public void OnDeleteUserDataClick()
    {
        dataManager.clear();
    }

}
