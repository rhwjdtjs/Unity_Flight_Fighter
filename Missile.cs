using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Photon.Pun;
using Photon.Realtime;
public class Missile : MonoBehaviourPun
{
    private Transform Target;
    public int actornumber;
    [SerializeField] private float turningForce;
    [SerializeField] private float maxspeed;
    [SerializeField] private float acceler;
    [SerializeField] private float SurviveTime;
    [SerializeField] private float speed;
    [SerializeField] private GameObject bombeffect;
    [SerializeField] private float sightangle;
    [SerializeField] private GameObject particlepos1;
    private PlayerController theplayer;
    private HP thehp;
    private RockOn therockon;
    private Rockon2 therockon2;
    private Rigidbody rb;
    [SerializeField] private PhotonView pv;
    [SerializeField] private PhotonView pv2;
    private PhotonView pv0;
    public void LookAtTarget() //락온됬을때 적플레이어를 따라감
    {
        if (Target == null)
            return;
        Vector3 targetDir = Target.position - transform.position; //타겟과의 거리 계산
        float angle = Vector3.Angle(targetDir, transform.forward); //타겟과의 전방벡터 사이의 각도를 구함

        if(angle>sightangle) //각도내에 없으면
        {
            Target = null;
            return;
        }
        Quaternion lookrotation = Quaternion.LookRotation(targetDir); //타겟의 위치를 바라봄
        transform.rotation = Quaternion.Slerp(rb.rotation, lookrotation, turningForce * Time.deltaTime); //회전값
    }
    public void LaunchFire(Transform target, float firespeed,int layer)
    {
        this.Target = target;
        speed = firespeed;
        gameObject.layer = layer;//레이어가 겹치지 않게 하기위함
    }
    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        therockon2 = FindObjectOfType<Rockon2>();
        theplayer = FindObjectOfType<PlayerController>();
        thehp = FindObjectOfType<HP>();
        Destroy(this.gameObject, SurviveTime);
        therockon = FindObjectOfType<RockOn>();
    }

    public void firemoving()
    {
        if(speed<maxspeed)
        {
            speed += acceler * Time.deltaTime; //가속을 받으면서 점차 속도가 증가함
        }
        rb.velocity = transform.forward * speed; //앞으로 이동
       
    }

    // Update is called once per frame
    void Update()
    {
        playereffect();
        firemoving();
        LookAtTarget();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.tag.Equals("Player")) //플레이어가 맞았을경우
        {
            //
            if (other.gameObject.GetComponent<PhotonView>().Owner.ActorNumber.Equals((int)other.gameObject.GetComponent<PlayerController>().mn)) //적플레이어 일경우
            {
                theplayer.guneffectaudio.clip = theplayer.missileclip;
                theplayer.guneffectaudio.Play();

                Explode();

                other.gameObject.GetComponent<HP>().Damage(30);
                Destroy(this.gameObject);
            }
        }
    }
    void Explode()
    { 
        PhotonNetwork.Instantiate("effect2", transform.position, Quaternion.identity, 0); //이펙트가 보이기 위함
    }
    void playereffect()
    {
        PhotonNetwork.Instantiate("WFX_SmokeGrenade AlphaBlend Black", particlepos1.transform.position, Quaternion.identity); //이펙트가 보이기위함
    }
}
