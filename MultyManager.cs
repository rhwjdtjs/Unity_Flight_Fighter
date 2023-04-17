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
    private void Awake() //start 보다 먼저 실행
    {
        float pos1 = Random.Range(7000, 10000); //플레이어 위치를 램덤으로 지정
        float pos2 = Random.Range(6000, 9000);

        if (!PhotonNetwork.IsMasterClient) //만약 첫번째로 들어와 서버의 마스터라면
        {
            theplayer1 = PhotonNetwork.Instantiate("Player1", new Vector3(pos1, 1000, pos1), Quaternion.identity, 0) as GameObject; //플레이어 1, 2 두객체를 우선 생성
            theplayer2 = PhotonNetwork.Instantiate("Player2", new Vector3(pos2, 1000, pos2), Quaternion.identity, 0) as GameObject;

            pv.RPC("take_owner", RpcTarget.Others, theplayer1.GetComponent<PhotonView>().ViewID, theplayer2.GetComponent<PhotonView>().ViewID); //그중하나으의 객체를 다른 플레이어에게 소유권을 넘김
        }
    }
    private void Start()
    {
        Screen.SetResolution(1920, 1080, true);
    }
    //다른 플레이어에게 소유권을 넘기는 함수
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
