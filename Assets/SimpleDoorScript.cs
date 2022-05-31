using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SimpleDoorScript : DoorScript
{
    private Animator doorAnimator;
    private BoxCollider2D doorCollider;

    void Awake()
    {
        doorAnimator = GetComponent<Animator>();
        doorCollider = GetComponent<BoxCollider2D>();
    }

    public override void Close()
    {
        doorAnimator.SetBool("DoorIsOpen", false);
        doorCollider.enabled = true;
    }

    public override void Open()
    {
        doorAnimator.SetBool("DoorIsOpen", true);
        doorCollider.enabled = false;
    }
}
