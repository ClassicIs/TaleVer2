using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class AutoQTEScript : MonoBehaviour
{
    protected QTEObject currentQTE;
    [SerializeField]
    protected QTEHolder QTEHolder;
    [SerializeField]
    protected QTEObject.HardVariety hardness;
    [SerializeField]
    protected QTEHolder.TypesOfQTE typeOfQTE;
    public bool alreadyUsed;

    public event Action OnQTEEnd;
    

    protected virtual void Start()
    {
        alreadyUsed = false;
        currentQTE = QTEHolder.ActivateQTE(typeOfQTE);
    }

    public abstract void SuccessfullyUsed();
    
    public abstract void UnSuccessfullyUsed();
    

    public void StartOfQTE()
    {
        currentQTE.Activate(hardness);
        currentQTE.OnSuccess += SuccessfullyUsed;
        currentQTE.OnSuccess += EndInteraction;

        currentQTE.OnFail += UnSuccessfullyUsed;
        currentQTE.OnFail += EndInteraction;

    }

    public void EndInteraction()
    {
        if(OnQTEEnd != null)
            OnQTEEnd();
        currentQTE.OnSuccess -= SuccessfullyUsed;
        currentQTE.OnFail -= UnSuccessfullyUsed;
    }
}
