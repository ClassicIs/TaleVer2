using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

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
    string yourComb;
    [SerializeField]
    TextMeshProUGUI theOutText;
    bool buttonsActive;


    public override void Activate(HardVariety Hardness)
    {
        Debug.LogWarning("You cannot use Cyphers like this. You need to write a Cypher first!");
    }

    // Start is called before the first frame update
    public void Activate(string cypherComb)
    {
        if(cypherComb == "")
        {
            Debug.LogWarning("You need to write a Cypher first!");
            return;
        }

        this.cypherComb = cypherComb;
        QTEHolder.SetActive(true);
        ButtonInstantiate();
        OutputText("");
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
        string tmpComb = yourComb + numOfButton;
        if (tmpComb.Length <= cypherComb.Length)
        {            
            yourComb = tmpComb;
            OutputText(yourComb);
        }
        else
        {
            ButtonOn(false);
        }
    }
    
    public void ButtonReset()
    {
        yourComb = "";
        OutputText(yourComb);
        ButtonOn(true);
    }

    void OutputText(string theText)
    {
        theOutText.text = theText;
    }

    public void CheckForComb()
    {
        if(yourComb.Length == cypherComb.Length)
        {
            if(yourComb == cypherComb)
            {
                Success();
                QTEEnd();
            }
            else
            {
                ButtonReset();
            }
            
        }
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

        cypherComb = null;
        yourComb = null;
        QTEHolder.SetActive(false);        
    }
}
