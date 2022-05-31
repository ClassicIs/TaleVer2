using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class isQtePassed : QTEObject
{
    public float fillImage = 0;
    public float timePassed = 0;
    public bool qteSuccess = false;
    public bool qteActive = false;

    [Header("References")]
    [SerializeField]
    Image UnderCircle;
    [SerializeField]
    Image QTECircle;
    [SerializeField]
    Image RButton;

    [SerializeField]
    GameObject QTECircleObj;
    [SerializeField]
    GameObject QTEUnderCircleObj;
    [SerializeField]
    GameObject R_ButtonObj;
    
    float countDown;    
    float strCountDown = 1f;
   
    void Start()
    {
        QTEOn(false);
    }

    public override void Activate(HardVariety Hardness)
    {
        QTEOn(true);
        StartCoroutine(QTESuccess());
    }

    protected override void QTEEnd()
    {
        QTEOn(false);
        NullActions();
    }
    
    private void QTEOn(bool thisQTE)
    {
        QTECircleObj.SetActive(thisQTE);
        QTEUnderCircleObj.SetActive(thisQTE);
        R_ButtonObj.SetActive(thisQTE);        
        countDown = strCountDown;
        if (!thisQTE)
        {
            fillImage = 0;
        }
        QTECircle.fillAmount = fillImage;
    }


    public IEnumerator QTESuccess()
    {
        bool hasEnded = false;
        while (!hasEnded)
        {           

            if (Input.GetKeyDown(KeyCode.R))
            {
                fillImage += .2f;
                Debug.Log("R is pressed");
            }

            timePassed += Time.deltaTime;

            if (timePassed > .05)
            {
                timePassed = 0;
                fillImage -= .02f;                
            }

            QTECircle.fillAmount = fillImage;

            if (countDown > 0)
            {
                countDown -= .5f * Time.deltaTime;
            }


            if (fillImage < 0 && countDown <= 0)
            {
                hasEnded = true;
                Failed();
            }            

            if (fillImage >= 1)
            {
                hasEnded = true;
                Success();                           
            }
            yield return null;            
        }
        QTEEnd();
    }
}