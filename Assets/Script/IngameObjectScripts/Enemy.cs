using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : AliveBeeing
{
    [SerializeField]
    PathFinding theFinder;
    [SerializeField]
    protected int attackStrength;
    protected bool seesPlayerOrNot;
    [SerializeField]
    protected float speedOfMon;
    bool isStalking = false;
    private float waitTime;
    private float startWaitTime;

    GameObject PlayerTarget;
    private const int MAX_DISTANCE = 20;
    private IEnumerator FollowCoroutine;
    
    [SerializeField]
    private Transform[] pointsToMove;
    bool isPatroling = false;

    private void Start()
    {
        startWaitTime = 3f;
        waitTime = startWaitTime;
        currState = PlayerStates.moving;
    }

    private void Update()
    {
        switch(currState)
        {
            case PlayerStates.moving:
                if (!isPatroling)
                {
                    isPatroling = true;
                    Patrol(pointsToMove);
                }
                break;
            case PlayerStates.stalking:
                if(!isStalking)
                {
                    isStalking = true;
                    ToStalk(PlayerTarget);
                }
                break;
            case PlayerStates.attacking:
                Attack();
                break;
            case PlayerStates.stunned:
                Debug.LogFormat("Enemy {0} is stunned!", transform.name);
                break;
            case PlayerStates.isDead:
                Debug.LogFormat("Enemy {0} is dead!", transform.name);
                break;
            default:
                //Patrol();
                break;
        }
    }

    void ToStalk(GameObject target)
    {

    }

    IEnumerator FindCoroutine(Vector3 target)
    {
        yield break;
        while (true)
        {
            /*List<Vector3> points = PathFinding.FindPath(transform.position, target);
            if (points != null)
            {
                for (int i = 0; i < points.Count; i++)
                {
                    Debug.LogFormat("Point {0} is {1}", i, points[i]);
                }
            DrawLineByPoint(points);
            }*/
            yield return null;
        }
    }

    private Vector2 [] PointsToVector(Transform[] points)
    {        
        Vector2[] vector = new Vector2[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            Vector3 tmpTransform = points[i].position;
            vector[i] = new Vector2(tmpTransform.x, tmpTransform.y);
            //Debug.LogFormat("Current i is = {0}", i);
            //Debug.LogFormat("Vector original = {0} \nVector transformed = {1}", tmpTransform, vector[i]);
        }
        return vector;
    }

    private void Patrol(Transform[] points)
    {
        Patrol(PointsToVector(points));
    }    

    private void Patrol(Vector2[] positions)
    {
        Debug.Log("Starting Coroutine");

        StartCoroutine(PatrolPositions(positions));
    }

    IEnumerator PatrolPositions(Vector2[] positions)
    {
        int i = 0;

        while (true)
        {
            float distFromPoint = Vector2.Distance(transform.position, positions[i]);
            if (distFromPoint > 0.3f)
            {
                transform.position = Vector2.MoveTowards(transform.position, positions[i], speedOfMon * Time.deltaTime);
            }
            else
            {
                if (waitTime > 0)
                {
                    waitTime -= Time.deltaTime;
                }
                else
                {
                    waitTime = startWaitTime;
                    //Debug.LogFormat("i = {0} \nPoints length = {1} ", i, pointsToMove.Length-1);
                    if (i == (pointsToMove.Length - 1))
                    {
                        i = 0;
                    }
                    else
                    {
                        i++;
                    }
                }

            }
            yield return null;
        }
    }


    protected virtual void OnTriggerEnter2D(Collider2D col)
    {
        if(col.CompareTag("Player"))
        {
            seesPlayerOrNot = true;
            Follow(PlayerTarget);
            
        }
    }

    protected virtual void OnTriggerExit2D(Collider2D col)
    {
        if (col.CompareTag("Player"))
        {
            seesPlayerOrNot = false;
        }
    }

    protected virtual void Attack()
    {
        //thePlayerMG.AddHealth(-attackStrength);
    }

    protected virtual void Follow(GameObject target)
    {
        FollowCoroutine = ToFollow(target.transform);
        StartCoroutine(FollowCoroutine);
    }

    private IEnumerator ToFollow(Transform target)
    {
        bool playerFar = false;
        float distFromPlayer;
        do
        {            
            distFromPlayer = Vector3.Distance(transform.position, target.position);
            if (distFromPlayer >= MAX_DISTANCE)
            {
                playerFar = true;
                Debug.Log("Player too far!");
                break;
            }
            transform.position = Vector2.Lerp(transform.position, target.position, speedOfMon * Time.deltaTime);
            yield return null;
        }
        while (distFromPlayer > 3f);
        if(playerFar)
        {
            Debug.LogFormat("Player is too far. {0}", (target.position));
        }
        else
        {
            Debug.Log("Player is here!");
        }
    }
}
