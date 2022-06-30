using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CharacterOverlap : MonoBehaviour
{
    public event Action OnFalling;
   
    public event Action OnSaving;
    public event Action <int, Vector2> OnTakingCoin;
    public event Action OnDangerCollision;
    
    public event Action<DangerObject> OnDangerousObject;
    public event Action<DangerObject> OnDangerFar;

    public event Action<InteractObject> OnNearInterObject;
    public event Action<InteractObject> OnFarInterObject;
    public event Action<PickableObject> OnPickableObject;
    public event Action<AutoQTEScript> OnEnteredQTE;

    public event Action OnEndOfInkDeath;

    public event Action OnEndOfLevel;

    public enum PlayerDeath
    {
        allInDeath,
        intermidiateDeath,
        inkDeath
    }

    [SerializeField]
    Transform[] checkForGround;
    [SerializeField]
    LayerMask theGroundLayer;
    [SerializeField]
    LayerMask theWallLayer;

    private IEnumerator timeToRes;
    private IEnumerator CorToFindInterObject;

    private Player Player;
    private PlayerEffectsScript thePlayerFX;
    private PlayerManager PlayerManager;
    private Animator thePlayerAnim;

    [SerializeField]
    private Transform playerLegPos;    
    private Rigidbody2D thePlayer; 
    
    [SerializeField]
    private bool isOnGround;

    Vector2 lastHitObject;
    [SerializeField]
    private int timeToCheck;

    public Vector2 theLastAlivePosition;
    public Vector2 theLastCheckpoint;

    [SerializeField]
    private float radOfGroundDetect = .05f;
    [SerializeField]
    private float radOfWallDetect = 1f;
    [SerializeField]
    private float radOfDropDetect = 0.1f;
    [SerializeField]
    private LayerMask theDeathLayer;    

    [SerializeField]
    public float InstadeathTime;
    [SerializeField]
    public float timeToInstaTime;

    [SerializeField]
    private float timeToDeath;
    [SerializeField]
    private float strTimeToDeath;

    [SerializeField]
    Transform firstCheckPoint;

    List <Transform> AllObjectsAround;
    InteractObject NearObject;

    DangerObject lastDangerObjectToHit;

    public bool isStunned;
    bool wasNearDeath = false;
    bool isDashing;
    bool checkCollisions;
    private void Awake()
    {
        AssignValues();
        CorToFindInterObject = FindInterObject();
    }

    private void Start()
    {
        isStunned = false;
        isDashing = false;
        checkCollisions = false;

        theLastAlivePosition = transform.position;
        
        isOnGround = true;    
        
        timeToInstaTime = 3f;
        InstadeathTime = timeToInstaTime;

        strTimeToDeath = .45f;
        timeToDeath = strTimeToDeath;

        Player.OnDashStart += IfDashStart;
        Player.OnDashEnd += IfDashEnd;

        // Is it needed?
        //StartCoroutine(FindSafePlace());
    }

    private void AssignValues()
    {        
        AllObjectsAround = new List<Transform>();
        thePlayerAnim = GetComponent<Animator>();
        thePlayer = GetComponent<Rigidbody2D>();
        PlayerManager = GetComponent<PlayerManager>();
        Player = GetComponent<Player>();
        thePlayerFX = GetComponent<PlayerEffectsScript>();
    }

    private void Update()
    {        
        if (Player.isAlive)
        {
            CheckForDeath();
        }
        /*
        if (AllObjectsAround != null)
        {
            StartCoroutine(CorToFindInterObject);
        }
        else
        {
            StopCoroutine(CorToFindInterObject);
        }*/
    }
    
    IEnumerator FindInterObject()
    {
        InteractObject TMPInterObject = null;
        while(true)
        {
            if (TMPInterObject != theNearestObject())
            {
                if (OnNearInterObject != null)
                {
                    OnFarInterObject(TMPInterObject);
                    OnNearInterObject(theNearestObject());
                }
            }
            yield return null;
        }
    }

    // To Find Last Save Space where Was Player
    IEnumerator FindSafePlace()
    {
        while(Player.isAlive)
        {
            bool isItSafe = false;
            foreach(Transform transf1 in checkForGround)
            {
                if (Physics2D.OverlapCircle(transf1.position, radOfGroundDetect, theGroundLayer))
                {
                    isItSafe = true;
                }
                else
                {
                    isItSafe = false;
                    break;
                }                    
            }
            if (isItSafe)
            {
                theLastAlivePosition = transform.position;
            }
            yield return new WaitForSeconds(timeToCheck);
        }
    }

    // To stop Player from Falling
    private void CheckForDeath()
    {
        RaycastHit2D goingIntoWall = Physics2D.Raycast(transform.position, Player.theVectRaw, radOfWallDetect, theWallLayer);
        if(goingIntoWall.collider != null)
        {            
            //Debug.Log("Is hitting into a wall");
            lastHitObject = goingIntoWall.point;
        }
        
        RaycastHit2D goingToDeath = Physics2D.Raycast(new Vector2(playerLegPos.position.x, playerLegPos.position.y), Player.theVectRaw, radOfDropDetect, theDeathLayer);
        if (goingToDeath.collider != null)
        {
            if(timeToDeath > 0f)
            {
                //Debug.Log("Raycast is " + goingToDeath.collider.tag);
                thePlayer.velocity = new Vector2(0, 0);
                Player.ToStun(true);
                timeToDeath -= 0.5f * Time.deltaTime;
                wasNearDeath = true;
            }                      
        }
        else
        {
            if (wasNearDeath)
            {
                Player.ToStun(false);
                timeToDeath = strTimeToDeath;
                wasNearDeath = false;
            }
        }

        if (!isOnGround)
        {
            Debug.Log("Death is near!");
            if (Player.isDashing)
            {
                if (InstadeathTime > 0f)
                {
                    InstadeathTime -= 5f * Time.deltaTime;
                }
                else
                {
                    if (OnFalling != null)
                    {
                        Debug.Log("OnFalling");
                        OnFalling();
                    }
                }
            }
        }
        else
        {
            InstadeathTime = timeToInstaTime;
        }
    }

    private void IfDashStart()
    {
        isDashing = true;
    }

    private void IfDashEnd()
    {
        //Debug.LogError("Character overlap Dash has ended");
        
        ToCheckCollisions();
        
    }

    public void ToCheckCollisions()
    {
        StartCoroutine(WaitFor());
    }

    IEnumerator WaitFor()
    {
        checkCollisions = true;
        firstTime = true;
        //Debug.LogError("Checking collision 0");
        yield return null;
        checkCollisions = false;
        isDashing = false;
        //Debug.LogError("End of checking collision");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (Player.isAlive && !isDashing)
        {
            CheckForAllCollisions(collision);
        }
    }

    private void CheckForAllCollisions(Collider2D collision)
    {
        Debug.Log("Checking for all collisions");
        if (collision.GetComponent<AutoQTEScript>())
        {
            if (!collision.GetComponent<AutoQTEScript>().alreadyUsed)
            {
                if (OnEnteredQTE != null)
                {
                    OnEnteredQTE(collision.GetComponent<AutoQTEScript>());
                }
            }
        }

        if (collision.GetComponent<InteractObject>())
        {
            // To collect all Interactive objects in one array
            if (!AllObjectsAround.Contains(collision.GetComponent<Transform>()))
            {
                AllObjectsAround.Add(collision.GetComponent<Transform>());
                Debug.Log("New interactable object is " + collision.name);
            }
        }
        if (collision.GetComponent<PickableObject>())
        {
            if (OnPickableObject != null)
            {
                OnPickableObject(collision.GetComponent<PickableObject>());
            }
        }

        if (collision.GetComponent<DangerObject>())
        {
            lastDangerObjectToHit = collision.GetComponent<DangerObject>();
            if (OnDangerousObject != null)
            {
                OnDangerousObject(lastDangerObjectToHit);
            }
        }

        if (collision.CompareTag("Checkpoint"))
        {
            if (OnSaving != null)
            {
                OnSaving();
            }
            Debug.Log("Saved");
            theLastCheckpoint = collision.gameObject.transform.position;
            collision.gameObject.SetActive(false);
        }

        if (collision.CompareTag("Floor"))
        {
            isOnGround = true;
            Player.isGrounded = true;
        }
        
        //Obsolete

        if (collision.CompareTag("Coin"))
        {
            if (OnTakingCoin != null)
            {
                OnTakingCoin(1, collision.gameObject.transform.position);
            }
            Destroy(collision.gameObject);
        }
       
        if (collision.CompareTag("Spikes"))
        {
            Debug.Log("Spikes!");
            if (OnDangerCollision != null)
            {
                Debug.Log("OnDangerCollision!");
                OnDangerCollision();
            }
        }

        if (collision.CompareTag("EndOfLevel"))
        {
            if (OnEndOfLevel != null)
            {
                OnEndOfLevel();
            }
        }

    }

    
    bool firstTime = false;
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (Player.isAlive)
        {
            if (collision.CompareTag("Floor"))
            {
                isOnGround = true;
                Player.isGrounded = true;
            }
        }

        if(checkCollisions && firstTime)
        {
            //Debug.LogError("Checking collision");
            AllObjectsAround.Clear();
            if (lastDangerObjectToHit != null)
            {
                if (OnDangerFar != null)
                {
                    OnDangerFar(lastDangerObjectToHit);
                }
                lastDangerObjectToHit = null;
            }
            CheckForAllCollisions(collision);
            firstTime = false;
        }
        
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (!isDashing)
        {
            // Empty list of Interact objects if exited
            if (collision.GetComponent<InteractObject>())
            {
                if (AllObjectsAround.Contains(collision.GetComponent<Transform>()))
                {
                    AllObjectsAround.Remove(collision.GetComponent<Transform>());
                    Debug.Log("Removed interactable object is " + collision.name);
                }
            }

            if (collision.GetComponent<DangerObject>())
            {
                if (OnDangerFar != null)
                {
                    OnDangerFar(collision.GetComponent<DangerObject>());
                }
            }

            if (collision.CompareTag("Floor"))
            {
                isOnGround = false;
                Player.isGrounded = false;
            }
        }

    }

    // To Find the Nearest Interactable object
    public InteractObject theNearestObject()
    {
        float distance = Mathf.Infinity;
        /*if (AllObjectsAround.Count <= 0)
            return null;*/
        Transform TheNearestObject = null;

        foreach (Transform obj in AllObjectsAround)
        {
            Vector2 VectorToTarget = transform.position - obj.position;
            float distToTarget = VectorToTarget.sqrMagnitude;
            if (distToTarget < distance)
            {
                distance = distToTarget;
                TheNearestObject = obj;
            }
        }
        InteractObject CurrNearObject = null;

        if (TheNearestObject)
        {
            CurrNearObject = TheNearestObject.GetComponent<InteractObject>();            
        } 

        if (CurrNearObject)
        {
            Debug.Log("Current near object is " + CurrNearObject.name);
            return CurrNearObject;
        }
        else
        {
            Debug.Log("There is nothing to interact with!");
            return null;
        }
    }

    // TODO To ResetManager

    public void InterDeath()
    {
        isAliveOrNot(false);
    }

    public void allInDeath()
    {
        isAliveOrNot(false);
    }

    public void SetPlayerTo(bool wasHeDead)
    {        
        if (wasHeDead)
        {
            transform.position = theLastCheckpoint;
        }
        else
        {
            transform.position = theLastAlivePosition;
        }
        isAliveOrNot(true);
    }

    public void InkDeath(bool isDeadOr)
    {
        if (isDeadOr)
        {
            isAliveOrNot(false);
            thePlayerAnim.SetBool("IsDeadInk", true);
        }
        else
        {
            isAliveOrNot(true);
            thePlayerAnim.SetBool("IsDeadInk", false);
        }
        StartCoroutine(InkDeathComplete());
    }

    IEnumerator InkDeathComplete()
    {
        yield return new WaitForSeconds(1.3f);
        OnEndOfInkDeath?.Invoke();
    }

    private void isAliveOrNot(bool isOrNot)
    {
        if (isOrNot)
        {
            Player.isAlive = true;            

        }
        else
        {
            thePlayer.velocity = Vector2.zero;
            Player.ChangeState(Player.PlayerStates.isDead);            
        }
    }


    // TODO To Player Manager
    

    private void OnDrawGizmos()
    {
        foreach(Transform obj in checkForGround)
        {
            Gizmos.DrawWireSphere(obj.position, radOfGroundDetect);
        }
        Gizmos.color = new Color(1, 0, 0);
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(Player.theVectRaw.x, Player.theVectRaw.y, 0f) * radOfWallDetect);
    }    
}
