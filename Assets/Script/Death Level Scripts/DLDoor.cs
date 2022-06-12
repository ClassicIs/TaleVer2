using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLDoor : MonoBehaviour
{

  

    public void OpenDoor()
    {
        gameObject.GetComponent<Animator>().SetBool("IsOpened", true);
    }

    public void CloseDoor()
    {
        gameObject.GetComponent<Animator>().SetBool("IsOpened", false);
    }
}
