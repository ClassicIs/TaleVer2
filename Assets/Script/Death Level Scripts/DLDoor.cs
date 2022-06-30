using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLDoor : MonoBehaviour
{
    [SerializeField]
    private bool isOpened = true;
    private Collider2D[] Colliders;
    private void Start()
    {
        Colliders = GetComponents<Collider2D>();
        if (isOpened)
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

    private void ColliderOn(bool on)
    {
        foreach (Collider2D col in Colliders)
        {
            col.enabled = on;
        }
    }

    public void CloseDoor()
    {
        
        gameObject.GetComponent<Animator>().SetBool("IsOpened", false);
    }
}
