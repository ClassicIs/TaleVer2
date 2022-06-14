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
        isInteractable = true;
        LongInteraction = true;        
        QTEHolder = GameObject.FindGameObjectWithTag("QTEHolder").GetComponent<QTEHolder>();
        currentQTE = QTEHolder.ActivateQTE(typeOfQTE);
    }

    public abstract void SuccessfullyUsed();    
    public abstract void UnSuccessfullyUsed();
    

    public override void InterAction()
    {
        ActivateQTE();
        currentQTE.OnSuccess += SuccessfullyUsed;
        currentQTE.OnSuccess += EndInteraction;

        currentQTE.OnFail += UnSuccessfullyUsed;
        currentQTE.OnFail += EndInteraction;

    }

    protected virtual void ActivateQTE()
    {
        currentQTE.Activate(hardness);
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
