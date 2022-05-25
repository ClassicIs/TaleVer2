using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventoryScript : MonoBehaviour
{
    
    [SerializeField]
    private LayerMask LayerToDrop;

    [SerializeField]
    private LayerMask LayersNotToDrop;

    [SerializeField]
    GameObject InventorySlot;
    [SerializeField]
    GameObject PlayerObject;
    [SerializeField]
    GameObject inventoryContainer;

    RectTransform positionOfInventory;
    float speedOfInventory;
    [SerializeField] PlayerManager PlayerManager;

    private bool uiInventoryIsShown;
    private IEnumerator toShowInventoryShell;

    List <GameObject> InventorySlots;
    InventoryScript Inventory;
    IEnumerator ToDropItem;


    // Test
    [SerializeField]
    GameObject TestObject;
    private void Awake()
    {
        speedOfInventory = 0.2f;
        positionOfInventory = GetComponent<RectTransform>();
        uiInventoryIsShown = false;
        InventorySlots = new List<GameObject>();
    }

    public void SetInventory(InventoryScript theInventory)
    {
        Inventory = theInventory;
        for (int i = 0; i < theInventory.ReturnSize(); i++)
        {
            /*GameObject TheInventorySlot = Instantiate(InventorySlot, inventoryContainer.transform);
            InventorySlots.Add(TheInventorySlot);*/
            //Instantiate(Panels, inventoryContainer.transform);
        }
        UpdateInventory();
    }
    
    public void UpdateInventory()
    {
        EmptyInventory();
        List<ItemScript> itemsFromInventories = Inventory.ReturnItems();
        for (int i = 0; i < itemsFromInventories.Count; i++)
        {
            int tmpI = i;
            GameObject theInventorySlot = Instantiate(InventorySlot, inventoryContainer.transform);
            InventorySlots.Add(theInventorySlot);
            UIInventorySlot thisSlot = theInventorySlot.GetComponent<UIInventorySlot>();
            thisSlot.PutSprite(itemsFromInventories[i].itemSprite);

            thisSlot.cancelButton.onClick.AddListener(delegate {
                
                Debug.Log("Item index is " + tmpI);
                DropItem(itemsFromInventories[tmpI], PlayerObject.transform.position);                                    
                Inventory.RemoveItemAt(tmpI);
                UpdateInventory();
                
            
            } );
            thisSlot.itemButton.onClick.AddListener(delegate {
                ItemScript itemToUse;
                if (Inventory.ReturnItem(tmpI, out itemToUse))
                {
                    if (itemToUse.typeOfTheItem != ItemScript.ItemTypes.QuestItem)
                    {
                        if (UseItem(itemToUse))
                        {
                            Debug.Log("Using item " + tmpI);
                            Inventory.RemoveItemAt(tmpI);
                            UpdateInventory();
                        }
                    }
                    else
                    {
                        Debug.Log("Cannot use quest Items");
                    }
                }
            });
        }
    }
    
    private bool UseItem(ItemScript ItemToUse)
    {
        int healthAdd = 2;
        int inkDrop = 20;
        int mixHealthAdd = 1;
        int mixInkDrop = 1;
        int strengthBuff = 2;

        int theItemLevel = ItemToUse.itemLevel;
        switch (ItemToUse.typeOfTheItem)
        {
            case ItemScript.ItemTypes.HealthPotion:
                PlayerManager.AddHealth(healthAdd * theItemLevel);
                return true;
            case ItemScript.ItemTypes.InkPotion:
                PlayerManager.AddInkLevel(inkDrop * theItemLevel);
                return true;
            case ItemScript.ItemTypes.MixedPotion:
                PlayerManager.AddInkLevel(mixInkDrop * theItemLevel);
                PlayerManager.AddHealth(mixHealthAdd * theItemLevel);
                return true;
            case ItemScript.ItemTypes.StrengthBuffPotion:
                // To boost Player strength in theItemLevel times
                Debug.Log("Strength of Player was boosted in " + strengthBuff * theItemLevel + " times.");
                return true;
            case ItemScript.ItemTypes.QuestItem:
                Debug.Log("You cannot use Quest Items. They are made for Quests!");
                return false;
            default:
                return false;
        }
    }

    private void EmptyInventory()
    {
        if (InventorySlots.Count != 0)
        {
            for (int i = 0; i < InventorySlots.Count; i++)
            {
                Destroy(InventorySlots[i]);
            }
            InventorySlots.Clear();

        }        
    }
    
    public void ShowInventoryUI()
    {
        if (toShowInventoryShell != null)
        {
            StopCoroutine(toShowInventoryShell);
            toShowInventoryShell = null;
        }
        if (!uiInventoryIsShown)
        {
            toShowInventoryShell = toShowInventory(true);
        }
        else
        {
            toShowInventoryShell = toShowInventory(false);
        }
        StartCoroutine(toShowInventoryShell);
        uiInventoryIsShown = !uiInventoryIsShown;
    }

    IEnumerator toShowInventory(bool showOrNot)
    {
        Vector3 desiredPosition;
        if (showOrNot)
        {
            desiredPosition = new Vector3(-461, 0, 0);
        }
        else
        {
            ActivateInventory(false);
            desiredPosition = new Vector3(450, 0, 0);
        }

        while (!positionOfInventory.anchoredPosition.Equals(desiredPosition))
        {
            positionOfInventory.anchoredPosition = Vector3.Lerp(positionOfInventory.anchoredPosition, desiredPosition, speedOfInventory);
            yield return null;
        }
        

        if (showOrNot)
        {
            ActivateInventory(true);
            Debug.Log("Inventory is usable.");
        }
        else
        {
            Debug.Log("Inventory is unusable.");
        }
    }

    private void ActivateInventory(bool activate)
    {
        for (int i = 0; i < InventorySlots.Count; i++)
        {
            InventorySlots[i].GetComponent<UIInventorySlot>().cancelButton.enabled = activate;
            InventorySlots[i].GetComponent<UIInventorySlot>().itemButton.enabled = activate;
        }
    }

    public void DropItem(ItemScript theItemToDrop, Vector3 playerPosition)
    {
        ToDropItem = FindItemPosition(theItemToDrop, playerPosition);
        StartCoroutine(ToDropItem);
    }

    IEnumerator FindItemPosition(ItemScript theItemToDrop, Vector3 playerPosition)
    {
        Vector3 randomAddition;
        Vector3 finalItemPosition;
        RaycastHit2D notToDrop;
        RaycastHit2D toDrop;        

        //int numberOfTries = 20;
        int currNumOfTries = 0;
        do
        {
            randomAddition = new Vector3(Random.Range(-2, 2), Random.Range(-2, 2), 0f);
            finalItemPosition = playerPosition + randomAddition;
            notToDrop = Physics2D.BoxCast(finalItemPosition, new Vector2(1, 1), 0f, Vector2.zero, 1f, LayersNotToDrop);
            toDrop = Physics2D.BoxCast(finalItemPosition, new Vector2(1, 1), 0f, Vector2.zero, 1f, LayerToDrop);
            Instantiate(TestObject, finalItemPosition, Quaternion.identity);

            Debug.Log("Atempt to drop # " + currNumOfTries);
            currNumOfTries++;

            if (toDrop)            
                Debug.Log("To drop equals true");            
            else
                Debug.Log("To drop equals false");

            if (notToDrop)
                Debug.Log("Not to drop equals true");
            else
                Debug.Log("Not to drop equals false");
            yield return null;
        }
        while (!toDrop || notToDrop);

        Instantiate(theItemToDrop.itemObject, finalItemPosition, Quaternion.identity);
    }

    /*
    public bool DropItem(ItemScript.ItemTypes itemToGive, Vector3 playerPosition)
    {
        int indexOfItem;
        if(FindItem(itemToGive, out indexOfItem))
        {
            Vector3 randomAddition;
            Vector3 finalItemPosition;
            RaycastHit2D collidersOfItem;

            int numberOfTries = 20;
            int currNumOfTries = 0;
            do
            {
                if (currNumOfTries < numberOfTries)
                {
                    currNumOfTries++;
                }
                else
                {
                    Debug.LogError("There's no space around Player!");
                    return false;
                }
                
                randomAddition = new Vector3(Random.Range(2, 4), Random.Range(1, 4), 0f);
                finalItemPosition = playerPosition + randomAddition;
                collidersOfItem = Physics2D.BoxCast(playerPosition, new Vector2(2, 2), 0f, Vector2.zero, LayerNotToDrop);
            }
            while (!collidersOfItem);

            GameObject droppedItem = Instantiate(Items[indexOfItem].itemObject, finalItemPosition, Quaternion.identity);
            return true;            
        }
        else
        {
            return false;
        }
    }*/

}
