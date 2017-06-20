using UnityEngine;
using System.Collections;

public class WebCam : MonoBehaviour
{
    WebCamTexture webCam = null;

    void Start()
    {
        StartCoroutine(StartWebcam());
    }

    IEnumerator StartWebcam()
    {
        webCam = new WebCamTexture(Screen.width, Screen.height);

        webCam.Play();

        while (!webCam.didUpdateThisFrame) yield return null;

        GetComponent<Renderer>().material.mainTexture = webCam;

        transform.localRotation = Quaternion.AngleAxis(webCam.videoRotationAngle, -Vector3.forward);

        //Debug.Log("WebCamTexture: " + webCam.width + ", " + webCam.height);

        var sy = 2.0f * Screen.width / Screen.height;
        var sx = sy * webCam.width / webCam.height;
        transform.localScale = new Vector3(sx, sy, 1);
    }

    //  목적지 변경하기위해 씬 변경하면 캠을 끄고, 모든 코르틴을 종료한다.
    void OnDestroy()
    {
        if (webCam != null)
        {
            webCam.Stop();
        }

        StopAllCoroutines();
    }
}
