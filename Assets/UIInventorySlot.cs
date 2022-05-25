using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIInventorySlot : MonoBehaviour
{
    public Button cancelButton;
    public Button itemButton;
    public Image thisItemSprite;
  
    void Start()
    {
        cancelButton = GetComponentsInChildren<Button>()[0];
        itemButton = GetComponentsInChildren<Button>()[1];
    }

    public void PutSprite (Sprite thisButtonSprite)
    {
        thisItemSprite.sprite = thisButtonSprite;
    }

    public void NoneSprite()
    {
        thisItemSprite.sprite = null;
    }

}
