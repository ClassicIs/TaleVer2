using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AliveBeeing : MonoBehaviour
{
    public enum PlayerStates
    {
        moving,
        dashing,
        attacking,
        stunned,
        stopped,
        isDead
    }
    public PlayerStates currState;
}
