using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GhostScript : MonoBehaviour
{

    [SerializeField]
    private float theDelay;
    private float ghostDelaySeconds;
    [SerializeField]
    private GameObject theGhost;
    public bool makeGhost;
    float ghostTime;
    [SerializeField]
    private float strGhostTime;
    [SerializeField]
    float speedToMove;

    private void Start()
    {        
        //ghostTime = strGhostTime;
        makeGhost = false;
        ghostDelaySeconds = theDelay;        
    }
    private void Update()
    {
        if (makeGhost)
        {
            ghostTime = strGhostTime;
        }
        if (ghostTime > 0)
        {
            if (ghostDelaySeconds > 0f)
            {
                ghostDelaySeconds -= Time.deltaTime;
            }
            else
            {
                GameObject currGhost = Instantiate(theGhost, transform.position, transform.rotation);
                Sprite currSprite = GetComponent<SpriteRenderer>().sprite;
                currGhost.GetComponent<SpriteRenderer>().sprite = currSprite;
                ghostDelaySeconds = theDelay;
                //Destroy(currGhost, 1f);
            }
            ghostTime -= Time.deltaTime;
        }
    }

    public void makeTheGhosts(Vector2 currPos, Vector2 NeedPosition)
    {
        /*if (ghostDelaySeconds > 0f)
        {
            ghostDelaySeconds -= Time.deltaTime;
        }
        else
        {
            GameObject currGhost = Instantiate(theGhost, Vector2.Lerp(currPos, NeedPosition, Time.deltaTime), transform.rotation);
            Sprite currSprite = GetComponent<SpriteRenderer>().sprite;
            currGhost.GetComponent<SpriteRenderer>().sprite = currSprite;
            ghostDelaySeconds = theDelay;
            //Destroy(currGhost, 1f);
        }*/
        StartCoroutine(theGhostTrail(currPos, NeedPosition));
    }

    IEnumerator theGhostTrail(Vector2 currPos, Vector2 NeedPosition)
    {
        
        Vector2 thePositionToMove = currPos;
        while (ghostDelaySeconds > 0f)
        {
            thePositionToMove = Vector2.Lerp(thePositionToMove, NeedPosition, Time.deltaTime * speedToMove);
            GameObject currGhost = Instantiate(theGhost, thePositionToMove, transform.rotation);
            Sprite currSprite = GetComponent<SpriteRenderer>().sprite;
            currGhost.GetComponent<SpriteRenderer>().sprite = currSprite;
            ghostDelaySeconds -= Time.deltaTime;
            Destroy(currGhost, 1f);
            yield return new WaitForSeconds(.2f);
        }
        ghostDelaySeconds = theDelay;
    }

    
    public void MakeTheGhost()
    {
        GameObject currGhost = Instantiate(theGhost, transform.position, transform.rotation);
        Sprite currSprite = GetComponent<SpriteRenderer>().sprite;
        currGhost.GetComponent<SpriteRenderer>().sprite = currSprite;
        ghostDelaySeconds = theDelay;
        Destroy(currGhost, 1f);
    }

}
