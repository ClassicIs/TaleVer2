using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class LetterScript : InteractObject
{    
    ScriptForLetter TheLetterScript;    

    [SerializeField]
    private string sign;
    [TextArea]
    [SerializeField]
    private string theMassOfStrings;
    

    protected override  void Start()
    {
        LongInteraction = true;
        TheLetterScript = GameObject.FindGameObjectWithTag("LetterUI").GetComponent<ScriptForLetter>();
        
    }
   
    public override void InterAction()
    {           
        TheLetterScript.SetLetter(sign, theMassOfStrings, "Your sincerely");
    }   

    public override void FutherAction()
    {
        TheLetterScript.ShowLetter(false);
        EndInteraction();
    }

}
