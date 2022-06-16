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
        int needCount;
        int maxTries;
        switch (Hardness)
        {
            case HardVariety.easy:
                needCount = 3;
                maxTries = 6;
                break;
            case HardVariety.normal:
                needCount = 4;
                maxTries = 8;
                break;
            case HardVariety.hard:
                needCount = 5;
                maxTries = 9;
                break;
            default:
                needCount = 2;
                maxTries = 4;
                break;
        }
        maxCount = needCount;
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

        Debug.LogFormat("Start of slider is {0}\nEnd of slider is {1} Random number is {2}", startXPos, endXPos, randomNumber);
        int numberOfParts = 25;
        int maxSize = 3;
        int tail = 200;
        for (int i = 0; i < numberOfParts; i++)
        {
            int x = Mathf.RoundToInt(startXPos + (endXPos / numberOfParts) * i);
            GameObject thePart = Instantiate(theSliderPart, new Vector3(x, yPos, 0), Quaternion.identity, QTESliderHolder.GetComponent<RectTransform>());
            thePart.name = "Part " + i.ToString();
            
            float close = Mathf.Pow(Mathf.Clamp01(1f - Mathf.Abs(thePart.GetComponent<RectTransform>().position.x - randomNumber) / range), 4);
            Debug.LogFormat("Close = {0}. \nRange is {1}.\nX position is {2}\nPart {3}", close, range, thePart.GetComponent<RectTransform>().position.x, i);
            
            thePart.GetComponent<Image>().color = new Color(close, 1 - close, Mathf.Clamp01(0.2f - close));
            thePart.GetComponent<RectTransform>().localScale += new Vector3(0f, maxSize * close, 0f);
        }
        
        StartCoroutine(theNum(tail, needCount, maxTries, startXPos, endXPos, yPos, range));
    }

    IEnumerator theNum(int tail, int needNumber, int maxTries, float startPos, float endPos, float yPos, float range)
    {
        int currTries = maxTries;
        float speed = 3f;
        int dir = 1;
        int score = 0;
        int needScore = 300;
        int cost = 50;

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
                score += Mathf.RoundToInt(1 - (raznitsa / range)) * cost;
                
                Debug.LogFormat("Raznitsa: {0}\nRaznitsa in range: {1}\nRange: {2}\nScore: {3}", raznitsa, raznitsa / range, range, score);
                if(score == needScore)
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
