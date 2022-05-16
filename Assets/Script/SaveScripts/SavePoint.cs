using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SavePoint : MonoBehaviour
{
    int CurrHealth;
    int CurrInkLevel;
    int CoinCount;
    Vector2 PositionOfPlayer;
    List<string> CurrentInventory;

    SavePoint(int TheHealth, int TheInkLevel, Vector2 ThePositionOfPlayer, List<string> CurrentInventory)
    {
        CurrHealth = TheHealth;
        CurrInkLevel = TheInkLevel;
        PositionOfPlayer = ThePositionOfPlayer;
        this.CurrentInventory = CurrentInventory;
    }

    public void SetCheckPoint(int CurrHealth, int CurrInkLevel, int CoinCount, Vector2 PositionOfPlayer, List<string> CurrentInventory)
    {
        this.CurrHealth = CurrHealth;
        this.CurrInkLevel = CurrInkLevel;
        this.PositionOfPlayer = PositionOfPlayer;
        this.CurrentInventory = CurrentInventory;
    }

    public void ReturnPoint(out int CurrHealth, out int CurrInkLevel, out int CoinCount, out Vector2 PositionOfPlayer, out List <string> CurrentInventory)
    {
        CurrHealth = this.CurrHealth;
        CurrInkLevel = this.CurrInkLevel;
        CoinCount = this.CoinCount;
        PositionOfPlayer = this.PositionOfPlayer;
        CurrentInventory = this.CurrentInventory;
    }

    public void PrintCheckPoint()
    {
        Debug.Log("Health = " + CurrHealth + "\nInk Level " + CurrInkLevel + "\nCoin count " + CoinCount + "\nPlayer position " + PositionOfPlayer.x + " " + PositionOfPlayer.y);
    }
}
