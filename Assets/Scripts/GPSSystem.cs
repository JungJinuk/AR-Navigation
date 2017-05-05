using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class GPSSystem : MonoBehaviour
{
    public Text latitudeText = null;
    public Text longitudeText = null;
    public GoogleMap map = null;

    IEnumerator Start()
    {
        //사용자 위치 서비스가 실행되고 있는지 먼저 점검한다.
        if (!Input.location.isEnabledByUser)
            yield break;

        // 위치를 조회하기 전에 서비스를 시작합니다.
        Input.location.Start(0.5f);

        // 서비스 시작하기 전까지 대기 합니다.
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
            Character character = GameObject.FindObjectOfType<Character>();

            //  1초마다 자신의 위치를 갱신해서 구글맵 수정
            while(true)
            {
                latitudeText.text = Input.location.lastData.latitude.ToString();
                longitudeText.text = Input.location.lastData.longitude.ToString();

                map.centerLocation.latitude = Input.location.lastData.latitude;
                map.centerLocation.longitude = Input.location.lastData.longitude;

                character.UpdateMyPosition(Input.location.lastData.latitude, Input.location.lastData.longitude);
                
                //print(Input.location.status.ToString());
                //Access granted and location value could be retrieved
                //print("Location: " + Input.location.lastData.latitude + " " + Input.location.lastData.longitude + " " + Input.location.lastData.altitude + " " + Input.location.lastData.horizontalAccuracy + " " + Input.location.lastData.timestamp);
                yield return new WaitForSeconds(1);
            }

           
        }

        // Stop service if there is no need to query location updates continuously
       // Input.location.Stop();
    }
}