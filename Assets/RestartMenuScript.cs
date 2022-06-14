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
        if (!isActive)
        {
            isActive = true;            
            Time.timeScale = 0f;            
            gameObject.SetActive(isActive);
        }
    }
}
