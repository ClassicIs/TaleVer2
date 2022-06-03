using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BearTrap : AutoQTEScript
{
    [SerializeField]
    int damageAmount;

    private Animator Animator;

    PlayerManager PlayerManager;

    protected override void Start()
    {
        base.Start();
        PlayerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        Animator = GetComponent<Animator>();
    }

    public override void SuccessfullyUsed()
    {
        alreadyUsed = true;
        Debug.Log("Player is free!");
    }

    public override void UnSuccessfullyUsed()
    {
        Animator.SetBool("isPlayerCaught", true);
        PlayerManager.AddHealth(-damageAmount);
        alreadyUsed = true;
    }
}


