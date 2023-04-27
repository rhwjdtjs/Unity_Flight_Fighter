using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
[RequireComponent(typeof(speedController))]
public class WeaponManager : MonoBehaviourPun
{
    
    [SerializeField] public bool useWeapon=false;
    [SerializeField] public bool isFire=false;
    [SerializeField] public GameObject missile;
    [SerializeField] private speedController thespeed;
    [SerializeField] public int missileCnt;
    [SerializeField] private Transform leftmissletransform;
    [SerializeField] private Transform rightmissiletransform;
    [SerializeField] public float missilecool;
    [SerializeField] private float rightcool;
    [SerializeField] private float leftcool;
    [SerializeField] public Transform target;
    [SerializeField] private int bulletCnt;
    private Vector3 FireTransforms;
    [SerializeField] private GameObject bullets;
    [SerializeField] private Transform firetransform;
    [SerializeField] private float bulletcool;
    [SerializeField] private float guncooldown;
    [SerializeField] private Image machineUI;
    [SerializeField] private Image missileUI;
    [SerializeField] private Text guntext;
    [SerializeField] private Text missiletext;
    [SerializeField] private AudioSource theaudiomacine;
    [SerializeField] private AudioSource theaudiomissile;
    [SerializeField] private Image gunimage;
    [SerializeField] private Image missileimage;
    [SerializeField] private PhotonView pv;
    public GameObject Targettrans;
    private RockOn rockingsystem;
    private MultyManager themul;
    private GameObject targeting;
    
    // Start is called before the first frame update
    void Start()
    {
        themul = FindObjectOfType<MultyManager>();
        rockingsystem = FindObjectOfType<RockOn>();
        isFire = true;
        missiletext.gameObject.SetActive(false);
        machineUI.gameObject.SetActive(true);
        missileUI.gameObject.SetActive(false);
        gunimage.gameObject.SetActive(true);
        missileimage.gameObject.SetActive(false);
    }
    
    private void Awake()
    {
        gunimage = GameObject.Find("bulletcnt").GetComponent<Image>();
        missileimage = GameObject.Find("missilecnt").GetComponent<Image>();
        guntext = GameObject.Find("bullettext").GetComponent<Text>();
        missiletext = GameObject.Find("missiletext").GetComponent<Text>();
        machineUI = GameObject.Find("machinecrosshair").GetComponent<Image>();
        missileUI = GameObject.Find("missilecrosshar").GetComponent<Image>();
    }
    private void cntTEXT()
    {
        Debug.Log("123");
        if (isFire) //1번 키 즉 일반총일때
            guntext.text = bulletCnt.ToString();
        else //미사일 일때
            missiletext.text = missileCnt.ToString();
    }
    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
           
                cntTEXT();
                Fire();
                GunFire();
                switchweapon();
                missilecooldown(ref rightcool);
                missilecooldown(ref leftcool);
                machineguncooldown(ref guncooldown);
            
        }
        
    }
    private void LateUpdate()
    {
       
        target = themul.theplayer1.transform; //타겟의 위치를 매번 측정
    }
    public void Fire()
    {
        Debug.Log("1234");
        if (Input.GetButtonDown("Fire1"))
        {
            Debug.Log("fire");
            if (!isFire) //현재 무기가 미사일이라면
            {
                CancelInvoke("FireMachineGun"); //함수 취소
                if (rockingsystem.CheckSight()) //락상태라면
                {
                    Debug.Log("fire2");
                    pv.RPC("FireMissile", RpcTarget.All); //미사일 발사
                }
              
            }
        }
        if(Input.GetButton("Fire1"))
        {
            if (isFire)
            {
                Debug.Log("fire1111");
                pv.RPC("FireMachineGun", RpcTarget.All); //머신건 발사
            }
        }
        Debug.Log("FRIEFUNCTION");
    }
    [PunRPC]
    void FireMachineGun()
    {
        Debug.Log("12345");
        if (bulletCnt<=0) //총알이 없다면
        {
            CancelInvoke("FireMachineGun"); //함수 취소
            return;
        }
        if (guncooldown > 0) //쿨타임이라면 취소
            return;
        guncooldown = bulletcool; //쿨타임
        theaudiomacine.Play();
        FireTransforms = firetransform.position;
        GameObject bullet =PhotonNetwork.Instantiate("Bullet1", FireTransforms, transform.rotation,0) as GameObject; //총알을 네트워크로 생성
        Bullet bulletscript = bullet.GetComponent<Bullet>();
        bulletscript.Fire(thespeed.speed*10, gameObject.layer); //불렛 속도
        bulletCnt--;
    }
    public void machineguncooldown(ref float cooldown) //머신건 쿨타임
    {
        Debug.Log("123456");
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            if (cooldown < 0)
                guncooldown = 0;
        }
        else
            return;
    }
    public void GunFire()
    {
       
        if (Input.GetKeyDown(KeyCode.Alpha1)) //머신건 장착
        {
            gunimage.gameObject.SetActive(true);
            missileimage.gameObject.SetActive(false);
            isFire = true;
            missiletext.gameObject.SetActive(false);
            guntext.gameObject.SetActive(true);
            machineUI.gameObject.SetActive(true);
            missileUI.gameObject.SetActive(false);
            return;
        }
        if(Input.GetKeyDown(KeyCode.Alpha2)) //미사일 장착
        {
            gunimage.gameObject.SetActive(false);
            missileimage.gameObject.SetActive(true);
            isFire = false;
            missiletext.gameObject.SetActive(true);
            machineUI.gameObject.SetActive(false);
            guntext.gameObject.SetActive(false);
            missileUI.gameObject.SetActive(true);
            return;
        }
        Debug.Log("GUNFUNCTION");
    }
    public void switchweapon() //무기 교환
    {
        Debug.Log("12345678");
        if (Input.GetKeyDown(KeyCode.Alpha1))
            useWeapon = !useWeapon;
    }
    [PunRPC]
    void FireMissile() //미사일 발사
    {
        Debug.Log("123456789");
        if (missileCnt <= 0) //미사일이 없으면 반환
            return;
        if (leftcool > 0 && rightcool > 0) //쿨타임이 0초가아니면 반환
            return;

        Vector3 missileposition;
        if(missileCnt %2==1) //미사일을 쏘면 양날개 좌우에서 한발씩 나감
        {
            missileposition = rightmissiletransform.position;
            rightcool = missilecool;
        }
        else
        {
            missileposition = leftmissletransform.position;
            leftcool = missilecool;
        }
        GameObject Missile = PhotonNetwork.Instantiate("Missil1", missileposition, transform.rotation, 0) as GameObject;
        Missile mscript = Missile.GetComponent<Missile>();
        mscript.LaunchFire(target, thespeed.speeds * 15,gameObject.layer);
        theaudiomissile.Play();
        missileCnt--;
    }
    public void missilecooldown(ref float cooldown) //미사일 쿨타임 계산
    {
        Debug.Log("2");
        if (cooldown > 0)
        {
            cooldown -= Time.deltaTime;
            if (cooldown < 0)
                cooldown = 0;
        }
        else
            return;
    }
}
