using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLDoorsController : MonoBehaviour
{
    /*
    [SerializeField] 
    private DLDoor topDoor;

    [SerializeField]
    private DLDoor leftDoor;

    [SerializeField]
    private DLDoor bottomDoor;

    [SerializeField]
    private DLDoor rightDoor;
    */

    [SerializeField]
    private DLDoor[] doors;
    

    [SerializeField]
    private BattleSystem battleSystem;


    // Start is called before the first frame update
    void Start()
    {
      
        battleSystem.OnBattleStarted += BattleSystem_OnBattleStarted;
        battleSystem.OnBattleOver += BattleSystem_OnBattleOver;

    }


    private void BattleSystem_OnBattleStarted(object sender, EventArgs e)
    {
        /*
        topDoor.CloseDoor();
        leftDoor.CloseDoor();
        bottomDoor.CloseDoor();
        rightDoor.CloseDoor();
        */

        foreach(var door in doors)
        {
            door.CloseDoor();
        }
    }


    private void BattleSystem_OnBattleOver(object sender, EventArgs e)
    {
        /*
        topDoor.OpenDoor();
        leftDoor.OpenDoor();
        bottomDoor.OpenDoor();
        rightDoor.OpenDoor();
        */

        foreach (var door in doors)
        {
            door.OpenDoor();
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
