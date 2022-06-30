using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ControlsScript : MonoBehaviour, IMenu
{
    void Start()
    {
        MenuOn(false);
    }
    public void MenuOn(bool on)
    {
        gameObject.SetActive(on);
    }
}
