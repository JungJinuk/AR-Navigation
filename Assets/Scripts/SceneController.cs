using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class SceneController : MonoBehaviour
{
    public void ChangeToSetDestinationScene()
    {
        SceneManager.LoadScene("SetDestination");
    }

    public void ChangeToARnavigationScene()
    {
        SceneManager.LoadScene("ARnavigation");
    }
}
