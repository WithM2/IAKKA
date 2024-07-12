using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class LoginManager : MonoBehaviour
{
    public InputField usernameInput;
    public InputField passwordInput;
    public Button loginButton;
    public Text errorMessage;
    private UserData userData;

    void Start()
    {
        loginButton.onClick.AddListener(OnLoginButtonClicked);
        userData = UserData.LoadFromJSON("user_data");
        if (userData == null)
        {
            Debug.LogError("Failed to load user data!");
        }
    }

    void OnLoginButtonClicked()
    {
        string username = usernameInput.text;
        string password = passwordInput.text;

        if (userData != null && userData.ValidateUser(username, password))
        {
            SceneManager.LoadScene("ControlScene");
        }
        else
        {
            errorMessage.text = "Invalid username or password!";
        }
    }
}
