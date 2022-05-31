using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Player : AliveBeeing
{   
    private Rigidbody2D thePlayer;
    private Animator thePlayerAnim;
    private PlayerEffectsScript theFXScript;
    public float horMovement;
    public float vertMovement;

    public event Action OnDashStart;
    public event Action OnDashEnd;

    public bool isGrounded;
    public bool isAlive;
    public bool isDashing;

    private AudioManagerScript theAudioManager;
    public LayerMask theWallLayer;
    public Transform tmpGameObj;
    public Vector2 theVectRaw;
    private Vector2 theVect;

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

    public bool isSlowDown;


    // Start is called before the first frame update
    void Start()
    {
        isGrounded = true;
        isAlive = true;
        isDashing = false;

        currState = PlayerStates.moving;
        movePosition = transform.position;
        
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
        speedDodge = 5f; //Speed of dodge
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

    public void SlowEffectOn(bool on, float SlowModifier = 0.4f)
    {
        isSlowDown = on;
        if (on)
        {
            slowModif = SlowModifier;
        }
        else
        {
            slowModif = 1f;

        }
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
    
    void FixedUpdate()
    {
        switch (currState)
        {
            case PlayerStates.moving:
                Move(horMovement, vertMovement);
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
                Debug.Log("Dashing AAAAAAAAAAAAAAAA!");

                if (!isDashing)
                {
                    Debug.Log("Dashing!");

                    Dash();
                }
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


    public void ChangeState(PlayerStates theState)
    {
        if (theState != currState)
        {
            if (theState != PlayerStates.stunned && isGrounded && isAlive)
            {
                Debug.Log("Changing state to " + theState.ToString());
                if (theState == PlayerStates.moving)
                {
                    if (isGrounded && currState != PlayerStates.dashing)
                    {
                        currState = theState;
                    }
                }
                else if (theState == PlayerStates.dashing)
                {

                    currState = theState;
                }
                else if (theState == PlayerStates.attacking && currState != PlayerStates.dashing)
                {
                    if (currState != PlayerStates.dashing)
                    {

                        currState = theState;
                    }
                }
            }
        }
    }

    public void ToStun(bool stun)
    {
        if (stun)
        {
            if (currState != PlayerStates.stunned)
            {
                currState = PlayerStates.stunned;
            }
        }
        else
        {
            if (currState == PlayerStates.stunned)
            {
                currState = PlayerStates.moving;
            }
        }
    }

    private void Dash()
    {
        Debug.Log("Dodge has started!");
        isDashing = true;
        movePosition = new Vector2(transform.position.x, transform.position.y) + lastMoveDir * speedDodge;
       
        theFXScript.MakeTheGhosts(true);            
        thePlayer.velocity = Vector2.zero;
        thePlayerAnim.SetTrigger("Dodge");

        Vector3 lastMovePosition = movePosition;

        RaycastHit2D theCheckForObjs = Physics2D.Raycast(transform.position, lastMoveDir, (lastMoveDir * speedDodge).magnitude, theWallLayer);
        Instantiate(tmpGameObj, theCheckForObjs.point, Quaternion.identity);
        Debug.DrawLine(transform.position, movePosition.normalized * movePosition.magnitude, Color.red, 1000f);
        if (theCheckForObjs.collider != null)
        {
            lastMovePosition = theCheckForObjs.point;
        }
        thePlayer.MovePosition(lastMovePosition);

        if (theAudioManager != null)
        {
            if (!theAudioManager.isPlaying("DodgeWoosh"))
            {
                theAudioManager.Play("DodgeWoosh");
            }
        }
        StartCoroutine(HasDodged(lastMovePosition));     
        
    }

    private IEnumerator HasDodged(Vector3 needPos)
    {
        int count = 0;

        while ((transform.position - needPos).sqrMagnitude <= 0.2f)
        {
            count++;
            print("Dodge in proccess" + count);
            if(count > 100)
            {
                break;
            }
            yield return null;
        }
        transform.position = needPos;
        currState = PlayerStates.moving;
        theFXScript.MakeTheGhosts(false);
        isDashing = false;
        Debug.Log("Dodge is made!");
    }  

}
