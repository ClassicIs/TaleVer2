using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LittleThings : MonoBehaviour
{
    SpriteRenderer theSprite;
    [SerializeField]
    float speed;
    [SerializeField]
    float speedToFade;
    [SerializeField]
    Transform positionToGo;
    
    
    private void Start()
    {
        theSprite = GetComponent<SpriteRenderer>();
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag == "Player")
        {
            theSprite.flipX = true;
            
            StartCoroutine(toFadeAway());
        }
    }

    IEnumerator toFadeAway()
    {
        while(theSprite.color.a > 0.1)
        {
            transform.position = Vector2.Lerp(transform.position, positionToGo.position, speed);
            Color tmpCol = new Color(1, 1, 1, theSprite.color.a - speedToFade);
            theSprite.color = tmpCol;
            yield return null;
        }
        
        theSprite.color = new Color(1, 1, 1, 0);
        Debug.Log("Faded away");
        gameObject.SetActive(false);
    }
}
