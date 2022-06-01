using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeObject : MonoBehaviour
{

    [SerializeField]
    int health = 3;

    [SerializeField]
    UnityEngine.Object destructablePrefab;
    

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if (collision.CompareTag("Player"))
        //{
            if (collision.CompareTag("Weapon"))
            {
                health--;

                if (health <= 0)
                {
                    ExplodeThisObject();
                }
            }

            if (collision.CompareTag("DashHitBox"))
            {
                health = 0;

                if (health <= 0)
                {
                    ExplodeThisObject();
                }
            }
        //}
    }

    private void ExplodeThisObject()
    {
        GameObject destructable = (GameObject)Instantiate(destructablePrefab);
        destructable.transform.position = transform.position;

        Destroy(gameObject);
    }
}
