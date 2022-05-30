using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : InteractObject
{
    public string [] contentOfBox;
    private CircleCollider2D theColliderBox;

    protected override void Start()
    {
        LongInteraction = false;
        theColliderBox = GetComponent<CircleCollider2D>();
    }

    public override void InterAction()
    {
        theColliderBox.enabled = false;
        Debug.Log("The box is openned!");
    }

    public override void FutherAction()
    {
        throw new System.NotImplementedException();
    }
}
