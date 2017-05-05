using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class SetDestination : MonoBehaviour
{
    private int index;
    private DestinationDTO destinationDTO = null;
    public Text destinationText = null;

    public void OnSetDestination(GameObject obj)
    {
        string btnText = obj.name;

        switch (btnText)
        {
            case "HospitalBtn":
                index = 0;
                break;
            case "LibraryBtn":
                index = 1;
                break;
            case "Engineeringbuilding2":
                index = 2;
                break;
            default:
                break;
        }

        destinationText.text = "목적지 : " + obj.GetComponentInChildren<Text>().text;
        destinationDTO.ToDestinationData(index);
    }


    void Start()
    {
        //destinationDTO = GameObject.FindObjectOfType<DestinationDTO>();
        destinationDTO = new DestinationDTO();
    }

}
