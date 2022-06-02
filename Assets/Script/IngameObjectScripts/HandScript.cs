using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : InteractObject
{
    [SerializeField]
    protected ItemScript[] neededItem;
    [SerializeField]
    protected DoorScript doorToOpen;
    protected Animator handAnimator;
    protected PlayerManager PlayerManager;

    protected override void Start()
    {
        base.Start();
        handAnimator = GetComponent<Animator>();
        PlayerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        LongInteraction = false;
    }

    public override void InterAction()
    {
        bool allIn = false;
        int startOfNewArray = neededItem.Length;
        ItemScript[] tmpArray;
        for (int i = 0; i < neededItem.Length; i++)
        {
            if (PlayerManager.Inventory.FindItem(neededItem[i].itemName, true))
            {
                SomethingHappening(i);
                allIn = true;
                continue;
            }
            else
            {
                startOfNewArray = i;
                allIn = false;
                break;
            }
        }

        if (allIn)
        {
            neededItem = null;
            SuccessfulInteraction();
        }
        else
        {
            tmpArray = new ItemScript[neededItem.Length - startOfNewArray];
            int k = 0;
            for (int i = startOfNewArray; i < neededItem.Length; i++)
            {
                tmpArray[k] = neededItem[i];
                k++;
            }
            neededItem = tmpArray;
        }
    }

    protected virtual void SuccessfulInteraction()
    {
        handAnimator.SetTrigger("ToGetOut");
    }

    public override void FutherAction()
    {
    }

    protected virtual void SomethingHappening(int index)
    {

    }

    public void DoorOpen()
    {
        doorToOpen.Open();
        gameObject.SetActive(false);
    }

}
