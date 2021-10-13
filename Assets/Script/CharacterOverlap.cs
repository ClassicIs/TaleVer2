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
    public event EventHandler OnTakingCoin;
    public event EventHandler OnSaving;
    
    [SerializeField]
    Transform[] checkForGround;
    [SerializeField]
    LayerMask theGroundLayer;

    [SerializeField]
    private GameObject theLetter;
    private bool nearLetter;

    private IEnumerator timeToRes;

    private GameManagerScript theGMScript;

    [SerializeField]
    private Player thePlayerController;
    private PlayerEffectsScript thePlayerFX;
    private Animator thePlayerAnim;

    [SerializeField]
    private Transform playerLegPos;
    private Rigidbody2D thePlayer; 
    
    [SerializeField]
    private bool isOnGround;
    public bool isAlive;
    [SerializeField]
    private int timeToCheck;

    public Vector2 theLastAlivePosition;
    public Vector2 theLastCheckpoint;

    [SerializeField]
    private float radOfDetect = .05f;
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

    private float healthChange;
    private float changedHealth;
    public bool isReading;
    
    

    private void Start()
    {
        theLastCheckpoint = transform.position;
        theGMScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
        thePlayerAnim = GetComponent<Animator>();

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

    private void Awake()
    {
        thePlayer = GetComponent<Rigidbody2D>();
        thePlayerFX = GetComponent<PlayerEffectsScript>();
    }

    private void Update()
    {
        checkForDeath();
        CheckForLetter();
    }

    IEnumerator findSafePlace()
    {
        while(isAlive)
        {
            bool isItSafe = false;
            foreach(Transform transf1 in checkForGround)
            {
                if (Physics2D.OverlapCircle(transf1.position, radOfDetect, theGroundLayer))
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
        RaycastHit2D goingToDeath = Physics2D.Raycast(new Vector2(playerLegPos.position.x, playerLegPos.position.y), thePlayerController.theVectRaw, radOfDetect, theDeathLayer);
        if (goingToDeath.collider != null)
        {
            if(timeToDeath > 0f)
            {
                Debug.Log("Raycast is " + goingToDeath.collider.tag);
                thePlayer.velocity = new Vector2(0, 0);
                thePlayerController.canWalk = false;
                timeToDeath -= 0.5f * Time.deltaTime;
            }
            else
            {
                thePlayerController.canWalk = true;
            }            
        }
        else
        {
            timeToDeath = strTimeToDeath;
            thePlayerController.canWalk = true;
        }


        if (!isOnGround && isAlive)
        {
            Debug.Log("Death is near!");
            if (!thePlayerController.isDodging || thePlayerController.isRestarting)
            {
                if (InstadeathTime > 0f)
                {
                    InstadeathTime -= 0.5f * Time.deltaTime;

                }
                else
                {
                    if (OnFalling != null)
                    {
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
        if(collision.CompareTag("Checkpoint"))
        {
            if (OnSaving != null)
            {
                OnSaving(this, EventArgs.Empty);
            }

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
            /*
            if (OnTakingCoin != null)
            {
                OnTakingCoin(this, EventArgs.Empty);
            }*/

            theGMScript.ChangeMoney(1, collision.gameObject.transform.position);
            Destroy(collision.gameObject);
        }
        if (collision.CompareTag("Scary"))
        {
            if(OnEnteringInk != null)
            {
                OnEnteringInk(this, EventArgs.Empty);
            }
            thePlayerController.slowModif = 0.4f;
        }
        if (collision.CompareTag("MovingPlatform"))
        {
            Debug.Log("is on the moving plat");
            thePlayerController. isSliding = true;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("MovingPlatform"))
        {
            Debug.Log("is on the moving plat");
            thePlayerController.isSliding = true;
        }

        if (collision.CompareTag("Floor"))
        {
            isOnGround = true;
            thePlayerController.isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Scary"))
        {
            thePlayerController.slowModif = 1f;
            if(OnEscapingInk != null)
            {
                OnEscapingInk(this, EventArgs.Empty);
            }
            //thePlayerFX.endTrail = true;
        }

        if (collision.CompareTag("Letter"))
        {
            Debug.Log("Letter out is.");
            nearLetter = false;
            PaperClose();
        }

        if (collision.CompareTag("Floor"))
        {
            isOnGround = false;
            thePlayerController. isGrounded = false;
        }

        if (collision.CompareTag("MovingPlatform"))
        {
            Debug.Log("Not on the moving plat");
            thePlayerController.isSliding = false;
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
        isReading = true;
        Debug.Log("Here we are opening!");
        theLetter.GetComponent<Animator>().SetBool("LetterOpen", true);

    }

    private void PaperClose()
    {
        isReading = false;
        Debug.Log("Here we are closing!");
        theLetter.GetComponent<Animator>().SetBool("LetterOpen", false);
    }

    public void NearlyDeath()
    {
        isAlive = false;
        Debug.Log("Dead!");
        
        transform.position = theLastAlivePosition;
        isAlive = true;
    }
    public void Death()
    {
        timeToRes = resetTheGame();
        StartCoroutine(timeToRes);
    }
    IEnumerator resetTheGame()
    {
        yield return new WaitForSeconds(10);
        transform.position = theLastCheckpoint;
        thePlayerController.NormalizeAll();

    }

    public void InkDeath()
    {
        thePlayerAnim.SetBool("IsDeadInk", true);
        StartCoroutine(AfterInk());
    }

    IEnumerator AfterInk()
    {
        yield return new WaitForSeconds(10);
        thePlayerAnim.SetBool("IsDeadInk", false);
        StartCoroutine(resetTheGame());
    }

    private void OnDrawGizmos()
    {
        //Gizmos.DrawLine(playerLegPos.position, playerLegPos.position + new Vector3(thePlayerController.theVectRaw.x, thePlayerController.theVectRaw.y, 0) * radOfDetect);

    }    
}
