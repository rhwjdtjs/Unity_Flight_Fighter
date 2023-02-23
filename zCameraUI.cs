using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class zCameraUI : MonoBehaviour
{
    [SerializeField] private Text TargetPosText;
    [SerializeField] private Text cantseeTargetposTEXT;
    [SerializeField] private float targetpos;
    [SerializeField] private Transform targettransform;
    [SerializeField] private Transform playertransform;
    [SerializeField] private Image targetimage;
    [SerializeField] private GameObject targetobject;
    [SerializeField] new private Camera camera;
    [SerializeField] private Vector3 screenPos;
    [SerializeField] private Text targetTGT;
    [SerializeField] private GameObject cantseetarget;
    [SerializeField] public Image rockonimage;
    private RockOn therockon;
    void Start()
    {
        targetimage.gameObject.SetActive(true);
        therockon = FindObjectOfType<RockOn>();
        camera = Camera.main;
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
    void Update()
    {
        TargetUIFollow();
    }


    
    private void TargetUIFollow()
    {
        targetpos = (targettransform.position - playertransform.position).magnitude;
        TargetPosText.text = targetpos.ToString("F1") + "M";
        cantseeTargetposTEXT.text = targetpos.ToString("F1") + "M";
        screenPos = camera.WorldToScreenPoint(targettransform.position);
        targetimage.rectTransform.position = new Vector3(screenPos.x, screenPos.y, 0);
        rockonimage.rectTransform.position = new Vector3(screenPos.x, screenPos.y, 0);
        if (screenPos.z > 0)
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
