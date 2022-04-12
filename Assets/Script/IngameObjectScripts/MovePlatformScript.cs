using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovePlatformScript : MonoBehaviour
{
    [SerializeField]
    private Transform posOne;
    [SerializeField]
    private Transform posTwo;
    [SerializeField]
    private Vector3 nextPos;
    [SerializeField] 
    private float waitTime;
    [SerializeField]
    private float currWaitTime;

    private float platSpeed;
    private Transform thePlatform;
 
    // Start is called before the first frame update
    void Start()
    {
        thePlatform = GetComponent<Transform>();        
        nextPos = posTwo.position;
        platSpeed = 1f;
        waitTime = 10f;
        currWaitTime = waitTime;
    }    

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Debug.Log("Playyer!");
            collision.gameObject.transform.parent = thePlatform;
                
                //position += thePlatform.position;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Player"))
        {
            //Debug.Log("Playyer out!");
            collision.gameObject.transform.parent = null;
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (thePlatform.position == posOne.position)
        {
            //Debug.Log("PosOne!");
            if (currWaitTime > 0f)
            {
                currWaitTime -= Time.deltaTime;
            }
            else
            {                
                nextPos = posTwo.position;
                currWaitTime = waitTime;
            }
        }
        else if(thePlatform.position == posTwo.position)
        {
            //Debug.Log("PosTwo!");
            if (currWaitTime > 0f)
            {
                currWaitTime -= Time.deltaTime;
            }
            else
            {
                nextPos = posOne.position;
                currWaitTime = waitTime;
            }
        }

        thePlatform.position = Vector3.MoveTowards(thePlatform.position, nextPos, platSpeed * Time.deltaTime);
    }
    /*
    private void NextPosition(Vector3 thePosition)
    {
        
    }*/
}
