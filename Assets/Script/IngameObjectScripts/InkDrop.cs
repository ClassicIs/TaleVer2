using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkDrop : MonoBehaviour, DangerObject
{
    public Transform transformInk;
    public float toBig;
    public bool MaximumSize;
    public IEnumerator ToStartBig;

    public event Action OnBeingFree;

    public int HealthDamage { get; set; }
    public int InkGain { get; set; }
    public float SlowModifier { get; set; }
    public float TimeForDebuf { get; set; }
    public bool MakeStun { get; set; }
    public bool LongAction { get; set; }

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



    // Start is called before the first frame update
    void Start()
    {
        HealthDamage = 0;
        InkGain = 1;
        SlowModifier = 0.3f;
        TimeForDebuf = 0f;
        MakeStun = false;
        LongAction = true;
    }

    public void Freedom(int HealthDamage, int InkGain, float SlowModifier, float TimeForDebuf)
    {
        throw new NotImplementedException();
    }
    /*
public void ImpactStarted(out int HealthDamage, out int InkGain, out float SlowModifier, out float TimeForDebuf)
{
   HealthDamage = this.HealthDamage;
   InkGain = this.InkGain;
   SlowModifier = this.SlowModifier;
   TimeForDebuf = this.TimeForDebuf;
}

public void ImpactEnded()
{
   if(OnBeingFree != null)
   {
       OnBeingFree();
   }
   OnBeingFree = null;
}*/
}
