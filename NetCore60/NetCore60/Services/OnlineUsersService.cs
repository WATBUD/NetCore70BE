using System.Collections.Generic;

public class OnlineUsersService
{
    private readonly List<string> onlineUsers = new List<string>();

    public void AddUser(string username)
    {
        if (!onlineUsers.Contains(username))
        {
            onlineUsers.Add(username);
        }
    }

    public void RemoveUser(string username)
    {
        onlineUsers.Remove(username);
    }


    public int GetOnlineUserCount()
    {
        return onlineUsers.Count;
    }
}

