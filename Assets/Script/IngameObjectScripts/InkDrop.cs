using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkDrop : DangerObject
{
    public Transform transformInk;

    // Start is called before the first frame update
    void Start()
    {
        LongAction = true;
        StartDamage = new Damage(0, 1, 0.3f, 0f, false);
        EndDamage = new Damage(0, 0, 0, 0, false);
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
