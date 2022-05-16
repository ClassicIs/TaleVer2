﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MenuScript : MonoBehaviour, MenuObject
{
    bool isMenuOn;

    [SerializeField]
    GameObject Panel;
    [SerializeField]
    GameObject Menu;

    void Start()
    {
        isMenuOn = false;
    }
    
    public void MenuOn()
    {
        Image PanelRenderer = Panel.GetComponent<Image>();
        isMenuOn = !isMenuOn;
        if (isMenuOn)
        {
            
            PanelRenderer.color = new Color(1, 1, 1, 0);
            Panel.SetActive(isMenuOn);
            StartCoroutine(TurnOn(PanelRenderer, isMenuOn, 0.75f));
            Menu.SetActive(isMenuOn);
        }
        else
        {            
            StartCoroutine(TurnOn(PanelRenderer, isMenuOn, 0.75f));            
        }
    }

    private void SetActiveObjects()
    {
        Panel.SetActive(isMenuOn);
        Menu.SetActive(isMenuOn);
    }

    IEnumerator TurnOn(Image TheRenderer, bool On, float maxValue)
    {
        int turn;
        float endPoint;

        if(On)
        {
            Debug.Log("Turning menu On");
            endPoint = maxValue;
            turn = 1;
        }
        else
        {
            Debug.Log("Turning menu Off");
            endPoint = 0;
            turn = -1;
        }
        Debug.Log("Current alpha is " + TheRenderer.color.a + "\nEnd point is " + endPoint);
        if (On)
        {
            while (TheRenderer.color.a <= endPoint)
            {
                Debug.Log("Turning menu On " + TheRenderer.color.a);
                TheRenderer.color += new Color(1, 1, 1, 0.01f * turn);
                yield return null;
            }
        }
        else
        {
            while (TheRenderer.color.a >= endPoint)
            {
                Debug.Log("Turning menu Off " + TheRenderer.color.a);
                TheRenderer.color += new Color(1, 1, 1, 0.01f * turn);
                yield return null;
            }
        }

        TheRenderer.color = new Color(1, 1, 1, endPoint);
        SetActiveObjects();
    }
}
