using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InkDrop : MonoBehaviour
{
    public Transform transformInk;
    public float toBig;
    // Start is called before the first frame update
    void Start()
    {
        transformInk = GetComponent<Transform>();
        toBig = 0f;
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            toBig = .0005f;
        }
    }
    /*private void OnTriggerEnter2D(Collider2D collision)
    {
        
    }*/
    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            toBig = 0f;
        }
    }

    private void FixedUpdate()
    {
        if ((transformInk.localScale.x < 4) && (transformInk.localScale.y < 4))
        {
            transformInk.localScale = new Vector3(transformInk.localScale.x + toBig, transformInk.localScale.y + toBig, transformInk.localScale.z);
        }       
    }

    // Update is called once per frame
    void Update()
    {
         
    }
}
