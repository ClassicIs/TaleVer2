using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

public class isQtePassed : QTEObject
{
    public float fillImage = 0;
    public float timePassed = 0;
    public bool qteSuccess = false;
    public bool qteActive = false;

    private KeyCode[] possibleKeys;
    private KeyCode curKey;

    [Header("References")]
    [SerializeField]
    GameObject QTECircleObj;
    [SerializeField]
    GameObject QTEUnderCircleObj;
    [SerializeField]
    GameObject R_ButtonObj;
    [SerializeField]
    TextMeshProUGUI textForKeyValue;

    Image QTECircle;
    Image UnderCircle;
    Image RButton;

    float countDown;    
    float strCountDown = 1f;

    private void Awake()
    {
        QTECircle = QTECircleObj.GetComponent<Image>();
        UnderCircle = QTEUnderCircleObj.GetComponent<Image>();
        RButton = R_ButtonObj.GetComponent<Image>();
        
    }

    void Start()
    {
        possibleKeys = new KeyCode[]
        {
            KeyCode.R,
            KeyCode.U,
            KeyCode.P,
            KeyCode.O
        };

        QTEOn(false);
    }

    public override void Activate(HardVariety Hardness)
    {
        QTEOn(true);

        float giveForPress = 0.2f;
        float getForTime = 0.35f;
        float timeForKeyChange = 1.0f;

        switch(Hardness)
        {
            case HardVariety.easy:
                giveForPress = 0.15f;
                getForTime = 0.01f;
                timeForKeyChange = 2f;
                break;
            case HardVariety.normal:
                giveForPress = 0.1f;
                getForTime = 0.02f;
                timeForKeyChange = 1.5f;
                break;
            case HardVariety.hard:
                giveForPress = 0.08f;
                getForTime = 0.023f;
                timeForKeyChange = 1f;
                break;
            default:
                giveForPress = 0.2f;
                getForTime = 0.08f;
                timeForKeyChange = 1f;
                break;
        }
        StartCoroutine(QTESuccess(getForTime, giveForPress, timeForKeyChange));
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
        textForKeyValue.gameObject.SetActive(thisQTE);
        countDown = strCountDown;
        if (!thisQTE)
        {
            fillImage = 0;
        }
        QTECircle.fillAmount = fillImage;
    }

    private IEnumerator ChangeKey(float timeToChange)
    {
        float thisTimeToChange = timeToChange;
        while (true)
        {
            int curIndex = UnityEngine.Random.Range(0, possibleKeys.Length);

            curKey = possibleKeys[curIndex];

            textForKeyValue.text = curKey.ToString();

            Debug.LogFormat("Current key is {0}", curKey.ToString());
            yield return new WaitForSeconds(thisTimeToChange);
        }
    }
    public IEnumerator QTESuccess(float takeAway, float give, float timeToChange)
    {
        IEnumerator changeKeyCoroutine;
        changeKeyCoroutine = ChangeKey(timeToChange);
        StartCoroutine(changeKeyCoroutine);
        
        float takeAwayFromCircle = takeAway;
        float giveToCircle = give;

        while (true)
        {
            if (Input.GetKeyDown(curKey))
            {
                fillImage += giveToCircle;
                //Debug.LogFormat("{0} is pressed.\n{1} is given to circle.", curKey.ToString(), giveToCircle);
            }

            timePassed += Time.deltaTime;

            if (timePassed > .1f)
            {
                timePassed = 0;
                if(fillImage > 0)
                    fillImage -= takeAwayFromCircle;
                //Debug.LogFormat("This amount was taken away from circle: {0}", takeAwayFromCircle);

            }
            
            if (fillImage >= 0)
            {
                QTECircle.fillAmount = fillImage;
            }
            else
            {
                Debug.LogFormat("Fill image is less then 0. It's {0}", fillImage);
            }

            if (countDown > 0)
            {
                countDown -= .5f * Time.deltaTime;
            }


            if (fillImage < 0 && countDown <= 0)
            {
                Failed();
                break;
            }            

            if (fillImage >= 1)
            {
                Success();
                break;
            }
            yield return null;            
        }
        StopCoroutine(changeKeyCoroutine);
        QTEEnd();
    }
}