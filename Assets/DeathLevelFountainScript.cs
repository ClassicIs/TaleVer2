using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeathLevelFountainScript : HandScript
{
    SaveManager SaveManager;
    Animator DLFountainAnim;
    protected override void Start()
    {
        base.Start();
        SaveManager = GameObject.FindGameObjectWithTag("SaveManager").GetComponent<SaveManager>();
        PlayerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();

        DLFountainAnim = GetComponent<Animator>();
        PlayerManager.Inventory.AddItem(neededItem[0]);

    }

    protected override void SuccessfulInteraction()
    {
        DLFountainAnim.SetTrigger("Activate");
        StartCoroutine(WaitFor(2.5f));
        
    }

    IEnumerator WaitFor(float howMany)
    {
        yield return new WaitForSeconds(howMany);
        SaveManager.BackToMainLevel();
    }
}
