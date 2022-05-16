using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : AliveBeeing
{   
    private Rigidbody2D thePlayer;
    private Animator thePlayerAnim;
    private PlayerEffectsScript theFXScript;
    private AudioManagerScript theAudioManager;
    private PlayerCharacterInput thePlayerInput;
    private List <string> theInventory;

    public LayerMask theWallLayer;
    
    public Transform tmpGameObj;
    public Vector2 theVectRaw;
    private Vector2 theVect;

    private Vector2 lastMovePosition;

    [SerializeField]
    private Vector2 lastMoveDir;

    [SerializeField]
    Vector2 movePosition;
    public bool isMoving;

    public float dodge;
    private float startDodgeTime;
    [SerializeField]
    private float speedDodge;   
    
    public float speed;    
    private float normSpeed;
    public float slowModif;

    public bool canWalk;
    public bool isSlowDown;

    private bool isTryingToDie;   

    private float currTimeToFall;
    [SerializeField] 
    private float strTimeToFall;
    
    public bool isGrounded;
    public bool isSliding;

    // Start is called before the first frame update
    void Start()
    {
        thePlayerInput = GetComponent<PlayerCharacterInput>();
        currState = PlayerStates.moving;
        movePosition = transform.position;
        lastMovePosition = new Vector2(0, 0);
        //To check for last movement vector
        lastMoveDir = new Vector2(0, 0);

        theVectRaw = new Vector2(0, 0);
        theAudioManager = FindObjectOfType<AudioManagerScript>();
        theFXScript = GetComponent<PlayerEffectsScript>();
        
        //Speed variables        
        normSpeed = 1.5f;        
        speed = normSpeed;
        slowModif = 1f; //Modifikator for going in ink        

        //For dodge
        speedDodge = 500f; //Speed of dodge
        startDodgeTime = 1f;
        dodge = 0;

        isSlowDown = false;
    }

    private void Awake()
    {
        //Taking all variables from game object
        thePlayer = GetComponent<Rigidbody2D>();
        thePlayerAnim = GetComponent<Animator>();
    }

    public void SlowEffectOn(float SlowModifier)
    {
        isSlowDown = true;
        slowModif = SlowModifier;
    }
    public void SlowEffectOff()
    {
        isSlowDown = false;
        slowModif = 1f;
    }

    public void Move(float horMovement, float verMovement)
    {
        theVectRaw = new Vector2(horMovement, verMovement).normalized;
        theVect = new Vector2(horMovement, verMovement).normalized;
        if (Mathf.Abs(theVectRaw.x) > 0.1f || Mathf.Abs(theVectRaw.y) > 0.1f)
        {
            isMoving = true;
            lastMoveDir = theVectRaw;
        }
        else
        {
            isMoving = false;
        }
        speed = Mathf.Lerp(speed, normSpeed * slowModif, .1f);
        thePlayer.velocity = theVect * speed;
    }

    public void ToDash()
    {
        currState = PlayerStates.dashing;    
    }

    public void ToAttack()
    {
        currState = PlayerStates.attacking;
    }
    
    void FixedUpdate()
    {
        switch (currState)
        {
            case PlayerStates.moving:
                if ((Mathf.Abs(theVect.x) > 0f) || (Mathf.Abs(theVect.y) > 0f))
                {
                    if (!theAudioManager.isPlaying("WalkTile 1"))
                    {
                        theAudioManager.Play("WalkTile 1");
                    }                    
                    thePlayerAnim.SetBool("isMoving", true);
                    thePlayerAnim.SetFloat("HorizontalMovement", theVect.x);
                    thePlayerAnim.SetFloat("VerticalMovement", theVect.y);
                }
                else
                {                    
                    thePlayerAnim.SetBool("isMoving", false);
                }                
                break;
            case PlayerStates.dashing:
                Dash();
                break;
            case PlayerStates.stunned:                
                isMoving = false;
                thePlayerAnim.SetBool("isMoving", false);
                //Debug.Log("Is Stunned");
                break;
            case PlayerStates.attacking:
                Debug.Log("Attacking");
                break;
            case PlayerStates.isDead:
                break;
        }
    }    

    private void Dash()
    {
        //movePosition = thePlayer.position + lastMoveDir * speedDodge;
        
        if (currState != PlayerStates.dashing)
        {
            currState = PlayerStates.dashing;
            theFXScript.MakeTheGhosts(true);            
            thePlayer.velocity = Vector2.zero;
            thePlayerAnim.SetTrigger("Dodge");

            //lastMovePosition = movePosition;

            ////!TODO
            //RaycastHit2D theCheckForObjs = Physics2D.Raycast(transform.position, movePosition.normalized, movePosition.magnitude, theWallLayer);

            //if(theCheckForObjs.collider != null)
            //{
            //    Debug.Log("The collider " + theCheckForObjs.collider.tag);
            //    tmpGameObj.position = theCheckForObjs.point;
            //    lastMovePosition = theCheckForObjs.point;
            //    Debug.Log("New Move Position is " + lastMovePosition);                
            //}
            ////thePlayer.MovePosition(lastMovePosition);
            
            thePlayer.AddForce(lastMoveDir * 1000 * speedDodge * Time.fixedDeltaTime);
            Debug.Log("Dodge has started!");
            
            if (!theAudioManager.isPlaying("DodgeWoosh"))
            {
                theAudioManager.Play("DodgeWoosh");
            }
            StartCoroutine(HasDodged());     
        }
    }

    private IEnumerator HasDodged()
    {
        int count = 0;
        print("thePlayer.velocity" + thePlayer.velocity);
        while (thePlayer.velocity.x > 0f && thePlayer.velocity.y > 0f)
        {
            count++;
            print("Dodge in proccess" + count);
            yield return null;
        }        
        currState = PlayerStates.moving;
        theFXScript.MakeTheGhosts(false);
        Debug.Log("Dodge is made!");
    }  

}
