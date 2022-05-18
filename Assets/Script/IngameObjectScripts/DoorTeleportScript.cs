using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorTeleportScript : MonoBehaviour
{
    [SerializeField] Transform positionToGo;
    [SerializeField] GameObject keyText;

    bool playerDetected;
    GameObject playerGo;

    // Start is called before the first frame update
    void Start()
    {
        playerDetected = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (playerDetected)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                Invoke("ToGoFunction", 1);
                playerDetected = false;
            }
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDetected = true;
            playerGo = collision.gameObject;
            keyText.SetActive(true);
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            playerDetected = false;
            keyText.SetActive(false);
        }
    }

    private void ToGoFunction()
    {
        playerGo.transform.position = positionToGo.position;
    }
}
