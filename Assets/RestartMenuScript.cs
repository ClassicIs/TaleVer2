using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RestartMenuScript : MonoBehaviour, MenuObject
{
    private bool isActive;

    private void Start()
    {
        isActive = false;
        gameObject.SetActive(isActive);
    }

    public void MenuOn()
    {
        isActive = !isActive;

        Debug.LogFormat("Menu is active: {0}.", isActive);

        gameObject.SetActive(isActive);
        
        if (isActive)
        {         
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }
        
    }
}
