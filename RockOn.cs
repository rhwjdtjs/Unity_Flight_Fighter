using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class RockOn : MonoBehaviourPun
{
    [SerializeField] private float viewAngle;  // 시야 각도
    [SerializeField] private float viewDistance; // 시야 거리 
    [SerializeField] private LayerMask targetMask;  // 타겟 마스크
    [SerializeField] private GameObject[] target;
    [SerializeField] private Image rockoncrosshair;
    [SerializeField] private Image rockonimage;
    [SerializeField] private bool isrockon;
    [SerializeField] private Image Origintargetimage;
    [SerializeField] private Image waringimage;
    private WeaponManager theweapon;
    private bool istimeover=false;
    private TargetInfoUI thetargetui;
    public float rangeAngle = 100f;
    public float rangeDistance = 100f;
    public float timer = 4;
    [SerializeField] AudioSource rockonsound;
    [SerializeField] AudioClip rockeffect;
    [SerializeField] AudioClip beforerockeffect;
    [SerializeField] AudioSource beforerocksound;
    [SerializeField] AudioSource rockingme;
    [SerializeField] AudioClip rockingmeclip;
    private Rockon2 theenemy;
    private void Awake()
    {
        rockoncrosshair = GameObject.Find("rockonCrossHair").GetComponent<Image>();
        rockonimage = GameObject.Find("rockontargetpos").GetComponent<Image>();
        Origintargetimage = GameObject.Find("TargetPos").GetComponent<Image>();
        waringimage = GameObject.Find("Waring").GetComponent<Image>();


    }
    void Start()
    {
        theenemy = FindObjectOfType<Rockon2>();
        theweapon = FindObjectOfType<WeaponManager>();
        thetargetui = FindObjectOfType<TargetInfoUI>();
        
    }
    // Update is called once per frame
    void Update()
    {
        if (photonView.IsMine)
        {
            CheckSight();
            isrockon = CheckSight();
            RockonState();
            Debug.Log(isrockon);
            StartCoroutine(rockingsound());
            enemyrocksound();
        }
       
    }
    
   private IEnumerator rockingsound()
    {
        if (CheckSight()) //만약 락온상태라면
        {
            if (!theweapon.isFire) //무기가 미사일이라면
            {
                rockonsound.GetComponent<AudioSource>().enabled = true;
                if (!rockonsound.isPlaying)
                {
                    rockonsound.Play(); //락온 상태중 삐~~~사운드 출력함
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
    private void enemyrocksound()
    {
        
            if (theenemy.CheckSight())//만약 적이 나를 락온중이라면
            {
            waringimage.gameObject.SetActive(true);
                rockingme.clip = rockingmeclip;
                if (!rockingme.isPlaying)
                    rockingme.PlayOneShot(rockingmeclip);//위험 사운드 출력
            }

        
       
    }
    private void RockonState()
    {
        if(CheckSight()) //만약 락온상태라면
        {
            if (!theweapon.isFire) //마시알이라면
            {
                Origintargetimage.gameObject.SetActive(false);
                rockoncrosshair.gameObject.SetActive(true);
                rockonimage.gameObject.SetActive(true);
                //ui 수정 코드
            }
        }
        else if(!CheckSight()) //락온상태가 아니라면
        {
            waringimage.gameObject.SetActive(false);
            rockonsound.Stop();
            Origintargetimage.gameObject.SetActive(true);
            rockoncrosshair.gameObject.SetActive(false);
            rockonimage.gameObject.SetActive(false);
            //ui 수정 코드
        }
    }
    public bool CheckSight()
    {
        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask); //플레이어 기준으로 원을 그려 주위에 콜라이더를 확인해 저장함

        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform; //타겟의 위치를 저장
            Vector3 _direction = (_targetTf.position - transform.position).normalized; //타겟의 위치와 나의 위치 사이의 벡터값을 구함
            float _angle = Vector3.Angle(_direction, transform.forward); //구한 벡터값과 전방 벡터사이의 각도를 구함
            if (viewAngle*0.2f<_angle && _angle<viewAngle ) //그 각도가 일정 각도 이상 일정 각도 이하라면 대략 400m~600미터 이하
            {
                RaycastHit __hit;
                if (Physics.Raycast(transform.position + transform.forward, _direction, out __hit, viewDistance))
                {
                    if (__hit.transform.tag == "Player")
                    {
                        Debug.Log("!");
                        if (!theweapon.isFire) //미사일 무기 모드라면
                        {
                            if (!beforerocksound.isPlaying)
                                beforerocksound.PlayOneShot(beforerockeffect); //락온 되기전 삐삐삐 사운드 출력
                        }
                        return false; //락온 상태가 아니어야하니 false 반환
                    }
                }
            }
            else if (_angle < viewAngle * 0.5f) //각도가 0.5f 정도이면
            {
                RaycastHit _hit;

                if (Physics.Raycast(transform.position + transform.forward, _direction, out _hit, viewDistance))
                {

                    if (_hit.transform.tag == "Player")
                    {
                        Debug.DrawRay(transform.position + transform.forward, _direction, Color.blue);
                        return true; //값을 true로 반환시켜 락온이 된 상태를 나타냄
                    }
                }
            }
            
        }
        return false;
    }
}
