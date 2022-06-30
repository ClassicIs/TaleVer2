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
        base.Start();
        LongInteraction = true;
        QTEHolder = GameObject.FindGameObjectWithTag("QTEHolder").GetComponent<QTEHolder>();
        cypherObject = (CypherScript)QTEHolder.ActivateQTE(QTEHolder.TypesOfQTE.Cypher);
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        boxAnimator = GetComponent<Animator>();
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
        if(contentOfBox.Length == 0)
        {
            isInteractable = false;
            if (boxAnimator != null)
            {
                boxAnimator.SetTrigger("BoxOpen");
            }
            this.enabled = false;
        }
    }

    public virtual void SuccessfullyUsed()
    {
        contentOfBox = playerManager.Inventory.AddItems(contentOfBox);
        //contentOfBox = leftovers;        
    }

    public virtual void UnSuccessfullyUsed()
    {
        Debug.Log("Box was not openned");

    }

    public override void FutherAction()
    {
        Debug.LogWarning("There's no futher Action.");

    }
}
