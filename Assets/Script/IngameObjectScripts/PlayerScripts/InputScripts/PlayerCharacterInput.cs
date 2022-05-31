using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacterInput : MonoBehaviour
{
    public float horMovement;
    public float verMovement;
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
        thePlayer.horMovement = this.horMovement;
        thePlayer.vertMovement = verMovement;

        //thePlayer.Move(horMovement, verMovement);
        thePlayer.ChangeState(Player.PlayerStates.moving);

        if (Input.GetKeyDown(KeyCode.Space))
        {
            thePlayer.ChangeState(Player.PlayerStates.dashing);
        }

        if (Input.GetKeyDown(KeyCode.G))
        {
            thePlayer.ChangeState(Player.PlayerStates.attacking);
        }

    }
}
