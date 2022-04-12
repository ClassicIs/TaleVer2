using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterInput : MonoBehaviour
{
    private float horMovement;
    private float verMovement;
    Player thePlayer;
    Player.PlayerStates theStatesToChoose;

    private void Start()
    {
        thePlayer = GetComponent<Player>();
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
        thePlayer.Move(horMovement, verMovement);
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            thePlayer.ToDash();
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            thePlayer.ToAttack();
        }

    }
}
