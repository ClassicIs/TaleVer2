using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CypherScript : QTEObject
{
    [SerializeField]
    GameObject [] theButton = new GameObject[9];
    [SerializeField]
    GameObject strButton;

    [SerializeField]
    string cypherComb;
    [SerializeField]
    string yourComb;

    // Start is called before the first frame update
    void Start()
    {
        
        ButtonInstantiate();
    }

    void ButtonInstantiate()
    {
        
        for (int i = 0; i < theButton.Length; i++)
        {

            int count = i + 1;
            Debug.Log("the true i is " + i);
            theButton[i] = Instantiate(strButton, transform);
            
            theButton[i].GetComponent<Button>().onClick.AddListener(() => OnButtonClick(count));

            theButton[i].GetComponentInChildren<Text>().text = (i + 1).ToString();
        }
    }

    void OnButtonClick(int numOfButton)
    {
        string tmpComb = yourComb + numOfButton;
        CheckForComb(tmpComb);
    }

    void CheckForComb(string tryComb)
    {
        if(tryComb.Length < cypherComb.Length)
        {
            yourComb = tryComb;
        }
        else if(tryComb.Length == cypherComb.Length)
        {
            if(tryComb == cypherComb)
            {
                Success();
            }
            else
            {
                Failed();                
            }
            Close();
        }
    }

    protected override void Close()
    {
        base.Close();
        int currSize = theButton.Length;
        for (int i = 0; i < currSize; i++)
        {
            Destroy(theButton[i]);
        }
        gameObject.SetActive(false);
        
    }

    

    // Update is called once per frame
    void Update()
    {
        
    }
}
