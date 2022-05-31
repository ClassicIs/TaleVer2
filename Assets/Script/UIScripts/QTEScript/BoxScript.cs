using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : InteractObject
{
    [SerializeField]
    PlayerManager playerManager;
    [Header("Items of the Box")]
    public ItemScript [] contentOfBox;
    [SerializeField]
    private Animator boxAnimator;
    
    protected override void Start()
    {
        boxAnimator = GetComponent<Animator>();
        LongInteraction = false;
    }

    public override void InterAction()
    {
        if (contentOfBox.Length > 0)
        {
            ItemScript[] leftovers = playerManager.Inventory.AddItems(contentOfBox);
            contentOfBox = leftovers;
            
            boxAnimator.SetBool("BoxOpen", true);
            
            Debug.Log("The box was openned!");
            
            if (contentOfBox.Length > 0)
            {
                boxAnimator.SetBool("BoxOpen", false);
            }
        }
        else
        {
            Debug.Log("The box is EMPTY!");
        }
    }

    public override void FutherAction()
    {
        throw new System.NotImplementedException();
    }
}
