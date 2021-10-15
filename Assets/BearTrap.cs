using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class BearTrap : MonoBehaviour
{
    bool PlayerStay;
    bool firstStep = true;

    [SerializeField]
    isQtePassed qte;

    //[SerializeField]
    //Player PlayerScript;

    // Start is called before the first frame update
    void Start()
    {

    }

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerStay = true;
        }
    }

    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            PlayerStay = false;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (PlayerStay == true && firstStep == true)
        {
            Debug.Log("You are in trap, silly boy!");

            qte.EventPassed += Subscribe;
            qte.qteActive = true;
            firstStep = false;
            GetComponent<Animator>().SetBool("isPlayerCaught", true);
            qte.QTESuccess();
        }
    }

    void Subscribe(object sender, EventArgs e)
    {
        qte.EventPassed -= Subscribe;
    }
}


