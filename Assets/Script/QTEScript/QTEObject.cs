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
        if (OnFail != null)
        {
            OnFail();
        }
    }
    protected virtual void Success()
    {
        if (OnSuccess != null)
        {
            OnSuccess();
        }
    }
}
