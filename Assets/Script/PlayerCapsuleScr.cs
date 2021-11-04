using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCapsuleScr : MonoBehaviour
{
    public event Action OnCapsuleEnter;
    public event Action OnCapsuleExit;
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.tag == "NeedCapsule")
        {
            Debug.Log("Need Capsule is in!");
            if(OnCapsuleEnter != null)
            {
                OnCapsuleEnter();
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "NeedCapsule")
        {
            Debug.Log("Need Capsule is out!");
            if (OnCapsuleExit != null)
            {
                OnCapsuleExit();
            }
        }
    }
}
