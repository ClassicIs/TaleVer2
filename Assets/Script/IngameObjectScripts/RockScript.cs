using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RockScript : InteractObject
{
    [SerializeField]
    HandScript theHand;

    protected override void InterAction()
    {
        base.InterAction();
        theHand.isRockHere = true;
        
        gameObject.SetActive(false);
    }
}
