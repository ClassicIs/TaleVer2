using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Enemy : AliveBeeing
{
    [SerializeField]
    float health;
    [SerializeField]
    float maxHealth;
    
    [SerializeField]
    LayerMask playerLayer;
    [SerializeField]
    Damage enemyDamage;
    [SerializeField]
    private float radiusOfSight;
    [SerializeField]
    private float toNextAttackTime;

    [SerializeField]
    PathFinding PathFinding;
    [SerializeField]
    protected int attackStrength;
    protected bool seesPlayerOrNot;
    [SerializeField]
    protected float enemySpeed;
    bool isStalking = false;
    private float waitTime;
    private float startWaitTime;

    private const float MAX_DISTANCE = 40;
    private const float MIN_DISTANCE = 1;
    private IEnumerator FollowCoroutine;
    
    [SerializeField]
    private Transform[] pointsToMove;
    bool isPatroling = false;
    bool isAttacking = false;
    bool isMoving;

    public Action OnDeath;

    Vector2 enemyDirection;

    [SerializeField]
    Transform target;

    
    
    private void Start()
    {
        health = maxHealth;
        enemyDirection = new Vector2(0, -1);
        startWaitTime = 3f;
        waitTime = startWaitTime;
        ChangeState(PlayerStates.moving);
        isMoving = true;
        target = GameObject.FindObjectOfType<Player>().transform;
        PathFinding = GameObject.FindObjectOfType<PathFinding>();
    }


    public void TakeDamage(float damage)
    {
        Debug.LogErrorFormat("Damage taken {0}", damage);
        health -= damage;
        if(health <= 0)
        {
            Death();
        }
    }

    private void Death()
    {
        //Animator dead
        Debug.LogFormat("Enemy {0} is dead", gameObject.name);
        if (OnDeath != null) {
            OnDeath();        
        }
        //Destroy(gameObject);
        gameObject.SetActive(false);
    }

    public bool IsAlive()
    {
        if(health > 0)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void Spawn()
    {
        gameObject.SetActive(true);
    }

    IEnumerator ToFollowPlayer()
    {
        IEnumerator thisCoroutine = null;
        bool firstTime = true;
        while(true)
        {
            Debug.LogFormat("Starting stalking! Distance is {0}", Vector2.Distance(target.position, transform.position));
            if(Vector2.Distance(target.position, transform.position) > MAX_DISTANCE)
            {
                Debug.Log("Player too far!");
                ChangeState(PlayerStates.moving);
                break;
            }
            else if(Vector2.Distance(target.position, transform.position) < MIN_DISTANCE)
            {
                Debug.Log("Player too close!");
                ChangeState(PlayerStates.attacking);
                break;
            }

            Vector3[] points = PathFinding.FindPath(transform.position, target.position);
            if(!firstTime)
            {
                StopCoroutine(thisCoroutine);
            }
            else
            {
                firstTime = false;
            }
            
            thisCoroutine = GoByPoints(points);
            StartCoroutine(thisCoroutine);

            yield return new WaitForSeconds(1f);
        }
        isStalking = false;
    }

    IEnumerator GoByPoints(Vector3 [] points)
    {
        float distance;
        float minDistance = 0.2f;
        int i = 0;

        foreach(Vector2 point in points)
        {
            do
            {
                distance = Vector2.Distance(transform.position, point);
                transform.position = Vector2.MoveTowards(transform.position, point, enemySpeed * Time.deltaTime);
                yield return null;
            }
            while (distance > minDistance);
            transform.position = transform.position;
            i++;
        }
        Debug.Log("End of coroutine");
    }

    Vector2[] Vector3ToVector2(Vector3[] points)
    {
        Vector2 [] points2 = new Vector2[points.Length];
        for(int i = 0; i < points.Length; i++)
        {
            points2[i] = new Vector2(points[i].x, points[i].y);
        }
        return points2;
    }

    private void Update()
    {
        if (health <= 0)
        {
            Death();
        }

        switch (currState)
        {
            case PlayerStates.moving:
                if (!isPatroling)
                {                    
                    isPatroling = true;
                    StartCoroutine(PatrolPositions(pointsToMove));
                }
                break;
            case PlayerStates.stalking:
                if(!isStalking)
                {
                    isStalking = true;
                    StartCoroutine(ToFollowPlayer());
                }
                break;
            case PlayerStates.attacking:
                if (!isAttacking)
                {
                    isAttacking = true;
                    StartCoroutine(AttackCoroutine());
                }
                break;
            case PlayerStates.stunned:
                Debug.LogFormat("Enemy {0} is stunned!", transform.name);
                break;
            case PlayerStates.isDead:
                Debug.LogFormat("Enemy {0} is dead!", transform.name);
                break;
            default:                
                break;
        }
    }

    public void ChangeState(PlayerStates state)
    {
        Debug.LogFormat("Changing to state {0}", state.ToString());
        if (state == PlayerStates.attacking)
        {
            currState = PlayerStates.attacking;
        }
        else if (state == PlayerStates.moving)
        {
            currState = PlayerStates.moving;
        }

        else if (state == PlayerStates.stalking)
        {
            currState = PlayerStates.stalking;
        }
        else
        {
            currState = state;
        }
    }

    IEnumerator StalkingCoroutine()
    {
        Vector2 enemyPosition = transform.position;
        Vector2 targetPosition = target.transform.position;

        while (true)
        {
            float distanceToTarget = Vector2.Distance(enemyPosition, targetPosition);
            if (distanceToTarget < MAX_DISTANCE)
            {
                if (distanceToTarget < 3f)
                {
                    Debug.Log("Player too close.");
                    isStalking = false;

                    ChangeState(PlayerStates.attacking);
                    yield break;
                }
                else
                {
                    MoveTowardsPlayer(Patrol(PathFinding.FindPath(enemyPosition, targetPosition)));
                }
            }
            else
            {                   
                break;
            }
            yield return new WaitForSeconds(3);
        }
        isStalking = false;
        ChangeState(PlayerStates.moving);
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
            {
                transform.position = Vector2.MoveTowards(transform.position, pos, enemySpeed * Time.deltaTime);
                Vector3 tmp = new Vector2(pos.x - transform.position.x, pos.y - transform.position.y);
                enemyDirection = tmp.normalized;
            }
        }
    }

    IEnumerator PatrolPositions(Transform[] points)
    {
        int i = 0;

        while (true)
        {
            float distFromPoint = Vector2.Distance(transform.position, points[i].position);
            
            if (distFromPoint > 0.3f)
            {
                MoveToThePoint(points[i].position);
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
                    if (i == (points.Length - 1))
                    {
                        i = 0;
                    }
                    else
                    {
                        i++;
                    }
                }

            }
            if (Vector2.Distance(transform.position, target.position) < 5f)
            {
                ChangeState(PlayerStates.stalking);
                break;
            }
            yield return null;
        }
        isPatroling = false;
    }

    private void MoveToThePoint(Vector2 pos)
    {
        transform.position = Vector2.MoveTowards(transform.position, pos, enemySpeed * Time.deltaTime);
        Vector3 tmp = new Vector2(pos.x - transform.position.x, pos.y - transform.position.y);
        enemyDirection = tmp.normalized;
    }

    protected IEnumerator AttackCoroutine()
    {
        while (true)
        {
            Collider2D playerHit = Physics2D.OverlapCircle(transform.position, radiusOfSight, playerLayer);
            if (playerHit)
            {
                Debug.Log("Player was hit");
                playerHit.GetComponent<PlayerManager>().TakeDamage(enemyDamage);
            }
            if (!(Vector2.Distance(transform.position, target.position) <= 3))
            {                
                break;
            }
            yield return new WaitForSeconds(toNextAttackTime);
        }

        ChangeState(PlayerStates.stalking);
        isAttacking = false;
    }
}
