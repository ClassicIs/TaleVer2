using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ToMoveCharacter : MonoBehaviour
{
    Transform theTransformOne;
    public Transform startPos;

    private void Start()
    {
        theTransformOne = GetComponent<Transform>();
        StartCoroutine(WaitTilDist(startPos.position));

    }

    private void Update()
    {
        
    }


    private IEnumerator WaitTilDist(Vector3 strPos)
    {
        
        //bool isItThere = Vector2.Distance(theTransformOne.transform.position, strPos) <= 0.5f;
        //Debug.Log("isItThere: " + (Vector2.Distance(theTransformOne.position, strPos) <= 0.5f));
        while (!(Vector2.Distance(theTransformOne.transform.position, strPos) <= 0.5f))
        {
            //Debug.Log("isItThere: " + (Vector2.Distance(theTransformOne.position, strPos) <= 0.5f));
            theTransformOne.transform.position = Vector2.Lerp(theTransformOne.position, strPos, Time.deltaTime);
            Debug.Log("Distance is: " + Vector2.Distance(theTransformOne.position, strPos));
            yield return null;
        }

        Debug.Log("Is on position!");        
    }
}
