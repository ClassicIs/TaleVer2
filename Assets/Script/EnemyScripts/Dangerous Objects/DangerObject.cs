using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System; 

public interface DangerObject
{
    int HealthDamage { get;  }
    int InkGain { get; }
    float SlowModifier { get; }
    float TimeForDebuf { get; }
    bool MakeStun { get; }
    bool LongAction { get; }      

    public event Action <int, int, float, float> OnBeingFree;

    void Freedom(int HealthDamage, int InkGain, float SlowModifier, float TimeForDebuf);
}
