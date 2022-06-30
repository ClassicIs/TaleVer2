using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FountainScript : InteractObject
{
    
    PlayerManager PlayerManagement;

    protected override void Start()
    {
        base.Start();
        LongInteraction = false;
        PlayerManagement = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
    }

    public override void InterAction()
    {
        Debug.Log("Drink out of Fountain!");
        PlayerManagement.AddInkLevel(100);
        //EndInteraction();
    }

    public override void FutherAction()
    {
        throw new System.NotImplementedException();
    }

    public override void IFPlayerNear()
    {
        
    }

    public override void IFPlayerIsAway()
    {
        throw new System.NotImplementedException();
    }
    
}
