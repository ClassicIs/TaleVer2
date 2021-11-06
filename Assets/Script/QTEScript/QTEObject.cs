using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public class QTEObject : MonoBehaviour
{
    public event Action OnSuccess;
    public event Action OnFail;
    protected virtual void Failed()
    {
        Debug.Log("Fail!");
        if (OnFail != null)
        {
            OnFail();
        }
    }
    protected virtual void Success()
    {
        Debug.Log("Success!");
        if (OnSuccess != null)
        {
            OnSuccess();
        }
    }

}
