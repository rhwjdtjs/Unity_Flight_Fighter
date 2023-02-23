using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Photon.Pun;
public class SceneManage : MonoBehaviourPunCallbacks
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
        therigid.AddForce(Vector3.left * 50);
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
        moving();
    }
    IEnumerator loadsceneco()
    {
        yield return new WaitForSeconds(5f);
        OutStartFadeAnim();
        

    }
    public void OutStartFadeAnim()
    {
        if (isplaying == true)
            return;
        end = 1f;
        start = 0f;
        StartCoroutine("fadeoutplay");
    }
    public void InStartFadeAnim()
    {
        if (isplaying)
            return;
        start = 1f;
        end = 0f;
        StartCoroutine("fadeInanim");
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
            fadecolor.a = Mathf.Lerp(start, end, time);
            fadeimg.color = fadecolor;
            yield return null;
        }
        isplaying = false;
        PhotonNetwork.LoadLevel("Gamescene");

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
            fadecolor.a = Mathf.Lerp(start, end, time);
            fadeimg.color = fadecolor;
            yield return null;
        }
        isplaying = false;
        
    }

}
