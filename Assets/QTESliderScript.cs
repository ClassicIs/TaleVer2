using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTESliderScript : QTEObject
{
    [SerializeField]
    private GameObject QTESliderHolder;
    [SerializeField]
    Slider theSlider;
    bool hasEnded;
    [SerializeField]
    private float speedOfSlide;
    [SerializeField]
    private int winCount;
    [SerializeField]
    private int allCount;
    private int maxCount;
    private int maxTries;
    private float limForSucc = 0.4f;
    [SerializeField]
    float valOfSlide;

    // Start is called before the first frame update
    void Start()
    {
        theSlider = GetComponentInChildren<Slider>();
        theSlider.interactable = false;
    }

    public void Activate(int needCount = 3)
    {
        Debug.Log("We are activating");
        QTESliderHolder.SetActive(true);
        valOfSlide = 0f;
        maxCount = needCount;
        winCount = 0;
        speedOfSlide = 0.2f;
        maxTries = 100;
        allCount = maxTries;
        hasEnded = false;
        StartCoroutine(theNum());
    }

    IEnumerator theNum()
    {        
        bool atStart = true;
        while (!hasEnded)
        {            
            if (valOfSlide <= 0.0f)
            {
                atStart = true;
            }
            else if(valOfSlide >= 1.0f)
            {
                allCount -= 1;
                atStart = false;
            }

            if (atStart)
            {
                valOfSlide += speedOfSlide * Time.deltaTime;
            }
            else
            {
                valOfSlide -= speedOfSlide * Time.deltaTime;
            }
            theSlider.value = valOfSlide;

            if (Input.GetKeyDown(KeyCode.G))
            {
                float theValue;
                if (valOfSlide > 0.5f)
                {
                    theValue = 0.5f - (valOfSlide - 0.5f);
                }
                else
                {
                    theValue = valOfSlide;
                }
                
                if (theValue > limForSucc)
                {
                    winCount++;
                    Debug.Log("You're in the correct range! The value is " + theValue);
                }
                
                if (winCount == maxCount)
                {
                    hasEnded = true;
                    Success();
                }                               
            }
            if (allCount <= 0)
            {
                hasEnded = true;
                Failed();
            }

            yield return null;
        }
        Close();

    }

    private void Close()
    {
        QTESliderHolder.SetActive(false);
    }

    
}
