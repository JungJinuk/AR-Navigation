using UnityEngine;
using System.Collections;

public class Character : MonoBehaviour
{
    Animator ani = null;

    //  캐릭터가 뛰어가는 애니매이션으로
    void Start()
    {
        ani = GetComponent<Animator>();
        ani.SetTrigger("FollowMe");
    }
}
