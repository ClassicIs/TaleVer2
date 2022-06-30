using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

using System.Text.RegularExpressions;

public class CypherScript : QTEObject
{
    [SerializeField]
    GameObject [] theButton = new GameObject[9];
    [SerializeField]
    GameObject strButton;
    [SerializeField]
    GameObject QTEHolder;
    [SerializeField]
    Transform QTEButtonHolder;

    string cypherComb;
    string emptyString;
    string yourComb;
    int curIndex;
    int curStringIndex;

    [SerializeField]
    TextMeshProUGUI theOutText;
    bool buttonsActive;


    public override void Activate(HardVariety Hardness)
    {
        Debug.LogWarning("You cannot use Cyphers like this. You need to write a Cypher first!");
    }

    // Start is called before the first frame update
    public override void Activate(string cypherComb)
    {
        if(cypherComb == "")
        {
            Debug.LogWarning("You need to write a Cypher first!");
            return;
        }
        curIndex = 0;
        this.cypherComb = cypherComb;
        QTEHolder.SetActive(true);
        ButtonInstantiate();

        emptyString = "";
        for (int i = 0; i < cypherComb.Length; i++)
        {
            emptyString += "_";
        }

        yourComb = emptyString;

        OutputText(yourComb);
        buttonsActive = true;
    }

    void ButtonInstantiate()
    {        
        for (int i = 0; i < theButton.Length; i++)
        {
            int count = i + 1;            
            theButton[i] = Instantiate(strButton, QTEButtonHolder);
            theButton[i].GetComponent<Button>().onClick.AddListener(() => OnButtonClick(count));
            theButton[i].GetComponentInChildren<Text>().text = count.ToString();
        }
    }

    void ButtonOn(bool onOrOff)
    {
        for (int i = 0; i < 0; i++)
        {
            theButton[i].GetComponent<Button>().interactable = onOrOff;
        }
        buttonsActive = onOrOff;
    }

    void OnButtonClick(int numOfButton)
    {
        if (curIndex >= cypherComb.Length)
        {
            return;
        }

        string tmpComb = "";

        for(int i = 0; i < yourComb.Length; i++)
        {
            if (i == curStringIndex)
            {
                tmpComb += "<u>" + numOfButton + "</u>";
                curStringIndex = tmpComb.Length;
            }
            else
            {
                tmpComb += yourComb[i];
            }
            //Debug.LogFormat("{0} iteration. Current tmpComb is {1}", i, tmpComb);
        }

        yourComb = tmpComb;
        OutputText(yourComb);
        curIndex ++;
        if(curIndex >= cypherComb.Length)
        {
            ButtonOn(false);
        }
        
    }
    
    public void ButtonReset()
    {
        curIndex = 0;
        curStringIndex = 0;
        yourComb = emptyString;
        OutputText(yourComb);
        ButtonOn(true);
    }

    void OutputText(string theText)
    {
        theOutText.text = theText;
    }

    public void CheckForComb()
    {
        if(curIndex < cypherComb.Length)
        {
            return;
        }

        string tmpString = GetNumbersOfString(yourComb);
        
        if(tmpString == cypherComb)
        {
            Success();
            QTEEnd();
        }
        else
        {
            ButtonReset();
        }        
    }

    private string GetNumbersOfString(string origString)
    {
        string onlyNumbers = "";

        for (int i = 0; i < origString.Length; i++)
        {
            string tmp2 = "" + origString[i];
            if (isNumeric(tmp2))
            {
                onlyNumbers += origString[i];
            }
        }

        return onlyNumbers;
    }

    public void DeleteLastNum()
    {
        if(curIndex <= 0)
        {
            return;
        }

        string allNumbers = GetNumbersOfString(yourComb);
        string newNumbers = "";
        for(int i = 0; i < allNumbers.Length - 1; i++)
        {
            newNumbers += allNumbers[i];
        }
        curIndex -= 1;
        string tmpString = "<u>" + newNumbers + "</u>";

        curStringIndex = tmpString.Length;
        string addString = "";

        if(curIndex >= emptyString.Length)
        { 
            return;
        }
        int difference = emptyString.Length - curIndex;
        
        //Debug.LogFormat("Empty string length if {0}\nCurrent index is {1}\nDifference is {2}", emptyString.Length, curIndex, difference);

        if (difference != 0)
        {
            for (int i = 0; i < difference; i++)
            {
                addString += "_";
            }
        }

        yourComb = tmpString + addString;
        OutputText(yourComb);
    }

    private bool isNumeric(string strToCheck)
    {
        Regex rg = new Regex(@"^[0-9]*$");
        return rg.IsMatch(strToCheck);
    }

    public void EndButton()
    {
        QTEEnd();
        Failed();
    }

    protected override void QTEEnd()
    {
        for (int i = 0; i < theButton.Length; i++)
        {
            Destroy(theButton[i]);
        }

        curIndex = 0;
        curStringIndex = 0;
        emptyString = null;

        cypherComb = null;
        yourComb = null;
        QTEHolder.SetActive(false);        
    }
}
