using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ScriptForLetter : MonoBehaviour
{    
    Animator theLetterAnim;
    [SerializeField]
    TextMeshProUGUI SignOfLetter;
    [SerializeField]
    TextMeshProUGUI BodyOfLetter;
    [SerializeField]
    TextMeshProUGUI EndOfLetter;
    [SerializeField]
    Image bgPanel;
    [SerializeField]
    Image bgLetter;
    [SerializeField]
    float speedToShow;

    private void Start()
    {
        theLetterAnim = GetComponent<Animator>();
    }

    public void SetLetter(string sign = "John Doe", string body = "Hye, My Name is John Doe!", string end = "Yours Faithfuly,\nJohn Doe")
    {
        SignOfLetter.text = sign;
        BodyOfLetter.text = body;
        EndOfLetter.text = end;
        ShowLetter(true);
        
    }

    public void EmptyLetter()
    {
        SignOfLetter.text = "";
        BodyOfLetter.text = "";
        EndOfLetter.text = "";
    }

    public void ShowLetter(bool show)
    {
        float desiredOpacityText = 0f;
        float desiredOpacityPanel = 0f;
        if (show)
        {
            desiredOpacityText = 1f;
            desiredOpacityPanel = 0.45f;
        }

        StartCoroutine(toShow(desiredOpacityText, SignOfLetter));
        StartCoroutine(toShow(desiredOpacityText, BodyOfLetter));
        StartCoroutine(toShow(desiredOpacityText, EndOfLetter));
        StartCoroutine(toShow(desiredOpacityPanel, bgPanel));
        StartCoroutine(toShow(desiredOpacityText, bgLetter));
        //theLetterAnim.SetBool("LetterOpen", true);
    }
    
    IEnumerator toShow(float desireOpacity, TextMeshProUGUI text)
    {
        float startOpacity = text.color.a;
        Color tmpCol;

        while (Mathf.Abs(startOpacity - desireOpacity) > 0.1f)
        {
            if(startOpacity > desireOpacity)
            {
                startOpacity -= speedToShow;
            }
            else if(startOpacity < desireOpacity)
            {
                startOpacity += speedToShow;
            }
            
            tmpCol = text.color;
            tmpCol.a = startOpacity;
            text.color = tmpCol;
            yield return null;
        }

        tmpCol = text.color;
        tmpCol.a = desireOpacity;
        text.color = tmpCol;
    }

    IEnumerator toShow(float desireOpacity, Image text)
    {
        float startOpacity = text.color.a;
        Color tmpCol;
        
        while (Mathf.Abs(startOpacity - desireOpacity) > 0.1f)
        {
            if (startOpacity > desireOpacity)
            {
                startOpacity -= speedToShow;
            }
            else if (startOpacity < desireOpacity)
            {
                startOpacity += speedToShow;
            }

            tmpCol = text.color;
            tmpCol.a = startOpacity;
            text.color = tmpCol;
            yield return null;
        }

        tmpCol = text.color;
        tmpCol.a = desireOpacity;
        text.color = tmpCol;
    }    
}
