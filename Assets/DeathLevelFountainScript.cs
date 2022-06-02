using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathLevelFountainScript : HandScript
{
    SaveManager SaveManager;
    protected override void Start()
    {
        base.Start();
        SaveManager = GameObject.FindGameObjectWithTag("SaveManager").GetComponent<SaveManager>();
        PlayerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        PlayerManager.Inventory.AddItems(neededItem);

    }

    protected override void SuccessfulInteraction()
    {
        SaveManager.IfEndDeath();
    }
}
