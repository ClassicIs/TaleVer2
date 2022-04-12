using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractObject : MonoBehaviour
{
    [SerializeField]
    PlayerOtherInput PlayerInput;

    protected virtual void Start()
    {
        PlayerInput = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerOtherInput>();
    }

    protected virtual void InterAction()
    {
        Debug.Log("Interaction");
    }   

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerInput.OnInteracting += InterAction;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            PlayerInput.OnInteracting -= InterAction;
        }
    }

}
