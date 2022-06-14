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
        
    }

    public override void InterAction()
    {
        if (contentOfBox.Length > 0)
        {
            base.InterAction();
        }
    }

    public override void EndInteraction()
    {
        base.EndInteraction();
        if (contentOfBox.Length <= 0)
        {
            if (boxAnimator != null)
            {
                boxAnimator.SetTrigger("BoxOpen");
            }
            isInteractable = false;
            this.enabled = false;
        }
    }

    public override void SuccessfullyUsed()
    {
        ItemScript[] leftovers = playerManager.Inventory.AddItems(contentOfBox);
        contentOfBox = leftovers;        

        Debug.Log("The box was openned!");
    }

    public override void UnSuccessfullyUsed()
    {
        Debug.Log("Box was not openned");
    }

    public override void FutherAction()
    {
    }
}
