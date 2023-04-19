using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class Resultscene : MonoBehaviourPunCallbacks
{
    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.None; //커서를 보이게함
    }
    public void tolobby()
    {
        PhotonNetwork.Disconnect(); //서버 닫기
        PhotonNetwork.LoadLevel("Lobby");//로비로 불러옴
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
