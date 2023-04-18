using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class PlayerController : MonoBehaviourPun
{
    [SerializeField] private float pitchAmount;
    [SerializeField] private float yawAmount;
    [SerializeField] private float rollAmount;
    [SerializeField] private float speed;
    [SerializeField] private float mX, mY, mZ;
    [SerializeField] private Vector3 rotateValue;
    [SerializeField] private Rigidbody therigid;
    [SerializeField] private float lerpAmount;
    [SerializeField] private float maxSpeed = 2000f;
    [SerializeField] private float defaultspeed = 1000;
    [SerializeField] private float minSpeed = 1000f;
    [SerializeField] private float acceler;
    [SerializeField] private float BreakValue;
    [SerializeField] private float breakAmount;
    [SerializeField] private Vector3 particlepos1;
    [SerializeField] private Transform particlepos2;
    [SerializeField] private ParticleSystem boost;
    [SerializeField] private Text speedtext;
    [SerializeField] private Image accelimage;
    [SerializeField] private Image breakimage;
    [SerializeField] private Image defaultimage;
    [SerializeField] private AudioClip defaultsound;
    [SerializeField] private AudioClip accelsound;
    [SerializeField] private AudioClip breaksound;
    [SerializeField] private AudioSource movesound;
    [SerializeField] private AudioClip upspeed;
    [SerializeField] public AudioClip bulletclip;
    [SerializeField] public AudioSource guneffectaudio;
    [SerializeField] public AudioClip missileclip;
    [SerializeField] private Text ALTimage;
    public bool isaccel=false;
    public bool isbreak=false;
    public bool isdefault = false;
    public float calibreateAmount;
    private float speedReciprocal;
    private float accelervalue;
    public enum my_number { master=1, client=2}
    public my_number mn;
    private GAMESCENEUI thegameui;
    [SerializeField] PhotonView pv;
    [SerializeField] Camera zcamera;
    [SerializeField] Camera cmamera;
    [SerializeField] Camera maincamera;
    private Canvas canvas;
    private void camerachange()
    {
        if(Input.GetKey(KeyCode.Z)) //키에 따른 카메라 전환
        {
            zcamera.gameObject.SetActive(true);
            maincamera.gameObject.SetActive(false);
        }
        if (Input.GetKeyUp(KeyCode.Z))
        {
            maincamera.gameObject.SetActive(true);
            zcamera.gameObject.SetActive(false);
        }
        if (Input.GetKey(KeyCode.C))
        {
            maincamera.gameObject.SetActive(false);
            cmamera.gameObject.SetActive(true);
        }
        if (Input.GetKeyUp(KeyCode.C))
        {
            maincamera.gameObject.SetActive(true);
            cmamera.gameObject.SetActive(false);
        }
    }
    void Start()
    {
        speedReciprocal = 1 / maxSpeed; //속도 초기화
        mn = (PhotonNetwork.IsMasterClient) ? my_number.master : my_number.client; //플레이어 구분
        thegameui = FindObjectOfType<GAMESCENEUI>();
        Debug.Log("load complete");
    }
    private void Awake()
    {
        ALTimage = GameObject.Find("ALTTEXT").GetComponent<Text>();
        speedtext = GameObject.Find("speed").GetComponent<Text>();
        accelimage = GameObject.Find("activeaccel").GetComponent<Image>();
        breakimage = GameObject.Find("activebreak").GetComponent<Image>();
        defaultimage = GameObject.Find("noneactive").GetComponent<Image>();
    }
    void Update()
    {
        
            if (pv.IsMine) //클라이언트가 나라면
            {
            ALTimage.text = "ALT " + transform.position.y.ToString("F0");
            camerachange();
            PlaySE();
                Cursor.visible = false;
                Cursor.lockState = CursorLockMode.Locked;
                Moving();
                UICheck();
                ImageBool();
                
               
            }
        
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }


    }
    private void PlaySE()
    {
        if (!movesound.isPlaying)
            movesound.Play();
    }
    private void ImageBool()
    {
        if (isaccel) //W 가 누른상태 즉 가속상태라면
        {
            if (speed >= 1500 && speed <= 3450) //속도가 일정수준 이상 및 이하라면
            {
                movesound.clip = upspeed; //가속사운드 재생
            }
            else if(speed>=3451)
            {
                movesound.clip = accelsound;
            }
            accelimage.gameObject.SetActive(true);
            breakimage.gameObject.SetActive(false);
            defaultimage.gameObject.SetActive(false);
            isbreak = false;
            isdefault = false;
        }
        else if (isbreak) //S 를 누른 브레이크 상태라면
        {
            movesound.clip = breaksound;
            breakimage.gameObject.SetActive(true);
            accelimage.gameObject.SetActive(false);
            defaultimage.gameObject.SetActive(false);
            isaccel = false;
            isdefault = false;
        }
        else if (isdefault) //W S 아무것도 안누른 기본상태라면
        {
            movesound.clip = defaultsound;
            defaultimage.gameObject.SetActive(true);
            accelimage.gameObject.SetActive(false);
            breakimage.gameObject.SetActive(false);
            isaccel = false;
            isbreak = false;
        }
        else //그외에
        {
            isaccel = false;
            isbreak = false;
            isdefault = false;
        }

    }
    private void UICheck()
    {
        speedtext.text = (speed / 10).ToString("F1") + "Km/h";
    }
    private void Moving() //움직임 코드
    {
        MoveAirCraft();
        mX = Input.GetAxis("Mouse Y");
        mY = Input.GetAxis("Mouse X");
        mZ = Input.GetAxis("Horizontal");

    }

    private void MoveAirCraft()
    {
        Vector3 lerpV3 = new Vector3(mX * pitchAmount, mY * yawAmount, mZ * -rollAmount); //lerpV3 에 X, Y ,Z 값 저장
        rotateValue = Vector3.Lerp(rotateValue, lerpV3, lerpAmount * Time.deltaTime); //천천히 회전시킴
        therigid.MoveRotation(therigid.rotation * Quaternion.Euler(rotateValue * Time.deltaTime)); //리지드 바디로 움직이게함

       
        if (Input.GetKey(KeyCode.W)) //가속상태일때
        {
            float accelerEase = (maxSpeed - speed) * speedReciprocal;
            speed += 1 * acceler * accelerEase * Time.deltaTime; //속도를 점차 가속시킴
            therigid.velocity = transform.forward * speed * Time.deltaTime;
            isaccel = true;
            playereffect();
        }
        if (Input.GetKeyUp(KeyCode.W))
            isaccel = false;
        if(Input.GetKey(KeyCode.S)) //감속 상태일때
        {
            float breakEase = (speed - minSpeed) * speedReciprocal;
            speed -= 1 * breakAmount * breakEase * Time.deltaTime;  //속도를 점차 감속시킴
            therigid.velocity = transform.forward * speed * Time.deltaTime;
            isbreak = true;
        }
        if (Input.GetKeyUp(KeyCode.S))
            isbreak = false;
        if (accelervalue==0 && BreakValue==0) //감속, 가속 둘다아닐때
        {
            speed += (defaultspeed - speed) * speedReciprocal * calibreateAmount * Time.deltaTime; //평균속도로 만듬
            therigid.velocity = transform.forward * speed * Time.deltaTime;
            isdefault = true;
        }
        therigid.velocity = transform.forward * speed *Time.deltaTime;
    }
    void playereffect()
    {
        PhotonNetwork.Instantiate("momomomving", particlepos2.transform.position, Quaternion.identity);
    }
}
