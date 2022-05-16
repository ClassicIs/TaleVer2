using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class DialogueScript : MonoBehaviour
{
    [SerializeField]
    Text theSign;
    [SerializeField]
    Text theMessage;
    List<DialogueLine> DialogueToOutput;
    IEnumerator theMesCoroutine;
    IEnumerator mesOutput;

    bool lineInProgress = false;

    public event Action OnDialogueEnd;

    [SerializeField]
    float speedToPrint = 0.2f;
    int curLet = 0;

    // Start is called before the first frame update
    void Start()
    {
        DialogueToOutput = new List<DialogueLine>();
        theSign = GetComponentsInChildren<Text>()[0];
        theMessage = GetComponentsInChildren<Text>()[1];
        DialogOff();
        
    }
    // Service functions
    void DialogOn()
    {
        theSign.enabled = true;
        theMessage.enabled = true;
    }

    void DialogOff()
    {
        theSign.enabled = false;
        theMessage.enabled = false;
        if (DialogueToOutput != null)
        {
            DialogueToOutput.Clear();
        }
    }

    public void ToStartDialogue(DialogueLine[] theLines)
    {
        DialogOn();
        foreach (DialogueLine line in theLines)
        {
            DialogueToOutput.Add(line);
        }
        curLet = 0;        
        ChangeLine(theLines[curLet].theSpeaker.charName, theLines[curLet].theLine);        
    }
    
    public void NextLine()
    {
        if(DialogueToOutput == null)
        {
            return;
        }

        if (lineInProgress)
        {
            Debug.Log("Line progress is stopped!");
            StopCoroutine(mesOutput);
            lineInProgress = false;
            theMessage.text = DialogueToOutput[curLet].theLine;            
        }
        else
        {
            Debug.Log("Line is finished!");

            curLet = curLet + 1;

            if (curLet < DialogueToOutput.Count)
            {
                ChangeLine(DialogueToOutput[curLet].theSpeaker.charName, DialogueToOutput[curLet].theLine);
            }
            else
            {
                DialogOff();
                if (OnDialogueEnd != null)
                {
                    OnDialogueEnd();
                }                
            }
        }
    }

    /*IEnumerator PrintMes(List <DialogueLine> theLines)
    {        
        while (true)
        {                          
            if(Input.GetKeyDown(KeyCode.Backspace))
            {                
                if (lineInProgress)
                {
                    Debug.Log("Line progress is stopped!");
                    StopCoroutine(mesOutput);
                    lineInProgress = false;
                    theMessage.text = theLines[curLet].theLine;
                }
                else
                {
                    Debug.Log("Line is finished!");
                    
                    curLet = curLet + 1;
                    
                    if (curLet < theLines.Count)
                    {
                        ChangeLine(theLines[curLet].theSpeaker.charName, theLines[curLet].theLine);
                    }
                    else
                    {
                        if (OnDialogueEnd != null)
                        {
                            OnDialogueEnd();
                        }
                        break;
                    }
                }
            }
            yield return null;
        }
    }*/

    IEnumerator PrintByLetter(string theMesToPr)
    {
        lineInProgress = true;
        for (int i = 0; i < theMesToPr.Length; i++)
        {
            theMessage.text = theMessage.text + theMesToPr[i];
            yield return new WaitForSeconds(speedToPrint);
        }
        lineInProgress = false;
        Debug.Log("lineInProgress = false!");
    }

    void ChangeLine(string name, string message)
    {
        theSign.text = name;
        theMessage.text = "";
        mesOutput = PrintByLetter(message);
        StartCoroutine(mesOutput);        
    }
}
