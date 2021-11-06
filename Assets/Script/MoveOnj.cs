using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveOnj : MonoBehaviour
{
    [SerializeField]
    Transform[] pointsToVisit;
    [SerializeField]
    float speedToGo = 4f;
    int i = 0;

    private void Start()
    {
        
    }

    private void Update()
    {
        ToMoveTheObj();
    }

    private void ToMoveTheObj()
    {        
        while (true)
        {
            Debug.Log("The vector distance is " + Vector2.Distance(transform.position, pointsToVisit[i].position));
            while (Vector2.Distance(transform.position, pointsToVisit[i].position) > 0.1f)
            {
                transform.position = Vector2.MoveTowards(transform.position, pointsToVisit[i].position, speedToGo);
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
