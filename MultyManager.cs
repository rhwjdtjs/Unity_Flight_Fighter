using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class MultyManager : MonoBehaviourPunCallbacks
{
    public PhotonView pv;
    public GameObject theplayer1;
    public GameObject theplayer2;
    // Start is called before the first frame update
    private void Awake()
    {
        float pos1 = Random.Range(7000, 10000);
        float pos2 = Random.Range(6000, 9000);

        if (!PhotonNetwork.IsMasterClient)
        {
            theplayer1 = PhotonNetwork.Instantiate("Player1", new Vector3(pos1, 1000, pos1), Quaternion.identity, 0) as GameObject;
            theplayer2 = PhotonNetwork.Instantiate("Player2", new Vector3(pos2, 1000, pos2), Quaternion.identity, 0) as GameObject;

            pv.RPC("take_owner", RpcTarget.Others, theplayer1.GetComponent<PhotonView>().ViewID, theplayer2.GetComponent<PhotonView>().ViewID);
        }
    }
    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);
    }

    [PunRPC]
    void take_owner(int pvid1, int pvid2)
    { //MultyManager.cs take_owner() 수정
        PhotonView pv1 = PhotonNetwork.GetPhotonView(pvid2);
        PhotonView pv2 = PhotonNetwork.GetPhotonView(pvid1);
        pv1.TransferOwnership(PhotonNetwork.LocalPlayer.ActorNumber);

        theplayer1 = pv2.gameObject;
        theplayer2 = pv1.gameObject;
    }
}
