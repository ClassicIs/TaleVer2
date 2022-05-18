using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BearTrap : DangerObject
{
    bool PlayerStay;

    [SerializeField]
    isQtePassed qte;

    protected override void Freedom()
    {
        base.Freedom();
    }
    /*

// Start is called before the first frame update
void Start()
{
PlayerManagerScript = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
MakeStun = true;

}   

public override void MakeDamage()
{
Debug.Log("You are in trap, silly boy!");
GetComponent<Animator>().SetBool("isPlayerCaught", true);
Subscribe();
qte.Activate();                    
}


void GoodOutcome()
{
Debug.Log("You are out of Trap healthy.");
Untrapped();
Unsubscribe();
}

void BadOutcome()
{
Debug.Log("You are out of Trap NOT healthy.");
Untrapped();
PlayerManagerScript.AddHealth(-HealthDamage);
Unsubscribe();
}

void Subscribe()
{
qte.OnSuccess += GoodOutcome;
qte.OnFail += BadOutcome;
}

void Unsubscribe()
{
qte.OnSuccess -= GoodOutcome;
qte.OnFail -= BadOutcome;
}*/
}


