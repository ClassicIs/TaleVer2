using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
public abstract class QTEObject : MonoBehaviour
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
        QTEEnd();
    }
    protected virtual void Success()
    {
        Debug.Log("Success!");
        if (OnSuccess != null)
        {
            OnSuccess();
        }
        QTEEnd();
    }

    protected virtual void NullActions()
    {
        OnSuccess = null;
        OnFail = null;
    }

    public enum HardVariety
    {
        easy,
        normal,
        hard
    }

    public abstract void Activate(HardVariety Hardness);

    protected abstract void QTEEnd();
}
