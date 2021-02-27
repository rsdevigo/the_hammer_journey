using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite;


public static class UserModel
{
  public static User FindByUsername(string username)
  {
    return DataService.instance._connection.Table<User>().Where(x => x.Username == username).FirstOrDefault();
  }

  public static User Create(string username, string password)
  {
    User user = new User() { Username = username, Password = password };
    int count = DataService.instance._connection.Insert(user);
    if (count == 0)
    {
      return null;
    }
    else
    {
      return DataService.instance._connection.Table<User>().Where(x => x.Username == username).FirstOrDefault();
    }
  }

}
