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

    [SerializeField]
    private string signature;


    protected override  void Start()
    {
        base.Start();
        LongInteraction = true;
        TheLetterScript = GameObject.FindGameObjectWithTag("LetterUI").GetComponent<ScriptForLetter>();
        
    }
   
    public override void InterAction()
    {           
        TheLetterScript.SetLetter(sign, theMassOfStrings, "Yours sincerely,\n" + signature);
    }   

    public override void FutherAction()
    {
        TheLetterScript.ShowLetter(false);
        EndInteraction();
    }

}
