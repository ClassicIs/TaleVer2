using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterOverlap : MonoBehaviour
{
    [SerializeField]
    private GameObject theLetter;
    private bool nearLetter;
    private PlayerManager thePlayerMG;
    private Player thePlayerController;
    private PlayerEffectsScript thePlayerFX;

    private Rigidbody2D thePlayer;

    [SerializeField]
    private Transform leftTop;
    [SerializeField]
    private Transform rightTop;
    [SerializeField]
    private Transform leftBottom;
    [SerializeField]
    private Transform rightBottom;

    private bool isLeftTop;
    private bool isRightTop;
    private bool isLeftBottom;
    private bool isRightBottom;
    private bool isOnGround;
    private bool isOnTheInstantDeath;

    [SerializeField]
    private float radOfDetect;
    [SerializeField]
    private LayerMask theDeathLayer;

    public float InstadeathTime;
    [SerializeField]
    private float timeToDeath;
    [SerializeField]
    private float strTimeToDeath;

    private float healthChange;
    private float changedHealth;
    public bool isReading;
    [SerializeField]
    private Transform playerLegPos;

    private void Start()
    {        
        isReading = false;
        isOnGround = true;
        InstadeathTime = 0;
        strTimeToDeath = .45f;
        timeToDeath = strTimeToDeath;
        isOnTheInstantDeath = false;
        changedHealth = 0;
        healthChange = 0.4f;

    }

    private void Awake()
    {
        radOfDetect = .05f;
        thePlayerController = GetComponent<Player>();
        thePlayer = GetComponent<Rigidbody2D>();
        thePlayerFX = GetComponent<PlayerEffectsScript>();
        thePlayerMG = GetComponent<PlayerManager>();
    }

    private void Update()
    {
        checkForDeath();
        CheckForLetter();
    }

    private void checkForDeath()
    {
        /*isLeftTop = Physics2D.OverlapCircle(leftTop.position, radOfDetect, theDeathLayer);
        isRightTop = Physics2D.OverlapCircle(rightTop.position, radOfDetect, theDeathLayer);
        isLeftBottom = Physics2D.OverlapCircle(leftBottom.position, radOfDetect, theDeathLayer);
        isRightBottom = Physics2D.OverlapCircle(rightBottom.position, radOfDetect, theDeathLayer);

        bool fallRight = isRightBottom && isRightTop;
        bool fallLeft = isLeftBottom && isLeftTop;
        bool fallBottom = isRightBottom || isLeftBottom;
        bool fallTop = isLeftTop && isRightTop;
        */
        bool isOnTheDeathFloor = isLeftTop && isRightTop && isLeftBottom && isRightBottom;
        //Physics2D.BoxCast()
        //RaycastHit2D goingToDeath = Physics2D.BoxCast(transform.position, new Vector2(1f, 1f), 0f, thePlayerController.theVectRaw, 1f, theDeathLayer);
        RaycastHit2D goingToDeath = Physics2D.Raycast(new Vector2(playerLegPos.position.x, playerLegPos.position.y), thePlayerController.theVectRaw, radOfDetect, theDeathLayer);
        //RaycastHit2D goingToDeath = Physics2D.BoxCast(playerLegPos.position, new Vector2(radOfDetect, radOfDetect), 90, thePlayerController.theVectRaw, theDeathLayer);

        if (goingToDeath.collider != null && timeToDeath > 0f)
        {
            Debug.Log("The raycast is " + goingToDeath.collider.gameObject.name);
            thePlayer.velocity = new Vector2(0, 0);
            thePlayerController.canWalk = false;
            timeToDeath -= 0.1f;
        }
        else
        {
            timeToDeath = strTimeToDeath;
            thePlayerController.canWalk = true;
        }

        if (!isOnGround)
        {
            Debug.Log("Death is near!");

            if (!thePlayerController.isDodging || thePlayerController.isRestarting)
            {
                InstadeathTime += Time.deltaTime;
                if (InstadeathTime >= timeToDeath)
                {
                    Debug.Log("Death is hear!");
                    thePlayerMG.theDeath();
                }
            }
        }
        else
        {
            InstadeathTime = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Instant Death"))
        {
            isOnTheInstantDeath = true;
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
            thePlayerMG.ToChangeCoins(1, collision.gameObject);

        }
        if (collision.CompareTag("Scary"))
        {
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
        if (collision.CompareTag("Scary"))
        {
            changedHealth += healthChange * Time.deltaTime;
            if (changedHealth >= 0f)
            {
                thePlayerMG.ToChangeHealth(Mathf.RoundToInt(changedHealth));
                changedHealth = 0;                
            }
        }

        if (collision.CompareTag("Floor"))
        {
            isOnGround = true;
            thePlayerController.isGrounded = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Instant Death"))
        {
            isOnTheInstantDeath = false;
        }
        if (collision.CompareTag("Scary"))
        {
            thePlayerController.slowModif = 1f;
            thePlayerFX.endTrail = true;
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

    private void OnDrawGizmos()
    {
        /*
        Gizmos.DrawWireSphere(leftTop.position, radOfDetect);
        Gizmos.DrawWireSphere(rightTop.position, radOfDetect);
        Gizmos.DrawWireSphere(leftBottom.position, radOfDetect);
        Gizmos.DrawWireSphere(rightBottom.position, radOfDetect);
        */
        //Gizmos.DrawLine(playerLegPos.position, playerLegPos.position + new Vector3(thePlayerController.theVectRaw.x, thePlayerController.theVectRaw.y, 0) * radOfDetect);

        //Debug.Log("playerLegPos.position "+ playerLegPos.position);
        //Debug.Log("thePlayerController.theVectRaw: " + thePlayerController.theVectRaw);
    }

    
}
