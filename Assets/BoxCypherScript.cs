using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxCypherScript : InteractObject
{
    [SerializeField]
    private int cypher;
    CypherScript cypherObject;
    PlayerManager playerManager;
    [SerializeField]
    protected QTEHolder QTEHolder;

    [Header("Items of the Box")]
    public ItemScript[] contentOfBox;
    [SerializeField]
    private Animator boxAnimator;

    protected override void Start()
    {
        cypherObject = (CypherScript)QTEHolder.ActivateQTE(QTEHolder.TypesOfQTE.Cypher);
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        boxAnimator = GetComponent<Animator>();
        LongInteraction = true;
    }

    public override void InterAction()
    {
        cypherObject.Activate(cypher.ToString());
        cypherObject.OnSuccess += SuccessfullyUsed;
        cypherObject.OnSuccess += EndInteraction;

        cypherObject.OnFail += UnSuccessfullyUsed;
        cypherObject.OnFail += EndInteraction;
    }


    public override void EndInteraction()
    {
        base.EndInteraction();
        cypherObject.OnSuccess -= SuccessfullyUsed;
        cypherObject.OnFail -= UnSuccessfullyUsed;
    }

    public virtual void SuccessfullyUsed()
    {
        ItemScript[] leftovers = playerManager.Inventory.AddItems(contentOfBox);
        contentOfBox = leftovers;
        if (boxAnimator != null)
        {
            boxAnimator.SetBool("BoxOpen", true);
        }

        Debug.Log("The box was openned!");

        if (contentOfBox.Length > 0)
        {
            if (boxAnimator != null)
            {
                boxAnimator.SetBool("BoxOpen", false);
            }
        }
    }

    public virtual void UnSuccessfullyUsed()
    {
        Debug.Log("Box was not openned");

    }

    public override void FutherAction()
    {
    }
}
