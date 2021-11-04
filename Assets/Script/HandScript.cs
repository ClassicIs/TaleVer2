using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : InteractObject
{
    public bool isRockHere = false;
    Animator theHandAnim;

    [SerializeField]
    Animator theDoorAnim;
    protected override void Start()
    {
        base.Start();
        theHandAnim = GetComponent<Animator>();
    }

    protected override void InterAction()
    {
        base.InterAction();
        OpenTheDoor();
    }

    private void OpenTheDoor()
    { 
        if(isRockHere)
        {
            theHandAnim.SetTrigger("ToGetOut");
        }
    }

    public void DoorOpen()
    {
        theDoorAnim.SetBool("DoorIsOpen", true);
        gameObject.SetActive(false);
    }

}
