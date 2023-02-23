using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
using UnityEngine.UI;
public class launcher : MonoBehaviourPunCallbacks
{
    public Dropdown m_dropdown_RoomMaxPlayers; 
    public Dropdown m_dropdown_MaxTime; 
    public Text serverstate;
    public GameObject m_panel_Loading; 
    public Text m_text_CurrentPlayerCount; 

    void Awake()
    {
        PhotonNetwork.AutomaticallySyncScene = true;

        m_panel_Loading.SetActive(false);
    }

    void Start()
    {
        m_text_CurrentPlayerCount.gameObject.SetActive(false);
        serverstate.text = "서버 연결 시도";
        Debug.Log("서버 연결 시도.");
        PhotonNetwork.ConnectUsingSettings();
    }

    public void JoinRandomOrCreateRoom()
    {
     
        m_text_CurrentPlayerCount.gameObject.SetActive(true);
        byte maxPlayers = byte.Parse(m_dropdown_RoomMaxPlayers.options[m_dropdown_RoomMaxPlayers.value].text); 
        int maxTime = int.Parse(m_dropdown_MaxTime.options[m_dropdown_MaxTime.value].text);

        RoomOptions roomOptions = new RoomOptions();
        roomOptions.MaxPlayers = maxPlayers; // 인원 지정.
        roomOptions.CustomRoomProperties = new ExitGames.Client.Photon.Hashtable() { { "maxTime", maxTime } };
        roomOptions.CustomRoomPropertiesForLobby = new string[] { "maxTime" }; 
        PhotonNetwork.JoinRandomOrCreateRoom(
            expectedCustomRoomProperties: new ExitGames.Client.Photon.Hashtable() { { "maxTime", maxTime } }, expectedMaxPlayers: maxPlayers, 
            roomOptions: roomOptions 
        );
    }

    public void CancelMatching()
    {
        print("매칭 취소.");
        m_panel_Loading.SetActive(false);

        print("방 떠남.");
        PhotonNetwork.LeaveRoom();
    }

    private void UpdatePlayerCounts()
    {
        m_text_CurrentPlayerCount.text = $"{PhotonNetwork.CurrentRoom.PlayerCount} / {PhotonNetwork.CurrentRoom.MaxPlayers}";
    }

   

    public override void OnConnectedToMaster()
    {
        serverstate.text = "서버 접속 완료";
        Debug.Log("서버 접속 완료.");

    }
    public void outapplication()
    {
        Application.Quit();
    }
    public override void OnJoinedRoom()
    {
        serverstate.text = "매칭 기다리는 중";
        Debug.Log("방 참가 완료.");
        
        Debug.Log($"{PhotonNetwork.LocalPlayer.NickName}은 인원수 {PhotonNetwork.CurrentRoom.MaxPlayers} 매칭 기다리는 중.");
        UpdatePlayerCounts();

        m_panel_Loading.SetActive(true);
    }

    public override void OnPlayerEnteredRoom(Player newPlayer)
    {
        Debug.Log($"플레이어 {newPlayer.NickName} 방 참가.");
        UpdatePlayerCounts();

        if (PhotonNetwork.IsMasterClient)
        {
            if (PhotonNetwork.CurrentRoom.PlayerCount == PhotonNetwork.CurrentRoom.MaxPlayers)
            {
                PhotonNetwork.LoadLevel("CutScene");
            }
        }
    }

    public override void OnPlayerLeftRoom(Player otherPlayer)
    {
        UpdatePlayerCounts();
    }
}
