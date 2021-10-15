using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class CharacterOverlap : MonoBehaviour
{
    public event EventHandler OnFalling;
    public event EventHandler OnEnteringInk;
    public event EventHandler OnEscapingInk;    
    public event EventHandler OnSaving;
    public event Action <int, Vector2> OnTakingCoin;

    public event Action<string, string> OnNearLetter;
    public event EventHandler OnFarLetter;


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

    [SerializeField]
    private Player thePlayerController;
    private PlayerEffectsScript thePlayerFX;
    private Animator thePlayerAnim;

    [SerializeField]
    private Transform playerLegPos;
    [SerializeField]
    Transform tmpObj;
    private Rigidbody2D thePlayer; 
    
    [SerializeField]
    private bool isOnGround;
    public bool isAlive;
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

    
    private void Start()
    {
        //timeToRes = ResetTheGame();
        theLastCheckpoint = firstCheckPoint.position;
        thePlayerAnim = GetComponent<Animator>();
        thePlayer = GetComponent<Rigidbody2D>();
        thePlayerFX = GetComponent<PlayerEffectsScript>();

        theLastAlivePosition = transform.position;
        
        isAlive = true;
        isOnGround = true;    
        
        timeToInstaTime = 3f;
        InstadeathTime = timeToInstaTime;

        strTimeToDeath = .45f;
        timeToDeath = strTimeToDeath;      
        
        StartCoroutine(findSafePlace());
    }

    private void Update()
    {
        if (isAlive)
        {
            //Debug.LogWarning()
            checkForDeath();
        }
        if (Input.GetKeyDown(KeyCode.U))
        {
            tmpObj.position = lastHitObject;
        }
    }

    IEnumerator findSafePlace()
    {
        while(isAlive)
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

    private void checkForDeath()
    {
        RaycastHit2D goingIntoWall = Physics2D.Raycast(transform.position, thePlayerController.theVectRaw, radOfWallDetect, theWallLayer);
        if(goingIntoWall.collider != null)
        {
            
            Debug.Log("Is hitting into a wall");
            lastHitObject = goingIntoWall.point;
            thePlayerController.Stunned();
        }
        else
        {
            thePlayerController.Unstunned();
        }
        RaycastHit2D goingToDeath = Physics2D.Raycast(new Vector2(playerLegPos.position.x, playerLegPos.position.y), thePlayerController.theVectRaw, radOfDropDetect, theDeathLayer);
        if (goingToDeath.collider != null)
        {
            if(timeToDeath > 0f)
            {
                //Debug.Log("Raycast is " + goingToDeath.collider.tag);
                thePlayer.velocity = new Vector2(0, 0);
                thePlayerController.Stunned();
                timeToDeath -= 0.5f * Time.deltaTime;
            }
            else
            {
                thePlayerController.Unstunned();
            }            
        }
        else
        {
            timeToDeath = strTimeToDeath;
            thePlayerController.Unstunned();
        }

        if (!isOnGround)
        {
            Debug.Log("Death is near!");
            if (!thePlayerController.isDodging)
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
                        OnFalling(this, EventArgs.Empty);
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
        if (isAlive)
        {
            if (collision.CompareTag("Checkpoint"))
            {
                if (OnSaving != null)
                {
                    OnSaving(this, EventArgs.Empty);
                }
                Debug.Log("Saved");
                theLastCheckpoint = collision.gameObject.transform.position;
                collision.gameObject.SetActive(false);
            }

            if (collision.CompareTag("Letter"))
            {
                //nearLetter = true;
                Debug.Log("nearLetter = true;!");
                if (OnNearLetter != null)
                {
                    Debug.Log("OnNearLetter!");
                    OnNearLetter (collision.GetComponent<LetterScript>().sign, collision.GetComponent<LetterScript>().theMassOfStrings);                    
                }
            }

            if (collision.CompareTag("Coin"))
            {
                if (OnTakingCoin != null)
                {
                    OnTakingCoin(1, collision.gameObject.transform.position);
                }
                Destroy(collision.gameObject);
            }

            if (collision.CompareTag("Scary"))
            {
                if (OnEnteringInk != null)
                {
                    OnEnteringInk(this, EventArgs.Empty);
                }
                thePlayerController.slowModif = 0.4f;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isAlive)
        {
            if (collision.CompareTag("Floor"))
            {
                isOnGround = true;

            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (isAlive)
        {
            if (collision.CompareTag("Scary"))
            {
                thePlayerController.slowModif = 1f;
                if (OnEscapingInk != null)
                {
                    OnEscapingInk(this, EventArgs.Empty);
                }
                //thePlayerFX.endTrail = true;
            }

            if (collision.CompareTag("Letter"))
            {
                Debug.Log("nearLetter = false;!");
                if (OnFarLetter != null)
                {
                    Debug.Log("OnFarLetter!");
                    OnFarLetter(this, EventArgs.Empty);
                }                
            }

            if (collision.CompareTag("Floor"))
            {
                isOnGround = false;
                thePlayerController.isGrounded = false;
            }

            if (collision.CompareTag("MovingPlatform"))
            {
                Debug.Log("Not on the moving plat");
                thePlayerController.isSliding = false;
            }
        }
    }  
    
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
    
    private void isAliveOrNot(bool isOrNot)
    {
        if (isOrNot)
        {            
            thePlayerController.Unstunned();
            isAlive = true;
        }
        else
        {
            thePlayer.velocity = Vector2.zero;
            thePlayerController.Stunned();
            isAlive = false;
        }
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


    private void OnDrawGizmos()
    {
        foreach(Transform obj in checkForGround)
        {
            Gizmos.DrawWireSphere(obj.position, radOfGroundDetect);
        }
        Gizmos.color = new Color(1, 0, 0);
        Gizmos.DrawLine(transform.position, transform.position + new Vector3(thePlayerController.theVectRaw.x, thePlayerController.theVectRaw.y, 0f) * radOfWallDetect);

        //Gizmos.DrawLine(playerLegPos.position, playerLegPos.position + new Vector3(thePlayerController.theVectRaw.x, thePlayerController.theVectRaw.y, 0) * radOfDetect);

    }    
}
