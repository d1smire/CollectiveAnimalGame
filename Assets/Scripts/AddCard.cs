using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AddCard : MonoBehaviour
{
    private CardManager _cardManager;
    private Notification _notification;
    [SerializeField] private string prefabName;

    private void Start()
    {
        _cardManager = GameObject.Find("PlayfabManager(Clone)").GetComponent<CardManager>();
        _notification = GameObject.Find("Notification(Clone)").GetComponent<Notification>();
    }

    public void SetName(string name) 
    {
        prefabName = name;
    }

    public void AddTameAnimals()
    {
        if (!string.IsNullOrEmpty(prefabName))
        {
            _cardManager.SetAnimalParams(prefabName);
            _cardManager.AddCardToData();
        }
        else
        {
            _notification.SomeInformation(InfoStatus.Error,"Відскануйте картку для приручення тварини");
        }
    }
}
