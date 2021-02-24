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

}
