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
    GameObject QTEUnderCircleObj;
    [SerializeField]
    GameObject R_ButtonObj;
    [SerializeField]
    GameObject QTECircleObj;

    float countDown;    
    float strCountDown = 1f;
   
    void Start()
    {
        QTEOff();
    }

    public override void Activate(HardVariety Hardness)
    {
        QTECircleObj.SetActive(true);
        QTEUnderCircleObj.SetActive(true);
        R_ButtonObj.SetActive(true);
        StartCoroutine(QTESuccess());
    }

    protected override void QTEEnd()
    {
        QTEOff();
        NullActions();
    }
    
    private void QTEOff()
    {
        QTECircleObj.SetActive(false);
        QTEUnderCircleObj.SetActive(false);
        R_ButtonObj.SetActive(false);
        fillImage = 0;
        countDown = strCountDown;
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