using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
public class UIInventorySlot : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [Header("References")]
    [SerializeField]
    public Button cancelButton;
    [SerializeField]
    public Button itemButton;
    [SerializeField]
    public Image thisItemSprite;

    private bool toReadHover = true;

    public int btnIndex;
    public ItemScript thisButtonItemScript;

    //Actions 
    public event Action<string> OnPointerHover;
    public event Action OnPointerExit;

    public event Action<ItemScript, int> OnDropItem;
    public event Action<ItemScript, int> OnUseItem;

    public void SetButton(ItemScript buttonItem, int buttonIndex)
    {
        thisButtonItemScript = buttonItem;
        btnIndex = buttonIndex;
        PutSprite(buttonItem.itemSprite);
        itemButton.onClick.AddListener(delegate
        {
            if (OnUseItem != null)
            {
                OnUseItem(thisButtonItemScript, btnIndex);
            }
            
        });

        cancelButton.onClick.AddListener(delegate
        {
            if (OnDropItem != null)
            {
                OnDropItem(thisButtonItemScript, btnIndex);
            }
        });

    }

    public void ActivateButton(bool onOrOff)
    {
        itemButton.enabled = onOrOff;
        cancelButton.enabled = onOrOff;
        toReadHover = onOrOff;
    }

    private void PutSprite (Sprite thisButtonSprite)
    {
        thisItemSprite.sprite = thisButtonSprite;
    }

    void IPointerEnterHandler.OnPointerEnter(PointerEventData eventData)
    {
        if (toReadHover)
        {
            if (OnPointerHover != null)
            {
                OnPointerHover(thisButtonItemScript.itemDescription);
            }
        }
    }

    void IPointerExitHandler.OnPointerExit(PointerEventData eventData)
    {
        if (toReadHover)
        {
            if (OnPointerExit != null)
            {
                OnPointerExit();
            }
        }
    }
}
