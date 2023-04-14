using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
public class GAMESCENEUI : MonoBehaviourPun
{
    private HP thehp;
    // Start is called before the first frame update
    void Start()
    {
        thehp = FindObjectOfType<HP>();
    }
    public void IfHPZERO()
    {
        if(thehp.hp==0) //두 플레이어중 한명이 체력이 0이면
        {
            PhotonNetwork.LoadLevel("ResultScene"); //결과씬으로 변경
        }
    }
   
}
