using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Lever : InteractObject 
{
    public GameObject Door;
    Animator DoorAnim;
    [SerializeField]
    bool PlayerStay;

    [SerializeField]
    QTEHolder QTEHolder;
    QTEObject CurrQTE;

    private void Start()
    {
        PlayerStay = false;       
        DoorAnim = Door.GetComponent<Animator>();
    }

    public override void InterAction()
    {        
        Debug.Log("E is pressed");
        CurrQTE = QTEHolder.ActivateQTE(QTEHolder.TypesOfQTE.Simple);
        CurrQTE.Activate(QTEObject.HardVariety.easy);
        
        Subscribe();
    }

    public override void FutherAction()
    {
        throw new System.NotImplementedException();
    }

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
        DoorAnim.SetBool("DoorIsOpen", true);
        GetComponent<Animator>().SetBool("QTEisPassed", true);        
        GetComponent<Lever>().enabled = false;        
    }
}
