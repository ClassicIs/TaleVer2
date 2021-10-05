using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CharacterOverlap : MonoBehaviour
{
    [SerializeField]
    private GameObject theLetter;
    private bool nearLetter;
    [SerializeField]
    private Player thePlayerController;
    [SerializeField]
    private PlayerEffectsScript thePlayerFX;
    [SerializeField]
    private Rigidbody2D thePlayer;

    [SerializeField]
    private GameObject walls;
    private CompositeCollider2D wallColl;
    [SerializeField]
    private bool isOnGround;    

    [SerializeField]
    private float radOfDetect;
    [SerializeField]
    private LayerMask theDeathLayer;

    public float InstadeathTime;
    [SerializeField]
    private float timeToDeath;

    private float inkChange;
    private float changedInkLevel;

    public bool isReading;
    [SerializeField]
    private Transform playerLegPos;

    [SerializeField]
    GameObject GMHolder;
    [SerializeField]
    GameManagerScript GMScript;
    private void Awake()
    {
        wallColl = walls.GetComponent<CompositeCollider2D>();

        radOfDetect = .05f;
        thePlayerController = GetComponent<Player>();
        thePlayer = GetComponent<Rigidbody2D>();
        thePlayerFX = GetComponent<PlayerEffectsScript>();
        GMScript = GMHolder.GetComponent<GameManagerScript>();
    }

    private void NormalizeAll()
    {
        isReading = false;
        isOnGround = true;
        InstadeathTime = 0;

        timeToDeath = .45f;
        changedInkLevel = 0;
        inkChange = 0.4f;
    }

    private void Update()
    {
        checkForDeath();
        CheckForLetter();
    }

    private void checkForDeath()
    {
        RaycastHit2D goingToDeath = Physics2D.Raycast(new Vector2(playerLegPos.position.x, playerLegPos.position.y), thePlayerController.theVectRaw, radOfDetect);
        //RaycastHit2D goingToDeath = Physics2D.BoxCast(playerLegPos.position, new Vector2(radOfDetect, radOfDetect), 90, thePlayerController.theVectRaw, theDeathLayer);

        if (goingToDeath.collider.gameObject.CompareTag("Instant Death") && !goingToDeath.collider.gameObject.CompareTag("Floor") && timeToDeath > 0)
        {
            Debug.Log("The raycast is " + goingToDeath.collider.gameObject.name);
            thePlayer.velocity = new Vector2(0, 0);
            thePlayerController.canWalk = false;
            timeToDeath -= Time.deltaTime;
        }
        else
        {
            timeToDeath = .45f;
            thePlayerController.canWalk = true;
        }

        if (!isOnGround && !thePlayerController.isDodging && !thePlayerController.isRestarting)
        {
            Debug.Log("Death is near!");
            InstadeathTime += Time.deltaTime;
            if (InstadeathTime >= timeToDeath)
            {
                Debug.Log("Death is hear!");
                GMScript.theDeath();
            }            
        }
        else
        {
            InstadeathTime = 0f;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag("Floor") || collision.CompareTag("Stairs"))
        {
            isOnGround = true;
        }

        if (collision.CompareTag("Stairs"))
        {
            Debug.Log("We go on stairs");
            wallColl.isTrigger = true;
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
            GMScript.ChangeMoney(1, collision.gameObject);

        }
        if (collision.CompareTag("Scary"))
        {
            thePlayerController.slowModif = 0.4f;
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor") || collision.CompareTag("Stairs"))
        {
            isOnGround = true;
        }

        if(collision.CompareTag("Stairs"))
        {

            Debug.Log("We are on stairs");
            wallColl.isTrigger = true;
        }

        if (collision.CompareTag("Scary"))
        {
            changedInkLevel += inkChange * Time.deltaTime;
            if (changedInkLevel >= 1f)
            {
                GMScript.toChangeHealth(Mathf.RoundToInt(changedInkLevel));
                changedInkLevel = 0;                
            }
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag("Floor") || collision.CompareTag("Stairs"))
        {
            isOnGround = false;
        }

        if (collision.CompareTag("Stairs"))
        {
            Debug.Log("Walls enabled");
            wallColl.isTrigger = false;
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
        theLetter.GetComponent<Animator>().SetBool("LetterOpen", true);

    }

    private void PaperClose()
    {
        isReading = false;
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
        //Gizmos.DrawLine(playerLegPos.position, playerLegPos.position + Vector3.up * radOfDetect);
        //Gizmos.DrawLine(playerLegPos.position, playerLegPos.position + new Vector3(thePlayerController.theVectRaw.x, thePlayerController.theVectRaw.y, 0) * radOfDetect);

        //Debug.Log("playerLegPos.position "+ playerLegPos.position);
        //Debug.Log("thePlayerController.theVectRaw: " + thePlayerController.theVectRaw);
    }

    
}
