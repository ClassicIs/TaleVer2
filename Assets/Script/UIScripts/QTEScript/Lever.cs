using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Lever : QTEInWorldScript 
{
    [SerializeField]
    private DLDoor doorToOpen;

    /*
    protected override void Start()
    {
        base.Start();
        PlayerStay = false;       
    }*/
    /*
    public override void InterAction()
    {        
        CurrQTE = QTEHolder.ActivateQTE(QTEHolder.TypesOfQTE.Simple);
        CurrQTE.Activate(QTEObject.HardVariety.easy);
        
        //Subscribe();
    }
    
    public override void FutherAction()
    {
        Debug.Log("No futher action");
    }*/

    /*
    void UsedLever()
    {
        EndInteraction();
        Debug.Log("Lever was used!");
        OpenDoor();
    }


    void NotUsedLever()
    {
        EndInteraction();
        Debug.Log("Lever was failed!");

    }

    void Subscribe()
    {
        if (CurrQTE != null)
        {
            CurrQTE.OnSuccess += UsedLever;
            CurrQTE.OnFail += NotUsedLever;
        }
    }

    public override void EndInteraction()
    {
        if (CurrQTE != null)
        {
            CurrQTE.OnSuccess -= UsedLever;
            CurrQTE.OnFail -= NotUsedLever;
        }
        base.EndInteraction();

    }    

    private void OpenDoor()
    {
        Door.Open();
        GetComponent<Animator>().SetBool("QTEisPassed", true);        
        GetComponent<Lever>().enabled = false;        
    }
    */
    public override void SuccessfullyUsed()
    {
        doorToOpen.OpenDoor();
    }

    public override void UnSuccessfullyUsed()
    {
        Debug.Log("Unsuccessfully Used");
    }
}
