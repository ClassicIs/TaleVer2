using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : QTEInWorldScript
{
    PlayerManager playerManager;

    [Header("Items of the Box")]
    public ItemScript [] contentOfBox;
    [SerializeField]
    private Animator boxAnimator;
    
    protected override void Start()
    {
        base.Start();
        
        playerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        boxAnimator = GetComponent<Animator>();
        LongInteraction = true;
    }

    public override void InterAction()
    {
        if (contentOfBox.Length > 0)
        {
            base.InterAction();
        }
        else
        {
            Debug.Log("The box is EMPTY!");
        }
    }

    public override void EndInteraction()
    {
        base.EndInteraction();
    }

    public override void SuccessfullyUsed()
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

    public override void UnSuccessfullyUsed()
    {
        Debug.Log("Box was not openned");
    }

    public override void FutherAction()
    {
    }
}
