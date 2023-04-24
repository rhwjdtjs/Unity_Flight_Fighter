using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class TargetInfoUI : MonoBehaviourPun //타겟의 위치를 네모 아이콘으로 표시해주는 스크립트
{
    [SerializeField] public Text TargetPosText;
    [SerializeField] public Text cantseeTargetposTEXT;
    [SerializeField] public float targetpos;
    [SerializeField] public Transform targettransform;
    [SerializeField] public Transform playertransform;
    [SerializeField] public Image targetimage;
    [SerializeField] public GameObject targetobject;
    [SerializeField] private Camera camera;
    [SerializeField] public Vector3 screenPos;
    [SerializeField] public Text targetTGT;
    [SerializeField] public GameObject cantseetarget;
    [SerializeField] public Image rockonimage;
    [SerializeField] RectTransform rect;
    [SerializeField] private Text enemyposy;
    private RockOn therockon;
    private MultyManager themul;
    public GameObject targeting;
    void Start()
    {
        therockon = FindObjectOfType<RockOn>();
        themul = FindObjectOfType<MultyManager>();
    }
    private void Awake()
    {
        cantseeTargetposTEXT = GameObject.Find("cantseedistance").GetComponent<Text>();
        TargetPosText = GameObject.Find("Targetdistance").GetComponent<Text>();
        targetimage = GameObject.Find("TargetPos").GetComponent<Image>();
        targetTGT = GameObject.Find("TGTACTIVE").GetComponent<Text>();
        cantseetarget = GameObject.Find("NotSEEememy");
        rockonimage = GameObject.Find("rockontargetpos").GetComponent<Image>();
        

    }
    // Update is called once per frame
    private void LateUpdate()
    {
        targettransform = themul.theplayer1.transform; //타겟의 위치를 매번 계산
        if (photonView.IsMine)
            TargetUIFollow();
    }

   
    private void TargetUIFollow()
    {
        targetpos = (targettransform.position - playertransform.position).magnitude; //타겟의 위치와 자신의 위치 사이의 거리를 구함
        TargetPosText.text = targetpos.ToString("F1") + "M"; //그값을 표시
        cantseeTargetposTEXT.text = targetpos.ToString("F1") + "M"; //전방에 없는 적의 거리를 계산하고 표시

         screenPos = camera.WorldToScreenPoint(targettransform.position); //월드 좌표를 평면으로 구함
     
         targetimage.transform.position = new Vector3(screenPos.x, screenPos.y, 0); //타겟 위치를 보여주는 이미지의 위치 표시

        rockonimage.transform.position = new Vector3(screenPos.x, screenPos.y, 0); //라공ㄴ시에도 마찬가지로 표시
        if (screenPos.z<0) //적이 전방에 없을때 맞는 아이콘 표시
        {
            targetimage.gameObject.SetActive(false);
            targetTGT.gameObject.SetActive(false);
            cantseetarget.SetActive(true);
            rockonimage.gameObject.SetActive(false);
        }
        else //적이 전방에 있을때
        {
            if (!therockon.CheckSight())
            {
                targetimage.gameObject.SetActive(true);
            }
            targetTGT.gameObject.SetActive(true);
            cantseetarget.SetActive(false);
        }
    }
}
