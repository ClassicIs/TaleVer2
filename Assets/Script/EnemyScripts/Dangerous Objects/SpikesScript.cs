using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesScript : DangerObject
{

    public int HealthDamage { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public int InkGain { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public float SlowModifier { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public float TimeForDebuf { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool MakeStun { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }
    public bool LongAction { get => throw new NotImplementedException(); set => throw new NotImplementedException(); }

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
        HealthDamage = 1;
        InkGain = 0;
        SlowModifier = 0;
        TimeForDebuf = 0;
        MakeStun = false;
        LongAction = false;
    }

    public void Freedom(int HealthDamage, int InkGain, float SlowModifier, float TimeForDebuf)
    {
        //DangerObject.OnBeingFree(HealthDamage, InkGain, SlowModifier, TimeForDebuf);
    }
}
