using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using PlayFab;
using PlayFab.ClientModels;
using System.Net.Mail;
using UnityEngine.SceneManagement;
using Photon.Pun;

public class UserAccount : MonoBehaviour
{
    public static UserAccount Instance;

    public static UnityEvent OnSignInSuccess = new UnityEvent();
    public static UnityEvent OnSignOutSuccess = new UnityEvent();

    public static UnityEvent<string> OnSignInFailed = new UnityEvent<string>();
    public static UnityEvent<string> OnCreateAccountFailed = new UnityEvent<string>();
    public static UnityEvent<string> OnNicknameRetrieved = new UnityEvent<string>();

    private void Awake()
    {
        Instance = this;
    }

    public void CreateAccount(string userName, string emailAddress, string password)
    {
        PlayFabClientAPI.RegisterPlayFabUser(
            new RegisterPlayFabUserRequest()
            {
                Username = userName,
                Email = emailAddress,
                Password = password
            },
            response =>
            {
                UpdateDisplayName(userName, response.PlayFabId);
                SignIn(userName, password);
            },
            error =>
            {
                OnCreateAccountFailed.Invoke(error.ErrorMessage);
            }
        );
    }

    public void SignIn(string userName, string password)
    {
        PlayFabClientAPI.LoginWithPlayFab(
            new LoginWithPlayFabRequest()
            {
                Username = userName,
                Password = password
            },
            response =>
            {
                CreateGameComponents();

                GetUserDisplayName(response);

                InvokeSignInSuccessWithDelay(0.5f);
            },
            error =>
            {
                OnSignInFailed.Invoke(error.ErrorMessage);
            }
        );
    }

    private void GetUserDisplayName(LoginResult result)
    {
        PlayFabClientAPI.GetPlayerProfile(new GetPlayerProfileRequest
        {
            PlayFabId = result.PlayFabId,
            ProfileConstraints = new PlayerProfileViewConstraints
            {
                ShowDisplayName = true
            }
        }, profileResult =>
        {
            string displayName = profileResult.PlayerProfile?.DisplayName ?? "User";
            OnNicknameRetrieved.Invoke(displayName);
        }, error =>
        {
            OnNicknameRetrieved.Invoke("User");
        });
    }

    private void UpdateDisplayName(string userName, string playFabId)
    {
        PlayFabClientAPI.UpdateUserTitleDisplayName(new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = userName
        }, result =>
        {
            Debug.Log($"Нікнейм оновлено: {result.DisplayName}");
        }, error =>
        {
            Debug.LogError($"Не вдалося оновити нікнейм: {error.ErrorMessage}");
        });
    }

    private void CreateGameComponents()
    {
        //GameObject.Find("SceneLoader").GetComponent<SceneLoader>().SwitchAnimation(true);

        GameObject[] componentPrefabs = Resources.LoadAll<GameObject>("Components");

        foreach (GameObject prefab in componentPrefabs)
        {
            Instantiate(prefab);
        }
    }

    private IEnumerator InvokeSignInSuccessWithDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnSignInSuccess.Invoke();
    }

    public void SingOut()
    {
        PlayFabClientAPI.ForgetAllCredentials();
        OnSignOutSuccess.Invoke();
        PhotonNetwork.Disconnect();
        Destroy(GameObject.Find("Notification(Clone)"));
        Destroy(GameObject.Find("PhotonService(Clone)"));
        Destroy(GameObject.Find("PlayfabManager(Clone)"));
        Destroy(GameObject.Find("SaveFighterData(Clone)"));
        SceneManager.LoadScene("Registration");
    }
}
