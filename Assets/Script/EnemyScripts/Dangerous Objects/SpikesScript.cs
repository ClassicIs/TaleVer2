using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesScript : DangerObject
{
    void Start()
    {
        LongAction = false;
        StartDamage = new Damage(1);
    }
}
