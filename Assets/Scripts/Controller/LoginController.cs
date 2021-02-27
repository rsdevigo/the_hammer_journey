using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SQLite;
using UnityEngine.SceneManagement;

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

  public static ActionResult<User> DoRegister(string username, string password)
  {
    ActionResult<User> result = new ActionResult<User>();
    if (username == "" || password == "")
    {
      result.hasError = true;
      result.error = "Username e/ou password não podem ser vazios";
      result.item = null;
      result.status = "FAIL";
      return result;
    }
    try
    {
      User user = UserModel.Create(username, password);
      if (user == null)
      {
        result.hasError = true;
        result.error = "Ocorreu um erro ao tentar registrar o Usuário";
        result.item = null;
        result.status = "FAIL";
      }
      else
      {
        result.item = user;
      }
    }
    catch (SQLiteException ex)
    {
      if (SQLite3.ExtendedErrCode(DataService.instance._connection.Handle) == SQLite3.ExtendedResult.ConstraintUnique)
      {
        result.hasError = true;
        result.error = "Username já existente, tente outro";
        result.item = null;
        result.status = "FAIL";
      }
    }

    return result;
  }

  public static void Logout()
  {
    PlayerPrefs.DeleteKey("user.id");
    PlayerPrefs.DeleteKey("user.username");
    SceneManager.LoadScene("Login");
  }
}
