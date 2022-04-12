using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class PlayerOtherInput : MonoBehaviour
{
    public event Action OnInteracting;
    
    public event Action OnEscape;   

    void Update()
    {
        CheckForInput();
    }

    public void ClearAllInter()
    {
        OnInteracting = null;
    }

    void CheckForInput()
    {
        if (Input.GetKeyDown(KeyCode.F))
        {
            Debug.Log("F is Pressed!");
            if (OnInteracting != null)
            {
                OnInteracting();
            }
        }

        if(Input.GetKeyDown(KeyCode.Escape))
        {
            if (OnEscape != null)
            {
                OnEscape();
            }
        }
    }
}
