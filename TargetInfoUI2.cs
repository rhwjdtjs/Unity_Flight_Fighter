using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
using Photon.Realtime;
public class TargetInfoUI2 : MonoBehaviourPun
{
    [SerializeField] public Text TargetPosText2;
    [SerializeField] public Text cantseeTargetposTEXT2;
    [SerializeField] public float targetpos;
    [SerializeField] public Transform targettransform;
    [SerializeField] public Transform playertransform;
    [SerializeField] public Image targetimage2;
    [SerializeField] public GameObject targetobject;
    [SerializeField] private Camera camera;
    [SerializeField] public Vector3 screenPos;
    [SerializeField] public Text targetTGT2;
    [SerializeField] public GameObject cantseetarget2;
    [SerializeField] public Image rockonimage2;
    [SerializeField] private Text enemyposy;
    private Rockon2 therockon;
    private MultyManager themul;

    void Start()
    {
        therockon = FindObjectOfType<Rockon2>();
        themul = FindObjectOfType<MultyManager>();
    }
    private void Awake()
    {
        this.rockonimage2 = GameObject.Find("rockontargetpos").GetComponent<Image>();
        this.TargetPosText2 = GameObject.Find("Targetdistance").GetComponent<Text>();
        this.cantseeTargetposTEXT2 = GameObject.Find("cantseedistance").GetComponent<Text>();
        this.targetimage2 = GameObject.Find("TargetPos").GetComponent<Image>();
        this.targetTGT2 = GameObject.Find("TGTACTIVE").GetComponent<Text>();
        this.cantseetarget2 = GameObject.Find("NotSEEememy");
  
    }
    private void LateUpdate()
    {

        this.targettransform = themul.theplayer2.transform;
        if (photonView.IsMine)
            this.TargetUIFollow();
    }

    private void TargetUIFollow()
    {
        this.targetpos = (targettransform.position - playertransform.position).magnitude;
        this.TargetPosText2.text = targetpos.ToString("F1") + "M";
        this.cantseeTargetposTEXT2.text = targetpos.ToString("F1") + "M";
        this.screenPos = camera.WorldToScreenPoint(this.targettransform.position);
        this.targetimage2.transform.position = new Vector3(screenPos.x, screenPos.y, 0);
        this.rockonimage2.transform.position = new Vector3(screenPos.x, screenPos.y, 0);
        if (screenPos.z < 0)
        {
            this.targetimage2.gameObject.SetActive(false);
            this.targetTGT2.gameObject.SetActive(false);
            this.cantseetarget2.SetActive(true);
            this.rockonimage2.gameObject.SetActive(false);
        }
        else
        {
            if (!therockon.CheckSight())
            {
                this.targetimage2.gameObject.SetActive(true);
            }
            this.targetTGT2.gameObject.SetActive(true);
            this.cantseetarget2.SetActive(false);
        }
    }
}
