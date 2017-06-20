using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class DestinationManager : MonoBehaviour
{
    //  씬 전환시 목적지 정보 전달
    public static string destination = string.Empty;

    public void OnSetDestination(GameObject btn)
    {
        destination = btn.name;

        //  AR 내비게이션 씬으로 전환
        SceneManager.LoadScene("ARnavigation");
    }
}
