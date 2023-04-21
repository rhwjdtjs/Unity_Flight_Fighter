using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class Rockon2 : MonoBehaviourPun //Rockon 과 동일한 스크립트 각각의 플레이어에게 붙음
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
    private WeaponnManager2 theweapon;
    private bool istimeover = false;
    private TargetInfoUI2 thetargetui;
    public float rangeAngle = 100f;
    public float rangeDistance = 100f;
    public float timer = 4;
    [SerializeField] AudioSource rockonsound;
    [SerializeField] AudioClip rockeffect;
    [SerializeField] AudioClip beforerockeffect;
    [SerializeField] AudioSource beforerocksound;
    [SerializeField] AudioSource rockingme;
    [SerializeField] AudioClip rockingmeclip;
    private RockOn theenemy;
    private void Awake()
    {
        rockoncrosshair = GameObject.Find("rockonCrossHair").GetComponent<Image>();
        rockonimage = GameObject.Find("rockontargetpos").GetComponent<Image>();
        Origintargetimage = GameObject.Find("TargetPos").GetComponent<Image>();
        waringimage = GameObject.Find("Waring").GetComponent<Image>();

    }
    void Start()
    {
        theenemy = FindObjectOfType<RockOn>();
        theweapon = FindObjectOfType<WeaponnManager2>();
        thetargetui = FindObjectOfType<TargetInfoUI2>();

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
    private void enemyrocksound()
    {


        if (theenemy.CheckSight())
        {
            waringimage.gameObject.SetActive(true);
            rockingme.clip = rockingmeclip;
            if (!rockingme.isPlaying)
                rockingme.PlayOneShot(rockingmeclip);

        }
        
        
    }
    private IEnumerator rockingsound()
    {
        if (CheckSight())
        {
            if (!theweapon.isFire)
            {
                
                rockonsound.GetComponent<AudioSource>().enabled = true;
                if (!rockonsound.isPlaying)
                {
                    rockonsound.Play();
                }
                yield return new WaitForSeconds(1f);
            }
        }
    }
    private void RockonState()
    {
        if (CheckSight())
        {
            if (!theweapon.isFire)
            {

                Origintargetimage.gameObject.SetActive(false);
                rockoncrosshair.gameObject.SetActive(true);
                rockonimage.gameObject.SetActive(true);
            }
        }
        else if (!CheckSight())
        {
            waringimage.gameObject.SetActive(false);
            rockonsound.Stop();
            Origintargetimage.gameObject.SetActive(true);
            rockoncrosshair.gameObject.SetActive(false);
            rockonimage.gameObject.SetActive(false);
        }
    }
    public bool CheckSight()
    {
        Collider[] _target = Physics.OverlapSphere(transform.position, viewDistance, targetMask);

        for (int i = 0; i < _target.Length; i++)
        {
            Transform _targetTf = _target[i].transform;
            Vector3 _direction = (_targetTf.position - transform.position).normalized;
            float _angle = Vector3.Angle(_direction, transform.forward);
            if (viewAngle * 0.2f < _angle && _angle < viewAngle)
            {
                RaycastHit __hit;
                if (Physics.Raycast(transform.position + transform.forward, _direction, out __hit, viewDistance))
                {
                    if (__hit.transform.tag == "Player")
                    {
                        Debug.Log("!");
                        if (!theweapon.isFire)
                        {
                            if (!beforerocksound.isPlaying)
                                beforerocksound.PlayOneShot(beforerockeffect);
                        }
                        return false;
                    }
                }
            }
            else if (_angle < viewAngle * 0.5f)
            {
                RaycastHit _hit;

                if (Physics.Raycast(transform.position + transform.forward, _direction, out _hit, viewDistance))
                {

                    if (_hit.transform.tag == "Player")
                    {
                        Debug.DrawRay(transform.position + transform.forward, _direction, Color.blue);
                        return true;
                    }
                }
            }

        }
        return false;
    }
}
