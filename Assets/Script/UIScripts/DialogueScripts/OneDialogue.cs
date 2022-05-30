using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneDialogue : InteractObject
{
    [Header("Here are characters to speak:")]
    public Character[] characterPool;
    [Header("Here are their phrases:")]
    public DialogueLine[] theLines;
    
    private DialogueScript DialogScript;

    protected override void Start()
    {
        LongInteraction = true;
        DialogScript = GameObject.FindGameObjectWithTag("DialogHolder").GetComponent<DialogueScript>();
        DialogScript.OnDialogueEnd += EndInteraction;
    }

    public override void InterAction()
    {        
        DialogScript.ToStartDialogue(theLines, characterPool);
    }

    public override void FutherAction()
    {           
        DialogScript.NextLine();
    }


}
