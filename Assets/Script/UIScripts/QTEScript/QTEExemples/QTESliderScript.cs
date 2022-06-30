using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class QTESliderScript : QTEObject
{
    [SerializeField]
    private GameObject QTESliderHolder;
    [SerializeField]
    GameObject theSliderPart;
    bool hasEnded;
    [SerializeField]
    private float speedOfSlide;
    [SerializeField]
    private int winCount;
    [SerializeField]
    private int allCount;
    private int maxCount;
    
    private float limForSucc = 0.4f;
    [SerializeField]
    float valOfSlide;
    [SerializeField]
    RectTransform startOfSlider;
    [SerializeField]
    RectTransform endOfSlider;

    public override void Activate(HardVariety Hardness)
    {
        QTESliderHolder.SetActive(true);
        int needScore;
        int maxTries;
        float speed;
        switch (Hardness)
        {
            case HardVariety.easy:
                needScore = 300;
                maxTries = 10;
                speed = 8f;
                break;
            case HardVariety.normal:
                needScore = 300;
                maxTries = 8;
                speed = 9f;
                break;
            case HardVariety.hard:
                needScore = 300;
                maxTries = 6;
                speed = 10f;
                break;
            default:
                needScore = 300;
                maxTries = 8;
                speed = 8f;
                break;
        }
        maxCount = needScore;
        winCount = 0;
        speedOfSlide = 0.2f;
        
        allCount = maxTries;
        //hasEnded = false;
        float startXPos = startOfSlider.position.x;
        float endXPos = endOfSlider.position.x;
        float yPos = startOfSlider.position.y;
    
        float range = Mathf.Abs(endXPos) - Mathf.Abs(startXPos);
        int randomNumber = Mathf.RoundToInt(Random.Range(startXPos, endXPos));
        //Instantiate(theSliderPart, startOfSlider.position, Quaternion.identity, QTESliderHolder.GetComponent<RectTransform>());
        //Instantiate(theSliderPart, endOfSlider.position, Quaternion.identity, QTESliderHolder.GetComponent<RectTransform>());

        //Debug.LogFormat("Start of slider is {0}\nEnd of slider is {1} Random number is {2}", startXPos, endXPos, randomNumber);
        int numberOfParts = 25;
        int maxSize = 3;
        int tail = 200;
        for (int i = 0; i <= numberOfParts; i++)
        {
            float x = startXPos + ((endXPos - startXPos) / numberOfParts) * i;
            
            Debug.LogFormat("X {0}: {1}", i, x);
            GameObject thePart = Instantiate(theSliderPart, new Vector3(x, yPos, 0), Quaternion.identity, QTESliderHolder.GetComponent<RectTransform>());
            thePart.name = "Part " + i.ToString();
            
            float close = Mathf.Pow(Mathf.Clamp01(1f - Mathf.Abs(thePart.GetComponent<RectTransform>().position.x - randomNumber) / range), 4);
            //Debug.LogFormat("Close = {0}. \nRange is {1}.\nX position is {2}\nPart {3}", close, range, thePart.GetComponent<RectTransform>().position.x, i);
            
            thePart.GetComponent<Image>().color = new Color(close, 1 - close, Mathf.Clamp01(0.2f - close));
            thePart.GetComponent<RectTransform>().localScale += new Vector3(0f, maxSize * close, 0f);
        }
        
        StartCoroutine(theNum(tail, randomNumber, needScore, speed, maxTries, startXPos, endXPos, yPos, range));
    }

    IEnumerator theNum(int tail, int needNumber, int theNeedScore, float theSpeed, int maxTries, float startPos, float endPos, float yPos, float range)
    {
        int currTries = maxTries;
        float speed = theSpeed;
        int dir = 1;
        int score = 0;
        int needScore = theNeedScore;
        int cost = 83;

        GameObject part = Instantiate(theSliderPart, new Vector3(startPos, yPos, 0), Quaternion.identity, QTESliderHolder.GetComponent<RectTransform>());
        part.GetComponent<Image>().color = new Color(0, 0, 1);
        part.name = "playerArrow";
        RectTransform arrowTransform = part.GetComponent<RectTransform>();
        arrowTransform.localScale = new Vector3(1f, 5f, 0f);       

        while (true)
        {
            if (arrowTransform.position.x >= endPos)
            {
                dir = -1;
            }
            else if(arrowTransform.position.x <= startPos)
            {
                dir = 1;
            }

            arrowTransform.position += new Vector3(speed, 0f, 0f) * dir;

            if (Input.GetKeyDown(KeyCode.F))
            {
                currTries -= 1;
                float raznitsa = Mathf.Abs(arrowTransform.position.x - needNumber);
                //Debug.LogFormat("arrowTransform.position.x: {0}\nneedNumber: {1}", arrowTransform.position.x, needNumber);
                float curScore = (1f - (raznitsa / range));               
                
                if (curScore > 0.8f)
                {
                    score += Mathf.RoundToInt(curScore * cost);
                    //Debug.LogError("You're in range!");
                    
                }
                //Debug.LogFormat("Raznitsa: {0}\nRaznitsa in range: {1}\nCurrent score: {2}\nWhole score {3}", raznitsa, raznitsa / range, curScore, score);

                if(score >= needScore)
                {
                    Success();
                    break;
                }
            }            
            if (currTries <= 0)
            {                
                Failed();
                break;
            }
            yield return null;
        }
        QTEEnd();
    }

    protected override void QTEEnd()
    {
        QTESliderHolder.SetActive(false);
    }

    
}
