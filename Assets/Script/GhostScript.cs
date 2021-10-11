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

    float timeToNextGhost = 0.1f;
    float ghostCount = 0;
    float strGhostCount = 8;

    private void Start()
    {        
        //ghostTime = strGhostTime;
        
    }

    private void OnEnable()
    {
        makeGhost = true;
        ghostCount = strGhostCount;
        makeGhosts();
    }

    private void OnDisable()
    {
        makeGhost = false;
        ghostCount = strGhostCount;
    }

    public void makeGhosts()
    {
        StartCoroutine(toMakeGhosts());
    }

    IEnumerator toMakeGhosts()
    {
        while(ghostCount > 0)
        {
            GameObject currGhost = Instantiate(theGhost, transform.position, transform.rotation);
            currGhost.GetComponent<SpriteRenderer>().sprite = GetComponent<SpriteRenderer>().sprite;

            ghostCount -= 1;
            yield return new WaitForSeconds(timeToNextGhost);
        }

    }

    //private void Update()
    //{
    //    if (makeGhost)
    //    {
    //        ghostTime = strGhostTime;
    //    }
    //    /*if (ghostTime > 0)
    //    {
    //        if (ghostDelaySeconds > 0f)
    //        {
    //            ghostDelaySeconds -= 0.2f * Time.deltaTime;
    //        }
    //        else
    //        {
    //            GameObject currGhost = Instantiate(theGhost, transform.position, transform.rotation);
    //            Sprite currSprite = GetComponent<SpriteRenderer>().sprite;
    //            currGhost.GetComponent<SpriteRenderer>().sprite = currSprite;
    //            ghostDelaySeconds = theDelay;
    //            //Destroy(currGhost, 1f);
    //        }
    //        ghostTime -= Time.deltaTime;
    //    }
    //}

    //public void makeTheGhosts(Vector2 currPos, Vector2 NeedPosition)
    //{
    //    /*if (ghostDelaySeconds > 0f)
    //    {
    //        ghostDelaySeconds -= Time.deltaTime;
    //    }
    //    else
    //    {
    //        GameObject currGhost = Instantiate(theGhost, Vector2.Lerp(currPos, NeedPosition, Time.deltaTime), transform.rotation);
    //        Sprite currSprite = GetComponent<SpriteRenderer>().sprite;
    //        currGhost.GetComponent<SpriteRenderer>().sprite = currSprite;
    //        ghostDelaySeconds = theDelay;
    //        //Destroy(currGhost, 1f);
    //    }*/
    //    StartCoroutine(theGhostTrail(currPos, NeedPosition));
    //}

    //IEnumerator theGhostTrail(Vector2 currPos, Vector2 NeedPosition)
    //{
        
    //    Vector2 thePositionToMove = currPos;
    //    while (ghostDelaySeconds > 0f)
    //    {
    //        thePositionToMove = Vector2.Lerp(thePositionToMove, NeedPosition, Time.deltaTime * speedToMove);
    //        GameObject currGhost = Instantiate(theGhost, thePositionToMove, transform.rotation);
    //        Sprite currSprite = GetComponent<SpriteRenderer>().sprite;
    //        currGhost.GetComponent<SpriteRenderer>().sprite = currSprite;
    //        ghostDelaySeconds -= Time.deltaTime;
    //        Destroy(currGhost, 1f);
    //        yield return new WaitForSeconds(.2f);
    //    }
    //    ghostDelaySeconds = theDelay;
    //}

    
    //public void MakeTheGhost()
    //{
    //    GameObject currGhost = Instantiate(theGhost, transform.position, transform.rotation);
    //    Sprite currSprite = GetComponent<SpriteRenderer>().sprite;
    //    currGhost.GetComponent<SpriteRenderer>().sprite = currSprite;
    //    ghostDelaySeconds = theDelay;
    //    Destroy(currGhost, 1f);
    //}

}
