using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public abstract class DangerObject : MonoBehaviour
{
    public Damage StartDamage;
    public Damage EndDamage;
    public bool LongAction;
    public event Action <Damage> OnBeingFree;

    public virtual void Freedom()
    {
        if (LongAction)
        {
            if (OnBeingFree != null)
            {
                OnBeingFree(EndDamage);
            }
        }
    }
}
[Serializable]
public class Damage
{
    [SerializeField]
    int HealthDamage;
    [SerializeField]
    int InkGain;
    [SerializeField]
    float SlowModifier;
    [SerializeField]
    float TimeForDebuf;
    [SerializeField]
    bool MakeStun;
    

    public Damage(int HealthDamage, int InkGain, float SlowModifier, float TimeForDebuf, bool MakeStun/*, bool LongAction*/)
    {
        this.HealthDamage = HealthDamage;
        this.InkGain = InkGain;
        this.SlowModifier = SlowModifier;
        this.TimeForDebuf = TimeForDebuf;
        this.MakeStun = MakeStun;        
    }

    public Damage(int HealthDamage)
    {
        this.HealthDamage = HealthDamage;
        this.InkGain = 0;
        this.SlowModifier = 0;
        this.TimeForDebuf = 0;
        this.MakeStun = false;
    }

    public Damage(int HealthDamage, int InkGain, float SlowModifier)
    {
        this.HealthDamage = HealthDamage;
        this.InkGain = InkGain;
        this.SlowModifier = SlowModifier;
        this.TimeForDebuf = 0;
        this.MakeStun = false;
    }

    public Damage(bool MakeStun, float TimeForDebuf)
    {
        this.HealthDamage = 0;
        this.InkGain = 0;
        this.SlowModifier = 0;
        this.MakeStun = MakeStun;
        this.TimeForDebuf = TimeForDebuf;
        
    }

    public void GiveDamage(out int HealthDamage, out int InkGain, out float SlowModifier, out bool MakeStun, out float TimeForDebuf)
    {
        HealthDamage = this.HealthDamage;
        InkGain = this.InkGain;
        SlowModifier = this.SlowModifier;
        MakeStun = this.MakeStun;
        TimeForDebuf = this.TimeForDebuf;
    }

    public void PrintDamage()
    {
        Debug.Log("Health damage is " + HealthDamage + "\nInk Gain is " + InkGain + "Slow modifier of Speed: " + SlowModifier);
        if(MakeStun)
        {
            Debug.Log("Make a Stun.");
        }
        /*if(LongAction)
        {
            Debug.Log("Action is long.");
        }*/
    }
}
