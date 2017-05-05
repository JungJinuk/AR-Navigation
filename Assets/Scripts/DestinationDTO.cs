using UnityEngine;
using System.Collections;

public class DestinationDTO : MonoBehaviour
{
    private float[] latitude = new float[3];
    private float[] longitude = new float[3];
    private int index;

    Character character;

    public void ToDestinationData(int index)
    {
        DestinationCoordination();
        character = GameObject.FindObjectOfType<Character>();
        this.index = index;

        character.SetDestination(latitude[index], longitude[index]);
    }

    void DestinationCoordination()
    {
        //  Hospital of HanyangUniv
        latitude[0] = 37.559597f;
        longitude[0] = 127.043733f;

        //  Library of HanyangUniv
        latitude[1] = 37.557355f;
        longitude[1] = 127.045687f;

        //  engineeringcenter2 of HanyangUniv
        latitude[2] = 37.555729f;
        longitude[2] = 127.046117f;
    }

}
