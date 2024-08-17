using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;
using System.Linq;
using Newtonsoft.Json;
using Photon.Realtime;

public class FighterData : MonoBehaviour 
{
    public int playernumber;

    private string _fighterName;
    private PlayerParams _charParams;
    
    private PhotonView _photonView;
    
    private static FighterData instance;


    private void Start()
    {
        _photonView = GetComponent<PhotonView>();

        _photonView.ViewID = 2;

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void SetFighterInfo(string fighterName, PlayerParams charParams)
    {
        _fighterName = fighterName;
        _charParams = charParams;
    }

    public string SendCharacterName()
    {
        return _fighterName;
    }

    public PlayerParams SendCharacterParams() 
    {
        return _charParams;
    }

    public void AssignPlayerNumber()
    {
        Player[] players = PhotonNetwork.PlayerList;
        playernumber = players.ToList().IndexOf(PhotonNetwork.LocalPlayer) + 1;

        Debug.Log("Player Number: " + playernumber);
    }

    public bool IsAnimalSelected() 
    {
        if (_fighterName != "")
            return true;
        else
            return false;
    }
}
