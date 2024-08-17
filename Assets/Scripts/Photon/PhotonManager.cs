using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using TMPro;

public class PhotonManager : MonoBehaviourPunCallbacks
{
    private string region = "eu";
    [SerializeField] private string nickName;

    List<RoomInfo> allRoomsInfo = new List<RoomInfo>();

    private static PhotonManager instance;

    private void Awake()
    {
        PhotonNetwork.ConnectUsingSettings();
        PhotonNetwork.ConnectToRegion(region);
    }

    private void Start()
    {
        UserAccount.OnNicknameRetrieved.AddListener(OnNicknameRetrieved);

        if (instance != null && instance != this)
        {
            Destroy(this.gameObject);
            return;
        }

        instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    private void OnNicknameRetrieved(string name)
    {
        nickName = name;
        PhotonNetwork.NickName = nickName;
    }

    public override void OnConnectedToMaster()
    {
        if (!PhotonNetwork.InLobby)
        {
            PhotonNetwork.JoinLobby();
        }
    }

    public override void OnDisconnected(DisconnectCause cause)
    {
        Debug.Log("Ви відключені від сервера!!!");
    }

    public void PlayButton()
    {
        if (!PhotonNetwork.IsConnected)
        {
            return;
        }
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        CreateRoom();
    }

    public void CreateRoom()
    {
        string roomName = "Room_" + Random.Range(0, 1000);
        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = 2;
        PhotonNetwork.CreateRoom(roomName, roomOptions, TypedLobby.Default);
    }

    public override void OnCreatedRoom()
    {
        PhotonNetwork.LoadLevel("ArenaOnlineFight");
    }

    public override void OnCreateRoomFailed(short returnCode, string message)
    {
        Debug.Log("Не вдалося створити кмінату!!!" + message);
    }

    public override void OnRoomListUpdate(List<RoomInfo> roomList)
    {
        foreach (RoomInfo info in roomList)
        {
            if (info.RemovedFromList)
            {
                allRoomsInfo.Remove(info);
            }
            else
            {
                allRoomsInfo.Add(info);
            }
        }
    }

    public override void OnJoinedRoom()
    {
        PhotonNetwork.LoadLevel("ArenaOnlineFight");
        FighterData fighterData = GameObject.Find("SaveFighterData").GetComponent<FighterData>();
        fighterData.AssignPlayerNumber();
    }

    public void JoinRandRoomButton()
    {
        PhotonNetwork.JoinRandomRoom();
    }

    public override void OnLeftRoom()
    {
        if (PhotonNetwork.InRoom)
        {
            PhotonNetwork.LeaveRoom();
            PhotonNetwork.LoadLevel("Menu");
        }
    }
}
