using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InventoryScript 
{
    private List<ItemScript> Items;
    private int maxSize;

    public event Action OnInventoryUpdate;
    public event Action<string, string> OnNewItemAdd;

    public InventoryScript (int sizeOfInventory)
    {        
        Items = new List<ItemScript> ();
        maxSize = sizeOfInventory;
        Debug.Log("Inventory size is " + maxSize);
    }

    public List<ItemScript> ReturnItems()
    {
        return Items;
    }

    public int ReturnSize()
    {
        return maxSize;
    }

    public bool AddItem(ItemScript Item)
    {
        if (Items.Count < maxSize)
        {            
            Items.Add(Item);

            if (OnInventoryUpdate != null)
            {
                OnInventoryUpdate();
            }
            Item.PrintItem();
            return true;
        }
        else
        {
            Debug.LogWarning("Item named " + Item.itemName + " was not added to the Inventory. It's full!");
            return false;
        }
    }
    
    public ItemScript[] AddItems(ItemScript [] Items)
    {
        int freeSpace = maxSize - this.Items.Count;
        int spaceToUse = freeSpace - Items.Length;
        Debug.LogFormat("Free Space is {0}", freeSpace);
        List<ItemScript> tmpItems = new List<ItemScript>();

        if (spaceToUse >= 0)
        {
            foreach (ItemScript theItem in Items)
            {
                this.Items.Add(theItem);
                if(OnNewItemAdd != null)
                {
                    OnNewItemAdd(theItem.itemName, ColorByType(theItem.typeOfTheItem));
                }
            }
            Debug.Log("All items were placed in Inventory");
        }
        else
        {
            for (int i = 0; i < freeSpace; i++)
            {
                this.Items.Add(Items[i]);
            }

            for (int i = freeSpace; i < Items.Length; i++)
            {
                tmpItems.Add(Items[i]);
            }
        }
        
        if(OnInventoryUpdate != null)
        {
            OnInventoryUpdate();
        }

        return tmpItems.ToArray();
    }

    private string ColorByType(ItemScript.ItemTypes typeOfItem)
    {
        string color;
        string healthColor = "#F31F35"; // Red
        string inkColor = "#F31F35"; // Blue
        string mixedColor = "#21CEF3"; // Green
        string strengthColor = "#213FF3"; // Dark Blue
        string questColor = "#F3C821"; // Yellow

        switch (typeOfItem)
        {
            case ItemScript.ItemTypes.HealthPotion:
                return healthColor;
            case ItemScript.ItemTypes.InkPotion:
                return inkColor;
            case ItemScript.ItemTypes.MixedPotion:
                return mixedColor;
            case ItemScript.ItemTypes.StrengthBuffPotion:
                return strengthColor;
            case ItemScript.ItemTypes.QuestItem:
                return questColor;
            default:
                return "#FFFFFF";
        }
    }

    public InventoryScript ReturnInventory()
    {
        return this;
    }

    public bool ReturnItem(int indexOfItem, out ItemScript itemToBeUsed)
    {
        if(indexOfItem < Items.Count)
        {
            itemToBeUsed = Items[indexOfItem];
            return true;
        }
        itemToBeUsed = new ItemScript();
        return false;
    }

    public bool IsItemInInventory(string itemName)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (itemName == Items[i].itemName)
            {
                RemoveItemAt(i);
                return true;
            }
        }
        return false;
    }

    public bool IsItemInInventory(ItemScript item)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (item == Items[i])
            {
                RemoveItemAt(i);
                return true;
            }
        }
        return false;
    }

    public bool FindItem(ItemScript.ItemTypes itemToGive, out int indexOfTheItem)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if(Items[i].typeOfTheItem == itemToGive)
            {
                indexOfTheItem = i;
                return true;
            }
        }
        indexOfTheItem = 404;
        return false;
    }

    public bool FindItem(string itemName, out int indexOfTheItem)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].itemName == itemName)
            {
                indexOfTheItem = i;
                return true;
            }
        }
        indexOfTheItem = 404;
        return false;
    }

    public bool FindItem(string itemName, bool use)
    {
        for (int i = 0; i < Items.Count; i++)
        {
            if (Items[i].itemName == itemName)
            {
                RemoveItemAt(i);
                return true;
            }
        }        
        return false;
    }

    public bool RemoveItemAt(int itemIndex)
    {
        if(itemIndex <= Items.Count)
        {
            Items.RemoveAt(itemIndex);

            if (OnInventoryUpdate != null)
            {
                OnInventoryUpdate();
            }

            return true;
        }

        return false;
    }
}
