using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLDoor : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
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
