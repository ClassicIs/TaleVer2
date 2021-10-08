using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class scriptForL : MonoBehaviour
{
    public Animator anim1;
    int timer;
    int startTimer;
    private void Start()
    {
        anim1 = GetComponent<Animator>();
        startTimer = 100;
        timer = startTimer;
    }
    private void Update()
    {
        
    }
    private void FixedUpdate()
    {
        
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        
        if(collision.gameObject.tag == "Player")
        {
            anim1.SetBool("isPlayer", true);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            if (timer > 0)
            {
                timer -= 1;
            }
            else
            {
                //Spawn
                //Instantiate
            }
            //anim1.SetBool("isPlayer", true);
        }
    }
    private void OnTriggerExit2D(Collider2D collision)
    {

        if (collision.gameObject.tag == "Player")
        {
            anim1.SetBool("isPlayer", false);
        }
    }
}
