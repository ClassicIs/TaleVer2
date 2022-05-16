using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BearTrap : MonoBehaviour, DangerObject
{
    bool PlayerStay;    

    [SerializeField]
    isQtePassed qte;    
    PlayerManager PlayerManagerScript;

    public int HealthDamage => throw new NotImplementedException();

    public int InkGain => throw new NotImplementedException();

    public float SlowModifier => throw new NotImplementedException();

    public float TimeForDebuf => throw new NotImplementedException();

    public bool MakeStun => throw new NotImplementedException();

    public bool LongAction => throw new NotImplementedException();

    //public DamageUnit ThisDamage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

    public event Action OnBeingFree;

    event Action<int, int, float, float> DangerObject.OnBeingFree
    {
        add
        {
            throw new NotImplementedException();
        }

        remove
        {
            throw new NotImplementedException();
        }
    }

    public void Freedom(int HealthDamage, int InkGain, float SlowModifier, float TimeForDebuf)
    {
        throw new NotImplementedException();
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


