using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLHatchOnBattleController : MonoBehaviour
{
    [SerializeField]
    private DLHatch hatch;

    [SerializeField]
    private Gem redGem;

    [SerializeField]
    private BattleSystem battleSystem;


    // Start is called before the first frame update
    private void Start()
    {
        //battleSystem.OnBattleStarted += BattleSystem_OnBattleStarted;
        battleSystem.OnBattleOver += BattleSystem_OnBattleOver;
    }


    /*
    private void BattleSystem_OnBattleStarted(object sender, EventArgs e)
    {
       
    }
    */

    private void BattleSystem_OnBattleOver(object sender, EventArgs e)
    {
        hatch.OpenHatch();
        redGem.SetActive();
    }

}
