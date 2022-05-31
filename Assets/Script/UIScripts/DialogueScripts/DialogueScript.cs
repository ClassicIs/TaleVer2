using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;
using TMPro;

public struct DialogueComplete
{
    public Character curCharacter;
    public string charLine;
}

public class DialogueScript : MonoBehaviour
{
    [Header("References")]
    [SerializeField]
    TextMeshProUGUI theSign;
    [SerializeField]
    Image signBG;
    [SerializeField]
    TextMeshProUGUI theMessage;
    [SerializeField]
    Image messageBG;
    [SerializeField]
    Image characterImage;
    [Header("Padding")]
    [SerializeField]   
    float paddingVert;
    [SerializeField]
    float paddingHor;
    List<DialogueComplete> DialogueToOutput;    

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
        paddingHor = 15f;
        paddingVert = 8f;
        DialogueToOutput = new List<DialogueComplete>();

        DialogOn(false);
        
    }
    // Service functions
    void DialogOn(bool turnOn)
    {
        theSign.enabled = turnOn;
        theMessage.enabled = turnOn;
        signBG.enabled = turnOn;
        messageBG.enabled = turnOn;
        characterImage.enabled = turnOn;

        if (!turnOn)
        {
            theSign.text = "";
            theMessage.text = "";
            theSign.color = Color.white;

            characterImage.sprite = null;
            curLet = 0;
            if (DialogueToOutput != null)
            {
                DialogueToOutput.Clear();
            }
        }
    }

    public void ToStartDialogue(DialogueLine[] theLines, Character[] characterPool)
    {
        DialogOn(true);
        foreach (DialogueLine line in theLines)
        {
            Character charTMP = new Character();
            switch(line.curCharacter)
            {
                case DialogueLine.Cast.char1:
                    charTMP = characterPool[0];
                    break;
                case DialogueLine.Cast.char2:
                    charTMP = characterPool[1];
                    break;
                case DialogueLine.Cast.char3:
                    charTMP = characterPool[2];
                    break;
                default:
                    Debug.LogWarning("There is no character 4+.");
                    break;
            }
            DialogueComplete tmpDialogue;
            tmpDialogue.charLine = line.theLine;
            tmpDialogue.curCharacter = charTMP;

            DialogueToOutput.Add(tmpDialogue);
        }
        curLet = 0;        
        ChangeLine(DialogueToOutput[curLet]);        
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
            theMessage.text = DialogueToOutput[curLet].charLine;            
        }
        else
        {
            Debug.Log("Line is finished!");

            curLet = curLet + 1;

            if (curLet < DialogueToOutput.Count)
            {
                ChangeLine(DialogueToOutput[curLet]);
            }
            else
            {
                DialogOn(false);
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

    void ChangeLine(DialogueComplete Dialogue)
    {
        Character currentCharacter = Dialogue.curCharacter;
        theSign.text = currentCharacter.charName;
        theSign.color = currentCharacter.charColor;
        characterImage.sprite = currentCharacter.theCharSpr;
        theSign.ForceMeshUpdate();
        Vector2 preferedSize = theSign.GetRenderedValues(false);
        signBG.GetComponent<RectTransform>().sizeDelta = new Vector2(preferedSize.x + paddingHor * 2, preferedSize.y + paddingVert * 2);
        theMessage.text = "";
        mesOutput = PrintByLetter(Dialogue.charLine);
        StartCoroutine(mesOutput);        
    }
}
