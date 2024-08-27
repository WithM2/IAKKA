using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;


public class BattleGameManager : MonoBehaviour
{
    public static string ID = "";

    public static int my_HP;
    public static int my_ATT;

    public static int your_HP;
    public static int your_ATT;

    private string filePath;
    private UserList userList;

    // Start is called before the first frame update
    void Start()
    {
        Screen.orientation = ScreenOrientation.LandscapeLeft; //반 시계방향으로 회전
        filePath = Path.Combine(Application.persistentDataPath, "users.json");
        LoadUserData();
        Debug.Log($"Login ID : {ID}");

        testcode(); // json파일에 HP와 ATT 값 저장하는 코드 (나중에 지움)

        //foreach부터 시작
        foreach (User user in userList.users) //로그인 정보 가져오기
        {
            if (user.id == ID)
            {
                my_HP = int.Parse(user.HP);
                my_ATT = int.Parse(user.ATT);
                Debug.Log($"\"my_HP : {my_HP}\"\n\"my_ATT : {my_ATT}\"");
                // json에서 데이터 가져오기
            }
        }
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    void LoadUserData()
    {
        string jsonText = File.ReadAllText(filePath);
        userList = JsonUtility.FromJson<UserList>(jsonText);
    }

    void OnDestroy() //ID 초기화(클리어)
    {
        ID = null;
        if (ID == null){
            Debug.Log("Login information clear");
        }
    }

    void testcode() // 나중에 지우기
    {
        foreach (User user in userList.users) //로그인 정보 넣기
        {
            if (user.id == ID)
            {
                int a = 120;
                int b = 10;
                user.HP = $"{a}";
                user.ATT = $"{b}";
                Debug.Log($"\"user.HP : {user.HP}\"\n\"user.ATT : {user.ATT}\"");

                string updatedJson = JsonUtility.ToJson(userList, true); // json에 저장
                File.WriteAllText(filePath, updatedJson);
            }
        }
    }
}