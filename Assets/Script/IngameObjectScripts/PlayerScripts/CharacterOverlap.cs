using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CharacterOverlap : MonoBehaviour
{
    public event Action OnFalling;
    //public event Action OnEnteringInk;
    //public event Action OnEscapingInk;    
    public event Action OnSaving;
    public event Action <int, Vector2> OnTakingCoin;
    public event Action OnDangerCollision;
    public event Action<DangerObject> OnDangerousObject;
    public event Action<InteractObject> OnNearInterObject;
    public event Action<InteractObject> OnFarInterObject;
    public event Action<PickableObject> OnPickableObject;

    private PlayerCharacterInput PlayerInput;

    /*public event Action<GameObject> OnNearLock;
    public event Action OnFarLock;
    */

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

    [SerializeField]
    private Player thePlayerController;
    private PlayerEffectsScript thePlayerFX;
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

    public bool isStunned;
    bool wasNearDeath = false;
    private void Awake()
    {
        CorToFindInterObject = FindInterObject();
    }
    private void Start()
    {
        isStunned = false;

        AssignValues();
        //timeToRes = ResetTheGame();

        theLastCheckpoint = firstCheckPoint.position;
        theLastAlivePosition = transform.position;
        
        isOnGround = true;    
        
        timeToInstaTime = 3f;
        InstadeathTime = timeToInstaTime;

        strTimeToDeath = .45f;
        timeToDeath = strTimeToDeath;      
        
        StartCoroutine(FindSafePlace());
        
    }

    private void AssignValues()
    {
        AllObjectsAround = new List<Transform>();
        PlayerInput = GetComponent<PlayerCharacterInput>();
        thePlayerAnim = GetComponent<Animator>();
        thePlayer = GetComponent<Rigidbody2D>();
        thePlayerFX = GetComponent<PlayerEffectsScript>();
    }

    private void Update()
    {        
        if (thePlayerController.isAlive)
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
        while(thePlayerController.isAlive)
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
        RaycastHit2D goingIntoWall = Physics2D.Raycast(transform.position, thePlayerController.theVectRaw, radOfWallDetect, theWallLayer);
        if(goingIntoWall.collider != null)
        {            
            //Debug.Log("Is hitting into a wall");
            lastHitObject = goingIntoWall.point;
        }
        
        RaycastHit2D goingToDeath = Physics2D.Raycast(new Vector2(playerLegPos.position.x, playerLegPos.position.y), thePlayerController.theVectRaw, radOfDropDetect, theDeathLayer);
        if (goingToDeath.collider != null)
        {
            if(timeToDeath > 0f)
            {
                //Debug.Log("Raycast is " + goingToDeath.collider.tag);
                thePlayer.velocity = new Vector2(0, 0);
                thePlayerController.ToStun(true);
                timeToDeath -= 0.5f * Time.deltaTime;
                wasNearDeath = true;
            }                      
        }
        else
        {
            if (wasNearDeath)
            {
                thePlayerController.ToStun(false);
                timeToDeath = strTimeToDeath;
                wasNearDeath = false;
            }
        }

        if (!isOnGround)
        {
            Debug.Log("Death is near!");
            if (thePlayerController.isDashing)
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

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (thePlayerController.isAlive)
        {
            if(collision.CompareTag("Spikes"))
            {
                Debug.Log("Spikes!");
                if (OnDangerCollision != null)
                {
                    Debug.Log("OnDangerCollision!");
                    OnDangerCollision();
                }
            }

            // To collect all Interactive objects in one array
            if(collision != null)
            {
                if(collision.GetComponent<InteractObject>())
                {
                    if(!AllObjectsAround.Contains(collision.GetComponent<Transform>()))
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

            if (collision.CompareTag("Coin"))
            {
                if (OnTakingCoin != null)
                {
                    OnTakingCoin(1, collision.gameObject.transform.position);
                }
                Destroy(collision.gameObject);
            }

            if (collision.CompareTag("Floor"))
            {
                isOnGround = true;
                thePlayerController.isGrounded = true;
            }

        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (thePlayerController.isAlive)
        {
            if (collision.CompareTag("Floor"))
            {
                isOnGround = true;
                thePlayerController.isGrounded = true;
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        // Empty list of Interact objects if exited
        if (collision != null)
        {
            if (collision.GetComponent<InteractObject>())
            {
                if (AllObjectsAround.Contains(collision.GetComponent<Transform>()))
                {
                    AllObjectsAround.Remove(collision.GetComponent<Transform>());                    
                    Debug.Log("Removed interactable object is " + collision.name);
                }
            }

            if (collision.CompareTag("Floor"))
            {
                isOnGround = false;
                thePlayerController.isGrounded = false;
            }
        }


    }

    // To Find the Nearest Interactable object
    public InteractObject theNearestObject()
    {
        float distance = Mathf.Infinity;
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
    }

    private void isAliveOrNot(bool isOrNot)
    {
        if (isOrNot)
        {
            thePlayerController.isAlive = true;            

        }
        else
        {
            thePlayer.velocity = Vector2.zero;
            thePlayerController.ChangeState(Player.PlayerStates.isDead);            
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
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(thePlayerController.theVectRaw.x, thePlayerController.theVectRaw.y, 0f) * radOfWallDetect);
    }    
}
