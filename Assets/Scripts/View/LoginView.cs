using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class LoginView : MonoBehaviour
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

  // Update is called once per frame
  void Update()
  {

  }

  public void OnLoginButtonPressed()
  {
    ActionResult<User> action = LoginController.DoLogin(usernameInputField.text, passwordInputField.text);
    if (!action.hasError)
    {
      PlayerPrefs.SetInt("user.id", action.item.Id);
      PlayerPrefs.SetString("user.username", action.item.Username);
      SceneManager.LoadScene("MainMenu");
    }
    else
    {
      errorMessageText.text = action.error;
    }
  }
}
