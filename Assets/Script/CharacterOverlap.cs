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
    [SerializeField]
    private Player thePlayerController;
    private PlayerEffectsScript thePlayerFX;
    [SerializeField]
    private Transform playerLegPos;
    private Rigidbody2D thePlayer; 
    
    [SerializeField]
    private bool isOnGround;

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
        isReading = false;
        isOnGround = true;
        
        timeToInstaTime = 3f;
        InstadeathTime = timeToInstaTime;

        strTimeToDeath = .45f;
        timeToDeath = strTimeToDeath;        
        changedHealth = 0;
        healthChange = 0.4f;

    }

    private void Awake()
    {
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
        RaycastHit2D goingToDeath = Physics2D.Raycast(new Vector2(playerLegPos.position.x, playerLegPos.position.y), thePlayerController.theVectRaw, radOfDetect, theDeathLayer);
        //RaycastHit2D goingToDeath = Physics2D.BoxCast(playerLegPos.position, new Vector2(radOfDetect, radOfDetect), 90, thePlayerController.theVectRaw, theDeathLayer);

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
            //Debug.Log("The raycast is null");
        }


        if (!isOnGround)
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
                    Debug.Log("Death!");
                    thePlayerMG.theDeath();
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
        //Gizmos.DrawLine(playerLegPos.position, playerLegPos.position + new Vector3(thePlayerController.theVectRaw.x, thePlayerController.theVectRaw.y, 0) * radOfDetect);

    }    
}
