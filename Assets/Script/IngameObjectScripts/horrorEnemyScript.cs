using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class horrorEnemyScript : Enemy
{
    
    Animator theAnimOfEnemy;

    // Start is called before the first frame update
    void Start()
    {
        enemySpeed = 0.2f;
        attackStrength = 1;
        theAnimOfEnemy = GetComponent<Animator>();

    }
    
    
}
