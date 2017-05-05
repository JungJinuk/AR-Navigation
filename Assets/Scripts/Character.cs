using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    private float destinationLatitude = 0;
    private float destinationLongitude = 0;

    private Vector3 myPosition;
    private Vector3 direction;
    private Vector3 destination;

    private float trueHeading;
    private float phoneAngle = 0;
    private float destinationAngle = 0;
    private float headDirection = 0;

    Animator ani;

    public void SetDestination(float latitude, float longitude)
    {
        destinationLatitude = latitude;
        destinationLongitude = longitude;
    }

    //  From GPSSystem
    public void UpdateMyPosition(float latitude, float longitude)
    {
        myPosition.Set(longitude * 100, 0, latitude * 100);
    }

    IEnumerator Start()
    {
        Input.location.Start();
        Input.compass.enabled = true;

        ani = GetComponent<Animator>();

        while (destinationLatitude == 0)
        {
            yield return new WaitForSeconds(1);
        }

        ani.SetTrigger("Walk");
        StartCoroutine(NaviUpdate());
    }

    IEnumerator NaviUpdate()
    {
        while (true)
        {
            phoneAngle = Mathf.Floor(trueHeading/ 10) * 10;  //  make rotation angle multiples of 10
            destination.Set(destinationLongitude * 100, 0, destinationLatitude * 100);

            MakeLookDestination();

            transform.rotation = Quaternion.Euler(0, headDirection, 0);

            yield return new WaitForSeconds(1);
        }
    }

    //  Function of making character look destination
    void MakeLookDestination()
    {
        direction = destination - myPosition;
        direction.Normalize();

        destinationAngle = Vector3.Angle(Vector3.forward, direction);
        destinationAngle = Vector3.Dot(Vector3.right, direction) > 0.0 ? destinationAngle : -destinationAngle;
        destinationAngle = Mathf.Floor(destinationAngle / 10) * 10;

        headDirection = destinationAngle - phoneAngle;
    }

	void Update ()
    {
        trueHeading = Input.compass.trueHeading;
    }
}
