using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveBeeing : MonoBehaviour
{
    protected enum PlayerStates
    {
        moving,
        dashing,
        attacking,
        stunned
    }
    protected PlayerStates currState;
}
