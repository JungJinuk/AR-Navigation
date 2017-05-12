using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class NavigationManager : MonoBehaviour
{
    string destinationText = string.Empty;
    public Text destinationGUI = null;

    Navigation navigation = null;

    void Awake()
    {
        navigation = GameObject.FindObjectOfType<Navigation>();
        DestinationInit();
    }

    void Start()
    {
        destinationGUI.text = "목적지 : " + destinationText;
    }

    //  목적지에 대한 정보가 담겨있다.
    //  목적지를 선택했을 때 그에 맞는 목적지 위치좌표로 갱신
    void DestinationInit()
    {
        float latitude = 0f;
        float longitude = 0f;

        switch (DestinationManager.destination)
        {
            case "Hospital":
                {
                    destinationText = "한양대학교 병원";
                    latitude = 37.559597f;
                    longitude = 127.043733f;
                }
                break;
            case "Library":
                {
                    destinationText = "백남학술정보관";
                    latitude = 37.557355f;
                    longitude = 127.045687f;
                }
                break;
            case "Engineering2":
                {
                    destinationText = "제2공학관";
                    latitude = 37.555729f;
                    longitude = 127.046117f;
                }
                break;
            default:
                {
                    // 목적지가 잘못되었습니다.
                    // 목적지를 다시 선택해 주십시오.
                    // 목적지 선택씬으로 전환
                    BackToSetDestinationScene();
                }
                break;
        }

        //  내비게이션에 목적지 좌표를 전달한다.
        navigation.SetDestination(latitude, longitude);
    }

    //  목적지 변경 버튼 클릭시 목적지 선택 씬으로 전환
    public void BackToSetDestinationScene()
    {
        SceneManager.LoadScene("SetDestination");
    }
}
