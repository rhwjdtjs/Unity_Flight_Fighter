using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class HP : MonoBehaviourPun
{

    private TargetInfoUI targetui;
    [SerializeField] private Text hpremain;
    [SerializeField] private Text Rockinghpremain;
    [SerializeField] private Text myhpremain;
    [SerializeField] private ParticleSystem thedestroyeffect;
    public int hp;
    public int targethp;
    private float time1;
    void Start()
    {
        time1 = 2;
        targetui = FindObjectOfType<TargetInfoUI>();
        hpremain = GameObject.Find("targethp").GetComponent<Text>();
        Rockinghpremain = GameObject.Find("rockonhp").GetComponent<Text>();
        myhpremain = GameObject.Find("myhp").GetComponent<Text>();
        hp = 100;
        targethp = 100;
        myhpremain.text = hp.ToString();    //
        Rockinghpremain.text = hpremain.text = targethp.ToString();
    }
    IEnumerator timer()
    {
        yield return new WaitForSeconds(2f);
        PhotonNetwork.Destroy(this.gameObject);
        PhotonNetwork.LoadLevel("ResultScene");
    }
    public void Damage(int _damage)
    {
        hp -= _damage;
        myhpremain.text = hp.ToString();

        photonView.RPC("HPD", RpcTarget.Others, _damage);

        if (hp <= 0)
        {
            thedestroyeffect.Play();
            hp = 0;
           
            StartCoroutine(timer());
           
            
        }
    }
    [PunRPC]
    public void HPD(int _damage)
    {
        targethp -= _damage;
        Rockinghpremain.text = hpremain.text = targethp.ToString();


    }

}
