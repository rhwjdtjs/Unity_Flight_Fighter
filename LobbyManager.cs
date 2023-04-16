using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class LobbyManager : MonoBehaviourPunCallbacks //미사용 코드
{
    [SerializeField] private GameObject theplayer;
    public static bool isplayer1join = false;
    public static bool isplayer2join = false;
    private string gameVersion = "1";
    public Text connectionInfoText;
    public Button joinButton;
    private MultyManager themuit;
   
    // Start is called before the first frame update
    void Start()
    {
        Screen.SetResolution(1920, 1080, true);
        themuit = FindObjectOfType<MultyManager>();
        PhotonNetwork.GameVersion = gameVersion;
        PhotonNetwork.ConnectUsingSettings();

        joinButton.interactable = false;
        connectionInfoText.text = "connecting to Server...";
    }
    public override void OnConnectedToMaster()
    {
        joinButton.interactable = true;
        connectionInfoText.text = "Online : Connected to Server";
    }
    public override void OnDisconnected(DisconnectCause cause)
    {
        joinButton.interactable = false;
        connectionInfoText.text = "Offline: Can't connect Server...";
        PhotonNetwork.ConnectUsingSettings();
    }
    public void EXITTT()
    {
        Debug.Log("QUIT");
        Application.Quit();
    }
    public void Connect()
    {
        Debug.Log("click");
        joinButton.interactable = false;

        if(PhotonNetwork.IsConnected)
        {
            connectionInfoText.text = "connecting Room...";
            PhotonNetwork.JoinRandomRoom();
        }
        else
        {
            connectionInfoText.text = "Offline: Retring...";
            PhotonNetwork.ConnectUsingSettings();
        }
    }
    public override void OnJoinRandomFailed(short returnCode, string message)
    {
        connectionInfoText.text = "Have no Empty Room... Creating..";
        PhotonNetwork.CreateRoom(null, new RoomOptions { MaxPlayers = 2 });
    }
    public override void OnJoinedRoom()
    {
        connectionInfoText.text = "Connected to Room";
        PhotonNetwork.LoadLevel("CutScene");
       
      


    }

    // Update is called once per frame
    void Update()
    {
       
    }
}
