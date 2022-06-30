using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExplodeObject : MonoBehaviour, IDameagable
{
    [SerializeField]
    int health = 3;


    [SerializeField]
    UnityEngine.Object destructablePrefab;

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("DashHitBox"))
        {
            health = 0;
            ExplodeThisObject();

        }    
    }

    public void TakeDamage(int damage)
    {
        health -= damage;
        if(health <= 0)
        {
            ExplodeThisObject();
        }
        else
        {
            if (!alreadyRed)
            {
                redCoroutine = RedAfterDamage(GetComponent<SpriteRenderer>(), Color.red, 0.4f);
                StartCoroutine(redCoroutine);
                alreadyRed = true;
            }
        }
    }
    private IEnumerator redCoroutine;
    bool alreadyRed = false;
    private IEnumerator RedAfterDamage(SpriteRenderer spriteRenderer, Color colorToLerp, float timeToWait)
    {
        Color tmpColor = spriteRenderer.color;
        spriteRenderer.color = colorToLerp;
        yield return new WaitForSeconds(timeToWait);
        Debug.Log("End of red process");
        spriteRenderer.color = tmpColor;
        alreadyRed = false;
    }
    
    public void ExplodeThisObject()
    {
        GameObject destructable = (GameObject)Instantiate(destructablePrefab);
        destructable.transform.position = transform.position;

        Destroy(gameObject);
    }
}
