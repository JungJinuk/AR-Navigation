using UnityEngine;
using System.Collections;

public class Navigation : MonoBehaviour
{
    //  나침반 위치
    float trueHeading = 0f;

    //  현재위치와 목적지위치와 방향벡터
    Vector3 myPos = Vector3.zero;
    Vector3 direction = Vector3.zero;
    Vector3 destinationPos = Vector3.zero;

    //  나침반 바늘
    public RectTransform compassArrow = null;

    //  안내 화살표
    public RectTransform greenArrow = null;

    //  현재위치로부터 목적지까지의 회전각
    float destinationAngle = 0f;

    //  현재위치 좌표, 목적지 좌표
    float mlatitude = 0f;
    float mlongitude = 0f;
    float dlatitude = 0f;
    float dlongitude = 0f;

    //  목적지까지 남은 거리, GUI
    float distance = 0f;
    public UnityEngine.UI.Text distanceText = null;

    //  회전축
    GameObject cameraAxis = null;
    GameObject characterAxis = null;
    
    void Awake()
    {
        //  나침반기능 활성화
        Input.location.Start();
        Input.compass.enabled = true;

        cameraAxis = GameObject.FindGameObjectWithTag("CameraAxis");
        characterAxis = GameObject.FindGameObjectWithTag("CharacterAxis");
    }

    void Start()
    {
        StartCoroutine(CameraAxisRotate());
    }

    
    void Update()
    {
        //  GPS 기능을 켜지 않았거나, 현재위치 오류가 났을 경우
        if (!Input.location.isEnabledByUser || Input.location.status == LocationServiceStatus.Failed)
        {
            return;
        }

        //  현재 기기의 북쪽을 기준으로 한 회전각( 0 ~ 360 )
        trueHeading = Input.compass.trueHeading;
    }
    

    //  From NavigationManager : 목적지 변경시 다시 호출
    public void SetDestination(float latitude, float longitude)
    {
        //  목적지 위치좌표 갱신
        dlatitude = latitude;
        dlongitude = longitude;
        destinationPos.Set(dlongitude * 100, 0f, dlatitude * 100);

        calculateDirection();
    }

    //  From GPSSystem : 1초마다 호출됨
    public void UpdateMyPosition(float latitude, float longitude)
    {
        //  현재 위치좌표 갱신
        mlatitude = latitude;
        mlongitude = longitude;
        myPos.Set(mlongitude * 100, 0f, mlatitude * 100);

        calculateDirection();
    }

    //  현재위치로부터 목적지까지의 방향과 회전각 계산
    void calculateDirection()
    {
        //  목적지를 향한 방향벡터
        direction = destinationPos - myPos;
        direction.Normalize();

        //  목적지를 향한 방향벡터를 회전각으로 변환
        destinationAngle = Vector3.Angle(Vector3.forward, direction);
        destinationAngle = Vector3.Dot(Vector3.right, direction) > 0.0 ? destinationAngle : -destinationAngle;

        //  안내 캐릭터의 회전각
        characterAxis.transform.rotation = Quaternion.Euler(0f, destinationAngle, 0f);

        //  목적지까지의 남은 거리를 m 단위로 계산
        distance = (float)(DistanceManager.Distance(mlatitude, mlongitude, dlatitude, dlongitude, 'K') * 1000);

        //  20000m (20km) 보다 더 멀리 있으면
        if (distance >= 20000)
        {
            distanceText.text = "걸어가기엔 너무 멀어요!";
        }
        else if (distance <= 3)
        {
            //  도착
            distanceText.text = "근처에 도착하였습니다!";
        }
        else
        {
            distanceText.text = "남은 거리 : " + Mathf.Floor(distance) + " m";
        }
    }


    //  핸드폰 축의 회전
    IEnumerator CameraAxisRotate()
    {
        Quaternion currentHeading;
        Quaternion compassArrowAngle;
        Quaternion greenArrowAngle;

        while (true)
        {
            //  카메라의 방향과 회전각 + 부드럽게
            currentHeading = Quaternion.Euler(0f, trueHeading, 0f);

            cameraAxis.transform.rotation = Quaternion.Slerp(cameraAxis.transform.rotation,
                currentHeading, Time.deltaTime * 2f);

            //  나침반 바늘의 회전각 + 부드럽게
            compassArrowAngle = Quaternion.Euler(0f, 0f, trueHeading);

            compassArrow.rotation = Quaternion.Slerp(compassArrow.rotation,
                compassArrowAngle, Time.deltaTime * 2f);

            //  안내 화살표의 방향과 회전각 + 부드럽게
            greenArrowAngle = Quaternion.Euler(0f, 0f, -destinationAngle + trueHeading);

            greenArrow.rotation = Quaternion.Slerp(greenArrow.rotation,
                greenArrowAngle, Time.deltaTime * 2f);

            yield return null;
        }
    }

    /*
    //  핸드폰의 움직임이 들어간 회전각을 부드럽게 움직이도록 보정하는 함수
    Quaternion makeRotateSmooth(Quaternion rotation, Quaternion angle)
    {
        return Quaternion.Slerp(rotation, angle, Time.deltaTime * 2f);
    }
    */

    //  씬 전환시 모든 코르틴 종료
    void OnDestroy()
    {
        StopAllCoroutines();
    }
}
