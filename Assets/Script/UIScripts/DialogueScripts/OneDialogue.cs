using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OneDialogue : InteractObject
{
    public DialogueLine[] theLines;
    private DialogueScript DialogScript;

    private void Start()
    {
        LongInteraction = true;
        DialogScript = GameObject.FindGameObjectWithTag("DialogHolder").GetComponent<DialogueScript>();
        DialogScript.OnDialogueEnd += EndInteraction;
    }

    

    public override void InterAction()
    {        
        DialogScript.ToStartDialogue(theLines);
    }

    public override void FutherAction()
    {           
        DialogScript.NextLine();
    }


}
