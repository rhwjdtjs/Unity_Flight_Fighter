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
        myhpremain.text = hp.ToString();    //남아 있는 체력 표기
        Rockinghpremain.text = hpremain.text = targethp.ToString(); //락중일때 상대 정보에 체력 표시
    }
    IEnumerator timer()
    {
        yield return new WaitForSeconds(2f);
        PhotonNetwork.Destroy(this.gameObject);
        PhotonNetwork.LoadLevel("ResultScene"); //체력이 0이되고 바로 끝나지 않고 2초대기후 종료
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
    [PunRPC] //서버로 적용
    public void HPD(int _damage)
    {
        targethp -= _damage;
        Rockinghpremain.text = hpremain.text = targethp.ToString();


    }

}
