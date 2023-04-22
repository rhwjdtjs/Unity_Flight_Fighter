using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class SceneManage : MonoBehaviourPunCallbacks //게임 시작하기전에 컷신
{
    private float FadeTime = 2f;
    public Image fadeimg;
    float start;
    float end;
    float time = 0f;
    bool isplaying = false;
    private Rigidbody therigid;
    // Start is called before the first frame update
    private void moving()
    {
        therigid.AddForce(Vector3.left * 50); //왼쪽으로 점차 가속시킴
    }
    void Start()
    {
        therigid = GetComponent<Rigidbody>();
        InStartFadeAnim();
        StartCoroutine(loadsceneco());
    }

    // Update is called once per frame
    void Update()
    {
        moving(); //매프레임마다 실행
    }
    IEnumerator loadsceneco()
    {
        yield return new WaitForSeconds(5f); //5초가 지난후에
        OutStartFadeAnim(); //함수 실행
        

    }
    public void OutStartFadeAnim()
    {
        if (isplaying == true)
            return;
        end = 1f;
        start = 0f;
        StartCoroutine("fadeoutplay"); //페이드 아웃실행
    }
    public void InStartFadeAnim()
    {
        if (isplaying)
            return;
        start = 1f;
        end = 0f;
        StartCoroutine("fadeInanim"); //페이드 인실행
    }
    IEnumerator fadeoutplay()
    {
        isplaying = true;

        Color fadecolor = fadeimg.color;
        time = 0f;
        fadecolor.a = Mathf.Lerp(start, end, time);

        while (fadecolor.a <1f)
        {
            time += Time.deltaTime / FadeTime;
            fadecolor.a = Mathf.Lerp(start, end, time); //색깔을 점점 어둡게함
            fadeimg.color = fadecolor;
            yield return null;
        }
        isplaying = false;
        PhotonNetwork.LoadLevel("Gamescene"); //게임시작함

    }
    IEnumerator fadeInanim()
    {
        isplaying = true;

        Color fadecolor = fadeimg.color;
        time = 0f;
        fadecolor.a = Mathf.Lerp(start, end, time);

        while(fadecolor.a>0f)
        {
            time += Time.deltaTime / FadeTime;
            fadecolor.a = Mathf.Lerp(start, end, time); //색깔을 점점 밝게함
            fadeimg.color = fadecolor;
            yield return null;
        }
        isplaying = false;
        
    }

}
