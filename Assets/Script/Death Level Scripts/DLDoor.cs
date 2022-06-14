using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLDoor : MonoBehaviour
{
    [SerializeField]
    private bool isOpened = true;

    private void Start()
    {
        if(isOpened)
        {
            OpenDoor();
        }
        else
        {
            CloseDoor();
        }

    }

    public void OpenDoor()
    {
        gameObject.GetComponent<Animator>().SetBool("IsOpened", true);
    }

    public void CloseDoor()
    {
        gameObject.GetComponent<Animator>().SetBool("IsOpened", false);
    }
}
