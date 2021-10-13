using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class Player : MonoBehaviour
{
    public enum PlayerStates
    {
        moving,
        dashing,
        attacking,
        stunned
    }
    
    public PlayerStates currState;

    private Rigidbody2D thePlayer;
    private Animator thePlayerAnim;
    private PlayerEffectsScript theFXScript;
    private AudioManagerScript theAudioManager;

    public LayerMask theWallLayer;
    
    public Transform tmpGameObj;
    public bool isMoving;
    
    private Vector2 lastMovePosition;
    private Vector2 theVect;
    [SerializeField]
    private Vector2 lastMoveDir;
    public Vector2 theVectRaw;
    [SerializeField]
    Vector2 movePosition;

    public float dodge;
    [SerializeField]
    public bool isDodging;
    private float startDodgeTime;
    [SerializeField]
    private float speedDodge;   
    
    public float speed;    
    private float normSpeed;
    public float slowModif;

    public bool canWalk;

    private float horMovement;
    private float verMovement;
    private bool isTryingToDie;   

    private float currTimeToFall;
    [SerializeField] 
    private float strTimeToFall;

    
    public bool isGrounded;
    public bool isSliding;

    // Start is called before the first frame update
    void Start()
    {
        currState = PlayerStates.moving;
        movePosition = transform.position;
        lastMovePosition = new Vector2(0, 0);
        //To check for last movement vector
        lastMoveDir = new Vector2(0, 0);

        theVectRaw = new Vector2(0, 0);
        theAudioManager = FindObjectOfType<AudioManagerScript>();   
        theFXScript = GetComponent<PlayerEffectsScript>();
        isDodging = false;
        
        //Speed variables        
        normSpeed = 1.5f;        
        speed = normSpeed;
        slowModif = 1f; //Modifikator for going in ink        

        //For dodge
        speedDodge = 2.6f; //Speed of dodge
        startDodgeTime = 1f;
        dodge = 0;             
    }

    private void Awake()
    {
        //Taking all variables from game object
        thePlayer = GetComponent<Rigidbody2D>();
        thePlayerAnim = GetComponent<Animator>();
    }

    void Update()
    {
        CheckMovement();
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
                forMovement();
                break;
            case PlayerStates.dashing:
                toDodge();
                break;
            case PlayerStates.stunned:
                thePlayerAnim.SetBool("isMoving", false);
                Debug.Log("Is Stunned");
                break;
            case PlayerStates.attacking:
                Debug.Log("Attacking");
                break;
        }
    }
    
    private void Stunned()
    {
        currState = PlayerStates.stunned;
    }
    private void Unstunned()
    {
        currState = PlayerStates.moving;
    }

    private void CheckMovement()
    {
        horMovement = Input.GetAxisRaw("Horizontal");
        verMovement = Input.GetAxisRaw("Vertical");
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
        if (currState != PlayerStates.stunned)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                currState = PlayerStates.dashing;
            }

            if (Input.GetKeyDown(KeyCode.G))
            {
                currState = PlayerStates.attacking;
            }
        }
    }    

    private void forMovement()
    {
        thePlayer.velocity = theVect * speed;
    }

    private void toDodge()
    {
        //movePosition = thePlayer.position + lastMoveDir * speedDodge;
        
        if (!isDodging)
        {            
            isDodging = true;
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
        isDodging = false;
        currState = PlayerStates.moving;
        theFXScript.MakeTheGhosts(false);
        Debug.Log("Dodge is made!");
    }

    private void OnDrawGizmos()
    {
        //Gizmos.color = new Color(1, 0, 0);
        //Gizmos.DrawLine(transform.position, transform.position + new Vector3(theVectRaw.x * 0.5f, theVectRaw.y * 0.5f, 0f));

        //Gizmos.DrawLine(transform.position, movePosition);       


    }


}
