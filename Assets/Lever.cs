using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Lever : MonoBehaviour
{
    bool PlayerStay;

    public GameObject Door;
    Animator DoorAnim;

    [SerializeField]
    isQtePassed qte;

    // Start is called before the first frame update
    void Start()
    {
        DoorAnim = Door.GetComponent<Animator>();
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
        if (PlayerStay == true)
        {
            if (Input.GetKeyDown("e"))
            {
                Debug.Log("E is pressed");
                qte.qteActive = true;
                qte.QTESuccess();
            }
        }
    }

    public void OpenDoor()
    {
        DoorAnim.SetBool("DoorIsOpen", true);
        GetComponent<Animator>().SetBool("QTEisPassed", true);
        GetComponent<Lever>().enabled = false;
    }
}
