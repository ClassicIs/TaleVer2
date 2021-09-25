using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ButtonManager : MonoBehaviour
{

    public void CloseTheGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}
