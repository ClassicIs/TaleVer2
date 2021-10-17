using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour
{
    [SerializeField]
    Player thePlayer;

    protected virtual void Start()
    {
        thePlayer = GameObject.FindGameObjectWithTag("Player").GetComponent<Player>();
    }

    protected virtual void InterAction()
    {
        Debug.Log("Interaction");
    }   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            thePlayer.OnInteracting += InterAction;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            thePlayer.OnInteracting -= InterAction;
        }
    }

}
