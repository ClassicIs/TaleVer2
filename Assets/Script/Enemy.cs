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
    Transform theEnemy;

    protected void Start()
    {
        theEnemy = GetComponent<Transform>();
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
        //theEnemy.
        /*Vector2 distFromPlayer = col.transform.position - theEnemy.position;
        Vector2 dirToPlayer = distFromPlayer.normalized;
        Vector2 speedToPlayer = dirToPlayer * speedOfMon;
        theEnemy.Translate(speedToPlayer);
        */
        theEnemy.position = Vector2.Lerp(theEnemy.position, thePlayer.transform.position, speedOfMon * Time.deltaTime);
    }

}
