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
    public void LookAtTarget()
    {
        if (Target == null)
            return;
        Vector3 targetDir = Target.position - transform.position;
        float angle = Vector3.Angle(targetDir, transform.forward);

        if(angle>sightangle)
        {
            Target = null;
            return;
        }
        Quaternion lookrotation = Quaternion.LookRotation(targetDir);
        transform.rotation = Quaternion.Slerp(rb.rotation, lookrotation, turningForce * Time.deltaTime);
    }
    public void LaunchFire(Transform target, float firespeed,int layer)
    {
        this.Target = target;
        speed = firespeed;
        gameObject.layer = layer;
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
            speed += acceler * Time.deltaTime;
        }
        rb.velocity = transform.forward * speed;
       
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
        if (other.tag.Equals("Player"))
        {
            //
            if (other.gameObject.GetComponent<PhotonView>().Owner.ActorNumber.Equals((int)other.gameObject.GetComponent<PlayerController>().mn))
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
        PhotonNetwork.Instantiate("effect2", transform.position, Quaternion.identity, 0);
    }
    void playereffect()
    {
        PhotonNetwork.Instantiate("WFX_SmokeGrenade AlphaBlend Black", particlepos1.transform.position, Quaternion.identity);
    }
}
