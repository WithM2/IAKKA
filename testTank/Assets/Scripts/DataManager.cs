using System.Collections;
using System.Collections.Generic;
using UnityEngine.Phsics;
using System.IO;


public class DataManager : MonoBehaviour
{

    [HideInInspector]
    public string filePath;
    [HideInInspector]
    public UserList userList;


    // 파일 경로 설정
    public void Awake()
    {
        // 경로 설정 (Resources 폴더는 빌드 후에는 쓰기 불가능하므로 Application.persistentDataPath 사용)
        filePath = Path.Combine(Application.persistentDataPath, "users.json");
        LoadUserData();
    }
    public void LoadUserData()
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

    public void AddData(string username, string password, string hp, string att)
    {
        // userList에 데이터 추가
        userList.users.Add(new User { id = username, password = password, HP = hp, ATT = att });
    }

    public void clear()
    {
        userList.users.Clear();
    }

    public void JsonDataWrite()
    {
        // JSON 파일에 저장
        string updatedJson = JsonUtility.ToJson(userList, true);
        File.WriteAllText(filePath, updatedJson);
        Debug.Log("User added and saved to JSON.");
    }
}
