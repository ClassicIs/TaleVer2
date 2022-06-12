using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleBox : InteractObject
{
    [SerializeField]
    ItemScript[] ItemToGive;
    PlayerManager PlayerManager;

    protected override void Start()
    {
        PlayerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        LongInteraction = false;
    }

    public override void FutherAction()
    {
        Debug.Log("There's no futher Action.");
    }

    public override void InterAction()
    {
        PlayerManager.Inventory.AddItems(ItemToGive);
    }
}
