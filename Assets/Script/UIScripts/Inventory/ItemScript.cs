using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryDate", menuName = "Inventory/New Item")]
public class ItemScript : ScriptableObject
{
    public enum ItemTypes
    {
        HealthPotion,
        InkPotion,
        MixedPotion,
        StrengthBuffPotion,
        QuestItem
    };    
    public string itemName;    
    [TextArea]
    public string itemDescription;
    public ItemTypes typeOfTheItem;
    public int itemLevel = 1;
    public Sprite itemSprite;
    public GameObject itemObject;
    public int itemCost = 100;
    
    public void PrintItem()
    {
        Debug.Log("Item name is " + itemName + "\n" + itemDescription + "\nIt's level is " + itemLevel + "\nIt's cost is " + itemCost);
    }
}
