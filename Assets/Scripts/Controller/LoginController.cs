using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite;

public static class LoginController
{
  public static ActionResult<User> DoLogin(string username, string password)
  {
    User user = UserModel.FindByUsername(username);
    ActionResult<User> result = new ActionResult<User>();
    result.status = "SUCCESS";
    result.hasError = false;

    if (user != null)
    {
      result.item = user;
      if (user.Password != password)
      {
        result.hasError = true;
        result.error = "Usuário ou senha não encontrados";
        result.status = "FAIL";
        result.item = null;
      }
    }
    else
    {
      result.hasError = true;
      result.error = "Usuário ou senha incorretos";
      result.status = "FAIL";
    }
    return result;
  }
}
