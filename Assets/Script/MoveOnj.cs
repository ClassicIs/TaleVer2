using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnj : MonoBehaviour
{
    [SerializeField]
    Transform[] pointsToVisit;
    [SerializeField]
    float speedToGo = 4f;
    Transform theObj;
    int i = 0;

    protected void Start()
    {
        theObj = GetComponent<Transform>();
    }

    protected void Update()
    {
        ToMoveTheObj();
    }

    protected void ToMoveTheObj()
    {        
        while (true)
        {
            while (Vector2.Distance(theObj.position, pointsToVisit[i].position) > 0.1f)
            {
                theObj.position = Vector2.MoveTowards(theObj.position, pointsToVisit[i].position, speedToGo);
            }
            Debug.Log("pointsToVisit.Length" + pointsToVisit.Length);
            if (i == (pointsToVisit.Length - 1))
            {
                Debug.Log("We are here" + pointsToVisit.Length);
                i = 0;
            }
            else
            {
                i++;
            }
        }
        
    }
}
