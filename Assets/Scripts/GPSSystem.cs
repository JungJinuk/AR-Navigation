using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GPSSystem : MonoBehaviour
{
    /*
     * GPS로 현재 기기 위치와 googlemap 갱신 
     */
    
    public Text latitudeText = null;
    public Text longitudeText = null;
    public GoogleMap map = null;

    float latitude = 0f;
    float longitude = 0f;

    Navigation navigation = null;

    IEnumerator Start()
    {
        //사용자 위치 서비스가 실행되고 있는지 먼저 점검한다.
        if (!Input.location.isEnabledByUser)
            yield break;

        // 위치를 조회하기 전에 서비스를 시작한다.
        Input.location.Start(0.5f);

        // 서비스 시작하기 전까지 대기 한다.
        int maxWait = 20;

        while (Input.location.status == LocationServiceStatus.Initializing && maxWait > 0)
        {
            yield return new WaitForSeconds(1);
            maxWait--;
        }

        // 20초 안에 서비스가 초기화하지 않았다.
        if (maxWait < 1)
        {
            print("Timed out");
            yield break;
        }

        // 연결 실패
        if (Input.location.status == LocationServiceStatus.Failed)
        {
            print("Unable to determine device location");
            yield break;
        }
        else
        {
            navigation = GameObject.FindObjectOfType<Navigation>();

            //  1초마다 자신의 위치를 갱신해서 구글맵 수정
            while(true)
            {
                latitude = Input.location.lastData.latitude;
                longitude = Input.location.lastData.longitude;

                latitudeText.text = "위도 : " + latitude.ToString();
                longitudeText.text = "경도 : " + longitude.ToString();

                //  현재 위치를 맵 중앙에 위치하게끔 한다.
                map.centerLocation.latitude = latitude;
                map.centerLocation.longitude = longitude;

                //  네비게이션에 업데이트된 현재위치좌표를 전달한다.
                navigation.UpdateMyPosition(latitude, longitude);

                yield return new WaitForSeconds(1);
            }
        }

        // Stop service if there is no need to query location updates continuously
       // Input.location.Stop();
    }

    //  목적지 변경하기위해 씬 변경하면 모든 코르틴을 종료한다.
    void OnDestroy()
    {
        Input.location.Stop();
        StopAllCoroutines();
    }
}