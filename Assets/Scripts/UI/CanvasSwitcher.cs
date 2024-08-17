using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CanvasSwitcher : MonoBehaviour
{
    [SerializeField] private Canvas _menu;
    [SerializeField] private GameObject _queue;
    [SerializeField] private GameObject _submenu;
    [SerializeField] private Canvas _animalCollection;
    [SerializeField] private Canvas _animalParameters;
    [SerializeField] private Transform cardCollection;
    [SerializeField] private Button _scanButton;

    private FighterData _fighterData;
    private PhotonManager _photonManager;
    private Notification _notification;
    private CardManager _cardManager;
    private SceneLoader _sceneLoader;

    public void Initialize()
    {
        _fighterData = GameObject.Find("SaveFighterData(Clone)").GetComponent<FighterData>();
        _photonManager = GameObject.Find("PhotonService(Clone)").GetComponent<PhotonManager>();
        _notification = GameObject.Find("Notification(Clone)").GetComponent<Notification>();
        _cardManager = GameObject.Find("PlayfabManager(Clone)").GetComponent<CardManager>();
        _sceneLoader = GameObject.Find("SceneLoader").GetComponent<SceneLoader>();

        SetBtn();
    }

    public void PlayBtn() 
    {
        if (_fighterData.IsAnimalSelected())
        {
            _photonManager.PlayButton();

            _menu.enabled = false;
            _queue.SetActive(true);
        }
        else
        {
            _notification.SomeInformation(InfoStatus.Error, "Обери тварину для того щоб увійти в бій");
        }
    }

    public void OpenSubmenu()
    {
        _submenu.SetActive(true);
    }

    public void BackToMainMenu()
    {
        _menu.enabled = true;
        _submenu.SetActive(false);
    }

    public void OpenCollection()
    {
        _menu.enabled = false;

        _cardManager.SetGrid(cardCollection);
        _cardManager.GetCharacters();

        _animalCollection.enabled = true;
    }
    public void OpenAnimalParams()
    {
        _menu.enabled = false;
        _animalCollection.enabled = false;
        _animalParameters.enabled = true;
    }

    public void CloseCollection() 
    {
        _menu.enabled = true;

        DestroyCards();

        _cardManager.ClearList();

        _animalCollection.enabled = false;
    }

    public void CloseAnimalParams()
    {
        _cardManager.SetCurrentAnimalParams();
        if (_cardManager.IsSomethingUpgrade()) 
        {
            _cardManager.AddCardToData();
            
            _cardManager.ClearList();
            DestroyCards();
            _cardManager.GetCharacters();
        }

        _animalCollection.enabled = true;
        _animalParameters.enabled = false;
    }

    private void DestroyCards() 
    {
        foreach (Transform child in cardCollection.transform)
        {
            Destroy(child.gameObject);
        }
    }

    private void SetBtn() 
    {
        _scanButton.onClick.AddListener(() => _sceneLoader.LoadScene(2));
    }

    private void OnDisable()
    {
        _scanButton.onClick.RemoveAllListeners();
    }

    public void SignOut()
    {
        UserAccount.Instance.SingOut();
    }

    public void ExitGame() 
    {
        Application.Quit();

#if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
#endif
    }
}