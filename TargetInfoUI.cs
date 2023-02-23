using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class TargetInfoUI : MonoBehaviourPun
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
        targettransform = themul.theplayer1.transform;
        if (photonView.IsMine)
            TargetUIFollow();
    }

   
    private void TargetUIFollow()
    {
        targetpos = (targettransform.position - playertransform.position).magnitude;
        TargetPosText.text = targetpos.ToString("F1") + "M";
        cantseeTargetposTEXT.text = targetpos.ToString("F1") + "M";

         screenPos = camera.WorldToScreenPoint(targettransform.position);
     
         targetimage.transform.position = new Vector3(screenPos.x, screenPos.y, 0);

        rockonimage.transform.position = new Vector3(screenPos.x, screenPos.y, 0);
        if (screenPos.z<0)
        {
            targetimage.gameObject.SetActive(false);
            targetTGT.gameObject.SetActive(false);
            cantseetarget.SetActive(true);
            rockonimage.gameObject.SetActive(false);
        }
        else
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
