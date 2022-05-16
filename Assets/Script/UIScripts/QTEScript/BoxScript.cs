using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BoxScript : InteractObject
{
    Animator theBoxAnim;
    public string [] contentOfBox;
    private CircleCollider2D theColliderBox;
    private GameManagerScript GMScript;

    private void Start()
    {
        LongInteraction = false;
        GMScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();        
        theColliderBox = GetComponent<CircleCollider2D>();
        theBoxAnim = GetComponent<Animator>();
    }

    public override void InterAction()
    {
        theColliderBox.enabled = false;
        Debug.Log("The box is openned!");
    }

    public override void FutherAction()
    {
        throw new System.NotImplementedException();
    }
}
