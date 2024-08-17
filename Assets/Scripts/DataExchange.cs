using Newtonsoft.Json;
using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class DataExchange : MonoBehaviour
{
    [SerializeField] private FighterData _fighterData;

    private PhotonView _photonView;

    private GameObject _camera;
    private Arena _arena;

    private Dictionary<string, Character> characters;
    private Dictionary<string, string> playerAnimals;

    private void Start()
    {
        _photonView = GetComponent<PhotonView>();

        playerAnimals = new Dictionary<string, string>();
        characters = new Dictionary<string, Character>();

        _fighterData = GameObject.Find("SaveFighterData").GetComponent<FighterData>();
    }

    public void InitializeArena()
    {
        _arena = GameObject.Find("ArenaWithoutGrass").GetComponent<Arena>();
        _camera = GameObject.Find("Main Camera");
        SendFighter(_fighterData.SendCharacterName(), _fighterData.SendCharacterParams());
    }

    private void SendFighter(string fighterName, PlayerParams _charParams)
    {
        _photonView.RPC("GetFighterName", RpcTarget.AllBuffered, PhotonNetwork.NickName, fighterName);
        _photonView.RPC("GetCharacterParams", RpcTarget.AllBuffered, PhotonNetwork.NickName, JsonConvert.SerializeObject(_charParams.ReturnClass()));
    }

    public void SetAnimals()
    {
        _arena.SetPlayer1(playerAnimals.Values.ElementAt(0));
        _arena.SetPlayer2(playerAnimals.Values.ElementAt(1));
    }

    public void SetCameraAndPosition(Transform transform)
    {
        _camera.transform.position = transform.position;
        _camera.transform.rotation = Quaternion.Euler(0, 90, 0);
        _camera.transform.SetParent(transform);
    }

    public Dictionary<string, Character> SetCharacterParams()
    {
        return characters;
    }

    [PunRPC]
    private void GetFighterName(string nick, string animalName)
    {
        Debug.Log("User: " + nick + " send: " + animalName);

        if (!playerAnimals.ContainsKey(nick))
        {
            playerAnimals[nick] = animalName;
        }
    }

    [PunRPC]
    private void GetFighterParams(string nick, string animalParams)
    {
        Debug.Log("User: " + nick + " send: " + animalParams);

        if (!characters.ContainsKey(nick))
        {
            characters[nick] = JsonConvert.DeserializeObject<Character>(animalParams);
        }
    }
}
