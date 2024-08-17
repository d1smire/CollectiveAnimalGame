using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Photon.Pun;

public class ChatController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textLastMessage;
    [SerializeField] private TMP_InputField textMessageField;

    private PhotonView PhotonView;

    private void Start()
    {
        PhotonView = GetComponent<PhotonView>();
    }

    public void SendButton()
    {
        PhotonView.RPC("Send_Data", RpcTarget.AllBuffered, PhotonNetwork.NickName, textMessageField.text);
    }

    [PunRPC]
    private void Send_Data(string nick, string message)
    {
        textLastMessage.text = nick + ": " + message;
    }

}
