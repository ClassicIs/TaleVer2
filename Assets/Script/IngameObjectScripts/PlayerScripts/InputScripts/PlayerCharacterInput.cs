using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerCharacterInput : MonoBehaviour
{
    public float horMovement;
    public float verMovement;
    Player Player;
    public Action OnInventoryCall;
    private void Start()
    {
        Player = GetComponent<Player>();
    }
    // Update is called once per frame
    void Update()
    {
        GetInput();
    }
    private void GetInput()
    {
        horMovement = Input.GetAxisRaw("Horizontal");
        verMovement = Input.GetAxisRaw("Vertical");
        Player.horMovement = this.horMovement;
        Player.vertMovement = verMovement;

        if (horMovement != 0 || verMovement != 0)
        {
            // For the case
            if (Player.CurrentState() != Player.PlayerStates.stunned)
            {
                Player.ChangeState(Player.PlayerStates.moving);
            }
        }

        if (Input.GetKeyDown(KeyCode.Space))
        {
            Player.ChangeState(Player.PlayerStates.dashing);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            Player.ChangeState(Player.PlayerStates.attacking);
        }
        
        if (Input.GetKeyDown(KeyCode.I))
        {
            if (OnInventoryCall != null)
            {
                OnInventoryCall();
            }           
        }
    }
}
