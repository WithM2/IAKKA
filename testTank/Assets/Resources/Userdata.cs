[System.Serializable]
public class User
{
    public string id;
    public string password;
}

[System.Serializable]
public class UserList
{
    public User[] users;
}
