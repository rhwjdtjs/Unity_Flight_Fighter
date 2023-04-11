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
        gameObject.layer = layer;
        therigid.velocity = transform.forward * speed;
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player"))
        {
            //
            if (other.gameObject.GetComponent<PhotonView>().Owner.ActorNumber.Equals((int)other.gameObject.GetComponent<PlayerController>().mn))
            {
                theplayer.guneffectaudio.clip = theplayer.missileclip;
                theplayer.guneffectaudio.Play();

                Explode();

                other.gameObject.GetComponent<HP>().Damage(4);
                Destroy(this.gameObject);
            }
        }

    }

    private void Awake()
    {

    }
    void Explode()
    {
        PhotonNetwork.Instantiate("effect", transform.position, Quaternion.identity, 0);
    }
}
