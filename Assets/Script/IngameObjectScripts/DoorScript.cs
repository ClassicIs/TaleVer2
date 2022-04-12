using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class DoorScript : MonoBehaviour
{
    public abstract void Open();
    

    public GameObject [] keysToUnlock;

    public abstract void Close();
}