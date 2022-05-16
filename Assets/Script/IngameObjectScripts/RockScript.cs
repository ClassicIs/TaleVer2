using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RockScript : InteractObject
{
    [SerializeField]
    HandScript theHand;

    public override void InterAction()
    {
        theHand.isRockHere = true;
        
        gameObject.SetActive(false);
        EndInteraction();
    }

    public override void FutherAction()
    {
        Debug.Log("There is nothing to do here!");
    }
}
