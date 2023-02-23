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
        Cursor.lockState = CursorLockMode.None;
    }
    public void tolobby()
    {
        PhotonNetwork.Disconnect();
        PhotonNetwork.LoadLevel("Lobby");
    }
    
    // Update is called once per frame
    void Update()
    {
        
    }
}
