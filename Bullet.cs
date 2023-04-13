using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Bullet : MonoBehaviourPun
{
    Transform parent;
    TrailRenderer trailrenderer;
    public int actornumber;
    [SerializeField] private float speed; //총알 속도
    [SerializeField] private float survivaltime;
    [SerializeField] private GameObject guneffect;
    [SerializeField] private Rigidbody therigid;
    [SerializeField] private PhotonView pv;
    [SerializeField] private PhotonView pv2;
    private PhotonView pv0;
    private PlayerController theplayer;
    private HP thehp;
    // Start is called before the first frame update
    void Start()
    {
        theplayer = FindObjectOfType<PlayerController>();
        thehp = FindObjectOfType<HP>();
        Destroy(this.gameObject, survivaltime);
    }
    public void Fire(float _firespeed,int layer)
    {
        speed += _firespeed; 
        gameObject.layer = layer; //플레이어와 총알이 겹치지 않게 하기위한 레이어
        therigid.velocity = transform.forward * speed; //총알을 스피드만큼 전방으로 이동시킴
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player")) //player 와 닿았을 경우
        {
            //
            if (other.gameObject.GetComponent<PhotonView>().Owner.ActorNumber.Equals((int)other.gameObject.GetComponent<PlayerController>().mn)) //상대 플레이어라면
            {
                theplayer.guneffectaudio.clip = theplayer.missileclip; 
                theplayer.guneffectaudio.Play(); //사운드 재생

                Explode();

                other.gameObject.GetComponent<HP>().Damage(4); //hp 감소
                Destroy(this.gameObject); //총알 파괴
            }
        }

    }

    private void Awake()
    {

    }
    void Explode()
    {
        PhotonNetwork.Instantiate("effect", transform.position, Quaternion.identity, 0); //서버에서 이펙트 생성하여 다른 플레이어에게도 이펙트가 보이게함
    }
}
