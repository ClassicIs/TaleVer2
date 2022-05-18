using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DLPlayerChangePosition : MonoBehaviour
{
    [SerializeField]
    private Vector3 playerPos;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            collision.transform.position += playerPos;
        }
    }


}
