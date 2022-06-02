using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BearTrap : AutoQTEScript
{
    [SerializeField]
    int damageAmount;

    PlayerManager PlayerManager;

    protected override void Start()
    {
        base.Start();
        PlayerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
    }

    public override void SuccessfullyUsed()
    {
        alreadyUsed = true;
        Debug.Log("Player is free!");
    }

    public override void UnSuccessfullyUsed()
    {
        PlayerManager.AddHealth(-damageAmount);
        alreadyUsed = true;
    }
}


