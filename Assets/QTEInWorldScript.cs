using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class QTEInWorldScript : InteractObject
{    
    protected QTEObject currentQTE;
    [SerializeField]
    protected QTEHolder QTEHolder;
    [SerializeField]
    protected QTEObject.HardVariety hardness;
    [SerializeField]
    protected QTEHolder.TypesOfQTE typeOfQTE;

    protected override void Start()
    {
        currentQTE = QTEHolder.ActivateQTE(typeOfQTE);
    }

    public abstract void SuccessfullyUsed();    
    public abstract void UnSuccessfullyUsed();
    

    public override void InterAction()
    {
        currentQTE.Activate(hardness);
        currentQTE.OnSuccess += SuccessfullyUsed;
        currentQTE.OnSuccess += EndInteraction;

        currentQTE.OnFail += UnSuccessfullyUsed;
        currentQTE.OnFail += EndInteraction;

    }

    public override void EndInteraction()
    {
        base.EndInteraction();
        currentQTE.OnSuccess -= SuccessfullyUsed;
        currentQTE.OnFail -= UnSuccessfullyUsed;
    }

    public override void FutherAction()
    {
    }
}
