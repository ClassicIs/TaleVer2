using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpikesScript : DangerObject
{
    [SerializeField]
    LayerMask safeLayer;
    [SerializeField]
    LayerMask dangerousLayer;
    void Start()
    {
        LongAction = false;
        StartDamage = new Damage(-1);
    }
    /*
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Player"))
        {
            Debug.Log("Player collision!");
            Vector3 currentPlayerPosition = collision.transform.position;
            Vector2 vector = new Vector2();
            RaycastHit2D safeCheck;
            RaycastHit2D dangerCheck;
            float size = 0.5f;
            for (int i = -1; i < 1; i++)
            {
                for (int j = -1; j < 1; j++)
                {

                    safeCheck = Physics2D.Linecast(currentPlayerPosition, currentPlayerPosition + new Vector3(i, j, 0), safeLayer);
                    if (safeCheck)
                        Debug.Log("Safe is check normal.");
                        Debug.DrawLine(currentPlayerPosition, currentPlayerPosition + new Vector3(i, j, 0), Color.red);
                        //DrawRectangle(new Vector2(currentPlayerPosition.x, currentPlayerPosition.y) + new Vector2(i, j), size, Color.red);
                    dangerCheck = Physics2D.BoxCast(currentPlayerPosition, new Vector2(size, size), 0f, new Vector2(i, j), 1f, dangerousLayer);
                    if (dangerCheck)
                        DrawRectangle(new Vector2(currentPlayerPosition.x, currentPlayerPosition.y) + new Vector2(i, j), size, Color.blue);
                    if (safeCheck && !dangerCheck)
                    {
                        vector = new Vector2(i, j);
                        Debug.LogFormat("Vector is x: {0} y: {1}", vector.x, vector.y);
                        break;
                    }
                }
            }
            collision.GetComponent<Rigidbody2D>().MovePosition(new Vector2(currentPlayerPosition.x, currentPlayerPosition.y) + vector);

        }
    }*/

    private void DrawRectangle(Vector2 centerOfRectangle, float size, Color color)
    {
        //Debug.LogFormat("Drawing rectangle on Position {0}\nWith Size: {1}", centerOfRectangle, size);
        Vector2 pointA = centerOfRectangle - Vector2.up * size / 2 - Vector2.right * size / 2;
        Vector2 pointB = centerOfRectangle - Vector2.down * size / 2 - Vector2.right * size / 2;

        Vector2 pointC = centerOfRectangle - Vector2.up * size / 2 - Vector2.left * size / 2;
        Vector2 pointD = centerOfRectangle - Vector2.down * size / 2 - Vector2.left * size / 2;

        Debug.DrawLine(pointA, pointB, color, 100f);
        Debug.DrawLine(pointB, pointD, color, 100f);
        Debug.DrawLine(pointD, pointC, color, 100f);
        Debug.DrawLine(pointC, pointA, color, 100f);

    }
}
