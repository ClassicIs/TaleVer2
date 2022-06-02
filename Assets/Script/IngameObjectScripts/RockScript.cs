using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class RockScript : InteractObject
{
    [SerializeField]
    ItemScript itemToGive;
    PlayerManager PlayerManager;
    SpriteRenderer SpriteRenderer;

    protected override void Start()
    {
        SpriteRenderer = GetComponent<SpriteRenderer>();

        LongInteraction = false;
        PlayerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        PlayerManager.Inventory.AddItem(itemToGive);
    }


    public override void InterAction()
    {
        if (itemToGive != null)
        {
            SpriteRenderer.sprite = null;
            PlayerManager.Inventory.AddItem(itemToGive);
            itemToGive = null;
            this.enabled = false;
            gameObject.SetActive(false);
        }
    }

    public override void FutherAction()
    {
        //Debug.Log("There is nothing to do here!");
    }
}
