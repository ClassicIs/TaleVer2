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
    //public event Action<string, string> OnReadingLetter;

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

    [SerializeField]
    private GameObject theLetter;
    private bool nearLetter;

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


    public bool isReading; 
    
    private void Start()
    {
        timeToRes = ResetTheGame();
        theLastCheckpoint = firstCheckPoint.position;
        thePlayerAnim = GetComponent<Animator>();
        thePlayer = GetComponent<Rigidbody2D>();
        thePlayerFX = GetComponent<PlayerEffectsScript>();

        theLastAlivePosition = transform.position;
        
        isAlive = true;
        isReading = false;
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
            checkForDeath();
            CheckForLetter();
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
            thePlayerController.currState = Player.PlayerStates.stunned;
        }
        else
        {
            thePlayerController.currState = Player.PlayerStates.moving;
        }
        RaycastHit2D goingToDeath = Physics2D.Raycast(new Vector2(playerLegPos.position.x, playerLegPos.position.y), thePlayerController.theVectRaw, radOfDropDetect, theDeathLayer);
        if (goingToDeath.collider != null)
        {
            if(timeToDeath > 0f)
            {
                //Debug.Log("Raycast is " + goingToDeath.collider.tag);
                thePlayer.velocity = new Vector2(0, 0);
                thePlayerController.currState = Player.PlayerStates.stunned;
                timeToDeath -= 0.5f * Time.deltaTime;
            }
            else
            {
                thePlayerController.currState = Player.PlayerStates.moving;
            }            
        }
        else
        {
            timeToDeath = strTimeToDeath;
            thePlayerController.currState = Player.PlayerStates.moving;
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
                    isAlive = false;
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
                nearLetter = true;
                Text[] allText = theLetter.GetComponentsInChildren<Text>();
                allText[0].text = collision.GetComponent<LetterScript>().sign;
                allText[1].text = collision.GetComponent<LetterScript>().theMassOfStrings;
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
                nearLetter = false;
                if (isReading)
                {
                    PaperClose();
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

    private void CheckForLetter()
    {
        if (nearLetter)
        {
            if (Input.GetKeyDown(KeyCode.F) && !isReading)
            {
                PaperOpen();
            }
            else if (Input.GetKeyDown(KeyCode.F) && isReading)
            {
                PaperClose();
            }
        }
        else if (!nearLetter && isReading)
        {
            PaperClose();
        }
    }

    private void PaperOpen()
    {
        Debug.Log("Paper Opened");
        isReading = true;
        theLetter.SetActive(true);
    }

    private void PaperClose()
    {
        Debug.Log("Paper closed");
        isReading = false;
        theLetter.SetActive(false);
    }
    
    public void Death(PlayerDeath states)
    {
        thePlayerController.currState = Player.PlayerStates.stunned;
        switch (states)
        {
            case PlayerDeath.allInDeath:
                DeathInAll();
                break;
            case PlayerDeath.inkDeath:
                InkDeath();
                break;
            case PlayerDeath.intermidiateDeath:
                NearlyDeath();
                break;
        }        
    }

    private void DeathInAll()
    {
        transform.position = theLastCheckpoint;
        thePlayerController.currState = Player.PlayerStates.moving;
    }    

    public void NearlyDeath()
    {        
        Debug.Log("Dead!");
        transform.position = theLastAlivePosition;
        isAlive = true;
    }

    public void InkDeath()
    {
        isAlive = false;
        thePlayerController.currState = Player.PlayerStates.stunned;
        thePlayerAnim.SetBool("IsDeadInk", true);        
        StartCoroutine(AfterInk());
    }

    IEnumerator AfterInk()
    {
        yield return new WaitForSeconds(1);
        thePlayerAnim.SetBool("IsDeadInk", false);
        StartCoroutine(timeToRes);
    }

    IEnumerator ResetTheGame()
    {
        yield return new WaitForSeconds(1);
        Debug.LogWarning("Position of the player must be " + theLastCheckpoint);
        transform.position = theLastCheckpoint;
        thePlayerController.currState = Player.PlayerStates.moving;
        isAlive = true;
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
