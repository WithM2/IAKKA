using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class User
{
    public string username;
    public string password;
}

[System.Serializable]
public class UserData
{
    public List<User> users;

    public static UserData LoadFromJSON(string fileName)
    {
        TextAsset file = Resources.Load<TextAsset>(fileName);
        if (file != null)
        {
            return JsonUtility.FromJson<UserData>(file.text);
        }
        return null;
    }

    public bool ValidateUser(string username, string password)
    {
        foreach (User user in users)
        {
            if (user.username == username && user.password == password)
            {
                return true;
            }
        }
        return false;
    }
}
