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
        if(thehp.hp==0)
        {
            PhotonNetwork.LoadLevel("ResultScene");
        }
    }
   
}
