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

    //  현재위치로부터 목적지까지의 회전각
    float destinationAngle = 0f;

    //  회전축
    GameObject cameraAxis = null;
    GameObject characterAxis = null;

    void Start()
    {
        //  나침반기능 활성화
        Input.location.Start();
        Input.compass.enabled = true;

        cameraAxis = GameObject.FindGameObjectWithTag("CameraAxis");
        characterAxis = GameObject.FindGameObjectWithTag("CharacterAxis");

        StartCoroutine(CameraAxisRotate());
        StartCoroutine(CharacterAxisRotate());
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
        destinationPos.Set(longitude * 100, 0, latitude * 100);

        calculateDirection();
    }

    //  From GPSSystem : 1초마다 호출됨
    public void UpdateMyPosition(float latitude, float longitude)
    {
        myPos.Set(longitude * 100, 0, latitude * 100);

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
    }

    //  핸드폰 축의 회전
    IEnumerator CameraAxisRotate()
    {
        Quaternion currentHeading;

        while (true)
        {
            //  나침반을 부드럽게 작동
            currentHeading = Quaternion.Euler(0f, trueHeading, 0f);
            cameraAxis.transform.rotation = Quaternion.Slerp(cameraAxis.transform.rotation,
                currentHeading, Time.deltaTime * 2f);
            yield return null;
        }
    }

    //  안내 캐릭터, 화살표 축의 회전
    IEnumerator CharacterAxisRotate()
    {
        while (true)
        {
            characterAxis.transform.rotation = Quaternion.Euler(0f, destinationAngle, 0f);
            yield return null;
        }
    }

    //  씬 전환시 모든 코르틴 종료
    void OnDestroy()
    {
        StopAllCoroutines();
    }
}
