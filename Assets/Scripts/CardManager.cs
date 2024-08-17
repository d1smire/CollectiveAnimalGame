using UnityEngine;
using System.Collections.Generic;
using PlayFab;
using PlayFab.ClientModels;
using Newtonsoft.Json;

public class CardManager : MonoBehaviour 
{
    private Transform grid;

    private string cardName = "";
    private PlayerParams _playerParams;

    private List<Character> _characterParams = new List<Character>();

    private static CardManager instance;

    private void Start()
    {
        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetGrid(Transform Grid)
    {
        grid = Grid;
    }

    public void SetAnimalParams(string name)
    {
        cardName = name;
        _playerParams = Resources.Load<PlayerParams>("AnimalsBasicParameterSO/" + cardName + "BasicCharacteristics");
    }
    public void SetCurrentAnimalParams()
    {
        _playerParams = Resources.Load<PlayerParams>("CurrentCharCharacteristics");
        cardName = "Animals" + _playerParams.AnimalId;
    }

    public void AddCardToData() // Відправка
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                {cardName, JsonConvert.SerializeObject(_playerParams.ReturnClass()) }
            }
        };
        PlayFabClientAPI.UpdateUserData(request, OnSaveSuccess, OnError);
    }

    public void GetCharacters() // Отримання
    {
        PlayFabClientAPI.GetUserData(new GetUserDataRequest(), OnCharacterDataReceived, OnError);
    }

    private void OnSaveSuccess(UpdateUserDataResult result) // Відправка
    {
        GameObject.Find("Notification(Clone)").GetComponent<Notification>().SomeInformation(InfoStatus.Success, "Тварина успішно приручена");
    }

    private void OnCharacterDataReceived(GetUserDataResult result) // Отримання
    {
        if (result.Data != null)
        {
            foreach (var key in result.Data.Keys)
            {
                if (key.StartsWith("Animals") && int.TryParse(key.Substring(7), out int animalIndex) && animalIndex > 0)
                {
                    _characterParams.Add(JsonConvert.DeserializeObject<Character>(result.Data[key].Value));
                }
            }
        }
        CreateImagesOnGrid();
    }

    private void OnError(PlayFabError error)
    {
        Debug.LogError("Помилка збереження даних на PlayFab: " + error.ErrorMessage);
    }

    public Character GetCharParams(string name)
    {
        foreach (var Char in _characterParams) 
        {
            if (name == "Animals" + Char.AnimalId)
            {
                return Char;
            }
        }
        return null;
    }

    public void ClearList()
    {
        _characterParams.Clear();
    }

    public void CreateImagesOnGrid()
    {
        if (_characterParams != null)
        {
            foreach (var parameters in _characterParams)
            {
                GameObject prefab = Resources.Load<GameObject>("Animals" + parameters.AnimalId.ToString());

                if (prefab != null)
                {
                    Instantiate(prefab, grid.transform);
                }
                else
                {
                    Debug.LogWarning("Префаб з іменем " + "Animals" + parameters.AnimalId.ToString() + " не знайдено у папці Resources.");
                }
            }
        }
        else
        {
            Debug.LogWarning("Ім'я префаба не вказано.");
        }
    }
    public bool IsSomethingUpgrade()
    {
        foreach(var param in _characterParams) 
        {
            if (param.AnimalId == _playerParams.AnimalId)
            {
                if (param.NeedXPToLvlUp != _playerParams.NeedXPToLvlUp || param.CharLevel != _playerParams.CharLevel || param.CharXP != _playerParams.CharXP || param.Health != _playerParams.Health || param.ATK != _playerParams.ATK || param.DEF != _playerParams.DEF || 
                    param.Speed != _playerParams.Speed || param.CritChance != _playerParams.CritChance || 
                    param.EnergyRecovery != _playerParams.EnergyRecovery)
                {
                    return true;
                }
            }
        }
        return false;
    }
}
