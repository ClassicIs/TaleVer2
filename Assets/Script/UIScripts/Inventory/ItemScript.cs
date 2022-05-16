using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "InventoryDate", menuName = "Inventory/New Item")]
public class ItemScript : ScriptableObject
{   
    public string ItemName;    
    [TextArea]
    public string ItemDescription;

    public Sprite ItemSprite;

    public GameObject ItemObject;

    public int ItemCost = 500;
    public void ItemAction()
    {}
}
