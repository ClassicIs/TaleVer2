using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public abstract class InteractObject : MonoBehaviour
{
    private SpriteRenderer InterObjSpriteRend;
    protected IEnumerator ToChangeAlpha;

    public bool LongInteraction;

    protected virtual void Start()
    {
        InterObjSpriteRend = GetComponent<SpriteRenderer>();
        ToChangeAlpha = ChangeAlpha();
    }

    public event Action OnEndOfInteraction;    

    public virtual void EndInteraction()
    {
        if(OnEndOfInteraction != null)
        {
            OnEndOfInteraction();
        }
    }

    public abstract void InterAction();    

    public abstract void FutherAction();
   
    public virtual void IFPlayerNear()
    {
        if(!InterObjSpriteRend)
        {
            Debug.Log("Couldn't find a SpriteRenderer");
            return;
        }
        StartCoroutine(ToChangeAlpha);
    }
    
    public virtual void IFPlayerIsAway()
    {
        if (!InterObjSpriteRend)
        {
            Debug.Log("Couldn't find a SpriteRenderer");
            return;
        }
        StopCoroutine(ToChangeAlpha);
        InterObjSpriteRend.color = new Color(InterObjSpriteRend.color.r, InterObjSpriteRend.color.g, 1);
    }

    protected IEnumerator ChangeAlpha()
    {
        float ChangeCol = -1;
        while (true)
        {
            if (InterObjSpriteRend.color.b == 1)
            {
                ChangeCol = -1;
            }
            else if (InterObjSpriteRend.color.b <= 0)
            {
                ChangeCol = 1;
            }

            InterObjSpriteRend.color = new Color(InterObjSpriteRend.color.r, InterObjSpriteRend.color.g, InterObjSpriteRend.color.b + 0.05f * ChangeCol);
            yield return null;
        }
    }
}
