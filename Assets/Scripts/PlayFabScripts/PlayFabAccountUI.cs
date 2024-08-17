using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayFabAccountUI : MonoBehaviour
{
    [SerializeField] private Text errorSIText;
    [SerializeField] private Text errorRText;
    [SerializeField] private Canvas signInCanvas;
    [SerializeField] private Canvas registrationCanvas;

    [SerializeField] private SceneLoader sceneLoader;

    private string userName, password, emailAddress;

    private void OnEnable()
    {
        UserAccount.OnSignInFailed.AddListener(OnSignInFailed);
        UserAccount.OnSignInSuccess.AddListener(OnSignInSuccess);
        UserAccount.OnCreateAccountFailed.AddListener(OnCreateAccountFailed);
        UserAccount.OnSignInSuccess.AddListener(OnSignInSuccess);
    }

    private void OnDisable()
    {
        UserAccount.OnSignInFailed.AddListener(OnSignInFailed);
        UserAccount.OnSignInSuccess.RemoveListener(OnSignInSuccess);
        UserAccount.OnCreateAccountFailed.RemoveListener(OnCreateAccountFailed);
        UserAccount.OnSignInSuccess.RemoveListener(OnSignInSuccess);
    }

    private void OnSignInFailed(string error)
    {
        errorSIText.gameObject.SetActive(true);
        errorSIText.text = error;
    }

    private void OnCreateAccountFailed(string error)
    {
        errorRText.gameObject.SetActive(true);
        errorRText.text = error;
    }

    private void OnSignInSuccess()
    {
        signInCanvas.enabled = false;
        registrationCanvas.enabled = false;
    }

    public void UpdateUsername(string _username)
    {
        userName = _username;
    }

    public void UpdatePassword(string _password)
    {
        password = _password;
    }

    public void UpdateEmailAddress(string _emailAddress)
    {
        emailAddress = _emailAddress;
    }

    public void SignIn()
    {
        UserAccount.Instance.SignIn(userName, password);
        sceneLoader.LoadScene(1);
    }

    public void CreateAccount()
    {
        UserAccount.Instance.CreateAccount(userName, emailAddress, password);
    }

}
