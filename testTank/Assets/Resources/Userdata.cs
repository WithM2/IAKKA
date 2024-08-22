using System; // System.Serializable을 사용하기 위한 using 지시문
using System.Collections.Generic; // List<>를 사용하기 위한 using 지시문

[System.Serializable]
public class User
{
    public string id;
    public string password;
    public string HP;
    public string ATT;
}

[System.Serializable]
public class UserList
{
    public List<User> users; // List로 변경하여 쉽게 관리
}

// [System.Serializable]
// public class UserList
// {
//     public User[] users;
// }