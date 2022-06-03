using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : AliveBeeing
{
    [SerializeField]
    LayerMask playerLayer;
    
    [SerializeField]
    PathFinding PathFinding;
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
    bool isAttacking = false;
    bool isMoving;

    Transform Player;

    private void Start()
    {
        PathFinding = GetComponent<PathFinding>();
        Player = GameObject.FindGameObjectWithTag("Player").GetComponent<Transform>();
        startWaitTime = 3f;
        waitTime = startWaitTime;
        currState = PlayerStates.moving;
        isMoving = true;
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
                    ToStalk();
                }
                break;
            case PlayerStates.attacking:
                if (isAttacking)
                {
                    isAttacking = true;
                    StartCoroutine(Attack());
                }
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

    void ToStalk()
    {
        StartCoroutine(StalkingCoroutine());
    }

    IEnumerator StalkingCoroutine()
    {
        Vector2 enemyPosition = transform.position;
        Vector2 targetPosition = Player.transform.position;
        IEnumerator thisCoroutine;
        while (true)
        {
            float distanceToTarget = Vector2.Distance(enemyPosition, targetPosition);
            if (distanceToTarget < MAX_DISTANCE)
            {
                if (distanceToTarget < 3f)
                {
                    Debug.Log("Player too far.");
                    isStalking = false;

                    ChangeState(PlayerStates.attacking);
                    yield break;
                }
                else
                {
                    MoveTowardsPlayer(Patrol(PathFinding.FindPath(enemyPosition, targetPosition)));
                    //thisCoroutine = null;
                    //thisCoroutine = PatrolPositions(Patrol(PathFinding.FindPath(enemyPosition, targetPosition)));
                    //StartCoroutine(thisCoroutine);
                }
            }
            else
            {
                Debug.Log("Player too far. Stalking false.");
                StopAllCoroutines();

                isStalking = false;
                ChangeState(PlayerStates.moving);
                yield break;
            }
            yield return new WaitForSeconds(3);
        }
    }

    public void ChangeState(PlayerStates state)
    {
        if(state != currState)
        {
            Debug.LogFormat("Changing to state {0}", state.ToString());
            if(state == PlayerStates.attacking)
            {
                currState = PlayerStates.attacking;
            }

            if (state == PlayerStates.moving)
            {
                currState = PlayerStates.moving;
            }

            if (state == PlayerStates.stalking)
            {
                currState = PlayerStates.stalking;
            }
        }
    }
   
    private Vector2 [] PointsToVector(Transform[] points)
    {        
        Vector2[] vector = new Vector2[points.Length];
        for (int i = 0; i < points.Length; i++)
        {
            Vector3 tmpTransform = points[i].position;
            vector[i] = new Vector2(tmpTransform.x, tmpTransform.y);            
        }
        return vector;
    }

    private void Patrol(Transform[] points)
    {
        Patrol(PointsToVector(points));
    }    

    private void Patrol(Vector2[] positions)
    {       
        StartCoroutine(PatrolPositions(positions));
    }

    private Vector2[] Patrol(Vector3[] positions)
    {
        Vector2 [] newPos = new Vector2[positions.Length];
        int i = 0;
        foreach (Vector3 pos in positions)
        {
            newPos[i] = new Vector2(pos.x, pos.y);
            i++;
        }
        return newPos;
    }

    void MoveTowardsPlayer(Vector2[] positions)
    {
        int i = 0;
        foreach (Vector2 pos in positions)
        {
            float distFromPoint = Vector2.Distance(transform.position, pos);

            while (distFromPoint < 3)
                transform.position = Vector2.MoveTowards(transform.position, pos, speedOfMon * Time.deltaTime);
        }
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
                    if (i == (positions.Length - 1))
                    {
                        i = 0;
                    }
                    else
                    {
                        i++;
                    }
                }

            }
            if (Vector2.Distance(transform.position, Player.position) < 5f)
            {
                Debug.Log("See player");
                break;
            }
            yield return null;
        }
        ChangeState(PlayerStates.stalking);
        isPatroling = false;

    }

    protected IEnumerator Attack()
    {
        while (true)
        {
            Collider2D playerHit = Physics2D.OverlapCircle(transform.position, 10f, playerLayer);
            if (playerHit)
            {
                Debug.Log("Player was hit");
                playerHit.GetComponent<PlayerManager>().AddHealth(-attackStrength);
            }
            if (!(Vector2.Distance(transform.position, Player.position) <= 3))
            {
                ChangeState(PlayerStates.moving);
                break;
            }
            yield return new WaitForSeconds(2);
        }
        isAttacking = false;
    }
    /*
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
    }*/
}
