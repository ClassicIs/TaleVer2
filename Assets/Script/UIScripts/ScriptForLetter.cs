using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScriptForLetter : MonoBehaviour
{    
    Animator theLetterAnim;
    Text SignOfLetter;
    Text BodyOfLetter;
    Text EndOfLetter;

    private void Start()
    {
        theLetterAnim = GetComponent<Animator>();
        SignOfLetter = GetComponentsInChildren<Text>()[0];
        BodyOfLetter = GetComponentsInChildren<Text>()[1];
        EndOfLetter = GetComponentsInChildren<Text>()[2];
    }

    public void SetLetter(string sign = "John Doe", string body = "Hye, My Name is John Doe!", string end = "Your Faithfuly,\nJohn Doe")
    {
        SignOfLetter.text = sign;
        BodyOfLetter.text = body;
        EndOfLetter.text = end;
        ShowLetter();
    }

    public void EmptyLetter()
    {
        SignOfLetter.text = "";
        BodyOfLetter.text = "";
        EndOfLetter.text = "";
    }

    public void ShowLetter()
    {
        theLetterAnim.SetBool("LetterOpen", true);
    }
    
    public void CloseLetter()
    {        
        theLetterAnim.SetBool("LetterOpen", false);
        //EmptyLetter();
    }
}
