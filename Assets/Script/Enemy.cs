using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField]
    protected int attackStrength;
    
    protected bool seesPlayerOrNot;
    [SerializeField]
    protected float speedOfMon;    
    [SerializeField]
    GameObject thePlayer;
    PlayerManager thePlayerMG;

    private void Start()
    {
        thePlayerMG = thePlayer.GetComponent<PlayerManager>();
    }

    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            seesPlayerOrNot = true;
            followPlayer();
        }
    }

    protected virtual void Attack()
    {
        thePlayerMG.ToChangeHealth(-attackStrength);
    }

    protected virtual void followPlayer()
    {
        Vector2 distFromPlayer = thePlayer.gameObject.transform.position - transform.position;
        //transform.position = Vector2.Lerp(new Vector2(transform.position.x, transform.position.y), new Vector2(thePlayer.gameObject.transform.position.x, thePlayer.gameObject.transform.position.y), speedOfMon * Time.deltaTime);
    }

}
