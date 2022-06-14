using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint
{
    int CurrHealth;
    int maxHealth;
    int CurrInkLevel;
    int maxInkLevel;
    int CoinCount;
    Vector2 PositionOfPlayer;
    InventoryScript CurrentInventory;

    public SavePoint(int TheHealth, int TheInkLevel, int coinCount, Vector2 ThePositionOfPlayer, InventoryScript CurrentInventory, int maxHealth = 4, int maxInkLevel = 100)
    {
        CurrHealth = TheHealth;
        CoinCount = coinCount;
        CurrInkLevel = TheInkLevel;
        PositionOfPlayer = ThePositionOfPlayer;
        this.CurrentInventory = CurrentInventory;
        this.maxHealth = maxHealth;
        this.maxInkLevel = maxInkLevel;
    }

    public void SetCheckPoint(int CurrHealth, int CurrInkLevel, int CoinCount, Vector2 PositionOfPlayer, InventoryScript CurrentInventory, int maxHealth = 4, int maxInkLevel = 100)
    {
        this.CurrHealth = CurrHealth;
        this.CurrInkLevel = CurrInkLevel;
        this.PositionOfPlayer = PositionOfPlayer;
        this.CurrentInventory = CurrentInventory;
        this.maxHealth = maxHealth;
        this.maxInkLevel = maxInkLevel;
    }

    public void ReturnPoint(out int CurrHealth, out int CurrInkLevel, out int CoinCount, out Vector2 PositionOfPlayer, out InventoryScript CurrentInventory, out int maxHealth, out int maxInkLevel)
    {
        CurrHealth = this.CurrHealth;
        CurrInkLevel = this.CurrInkLevel;
        maxHealth = this.maxHealth;
        maxInkLevel = this.maxInkLevel;
        CoinCount = this.CoinCount;
        PositionOfPlayer = this.PositionOfPlayer;
        CurrentInventory = this.CurrentInventory;
    }

    public void PrintCheckPoint()
    {
        Debug.Log("Health = " + CurrHealth + "\nInk Level " + CurrInkLevel + "\nCoin count " + CoinCount + "\nPlayer position " + PositionOfPlayer.x + " " + PositionOfPlayer.y);
    }
}
