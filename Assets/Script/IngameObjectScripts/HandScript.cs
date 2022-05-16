using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandScript : InteractObject
{
    public bool isRockHere = false;
    Animator theHandAnim;

    [SerializeField]
    Animator theDoorAnim;
    private void Start()
    {
        theHandAnim = GetComponent<Animator>();
    }

    public override void InterAction()
    {
        OpenTheDoor();
    }

    public override void FutherAction()
    {
        throw new System.NotImplementedException();
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
