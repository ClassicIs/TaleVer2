using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class Lever : InteractObject 
{
    public GameObject Door;
    Animator DoorAnim;

    [SerializeField]
    isQtePassed qte;

    void Subscribe(object sender, EventArgs e)
    {
        OpenDoor();
    }

    // Start is called before the first frame update
    protected override void Start()
    {
        base.Start();
        DoorAnim = Door.GetComponent<Animator>();
    }

    /*void OnTriggerEnter2D(Collider2D collision) 
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
    */
    // Update is called once per frame
    /*void Update()
    {
        if (PlayerStay == true)
        {
            if (Input.GetKeyDown("e"))
            {
                qte.EventPassed += Subscribe;
                Debug.Log("E is pressed");
                qte.qteActive = true;
                qte.QTESuccess();
            }
        }
    }*/

    protected override void InterAction()
    {
        base.InterAction();
        qte.EventPassed += Subscribe;
        Debug.Log("E is pressed");
        qte.qteActive = true;
        qte.QTESuccess();
    }

    private void OpenDoor()
    {
        DoorAnim.SetBool("DoorIsOpen", true);
        GetComponent<Animator>().SetBool("QTEisPassed", true);
        qte.EventPassed -= Subscribe;
        GetComponent<Lever>().enabled = false;        
    }
}
