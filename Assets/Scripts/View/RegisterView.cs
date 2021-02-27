using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class RegisterView : MonoBehaviour
{
  [SerializeField] public InputField usernameInputField;
  [SerializeField] public InputField passwordInputField;
  [SerializeField] public Text errorMessageText;
  // Start is called before the first frame update
  void Start()
  {
    if (PlayerPrefs.GetInt("user.id", 0) != 0)
    {
      SceneManager.LoadScene("MainMenu");
    }
  }

  public void OnBackButtonPressed()
  {
    SceneManager.LoadScene("Login");
  }

  public void OnRegisterButtonPressed()
  {
    ActionResult<User> result = LoginController.DoRegister(usernameInputField.text, passwordInputField.text);
    if (result.hasError)
    {
      errorMessageText.text = result.error;
    }
    else
    {
      PlayerPrefs.SetInt("user.id", result.item.Id);
      PlayerPrefs.SetString("user.username", result.item.Username);
      SceneManager.LoadScene("MainMenu");
    }
  }
}
