using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D thePlayer;

    public bool goingToDie;
    
    private Animator thePlayerAnim;
    private PlayerManager thePlayerManager;
    private PlayerEffectsScript theFXScript;

    public LayerMask theWallLayer;
    
    public Transform tmpGameObj;
    public bool isMoving;
    private AudioManagerScript theAudioManager;    
    public bool isRestarting;  
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
        movePosition = transform.position;
        lastMovePosition = new Vector2(0, 0);
        //To check for last movement vector
        lastMoveDir = new Vector2(0, 0);

        theVectRaw = new Vector2(0, 0);
        canWalk = true;
        theAudioManager = FindObjectOfType<AudioManagerScript>();
        thePlayerManager = GetComponent<PlayerManager>();
        theFXScript = GetComponent<PlayerEffectsScript>();

        isSliding = false;
        strTimeToFall = 3f;
        currTimeToFall = strTimeToFall;
        isTryingToDie = false;
        isDodging = false;

        isRestarting = false;
        
        goingToDie = false;      

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

    public void NormalizeAll()
    {                     
        isRestarting = true;
        currTimeToFall = 3f;
        thePlayerManager.NormalizeAll();
    }

    // Update is called once per frame
    void Update()
    {        
        CheckMovement();           
    }

    void FixedUpdate()
    {
        if (!isDodging)
        {
            if (canWalk)
            {
                if ((Mathf.Abs(theVect.x) > 0f) || (Mathf.Abs(theVect.y) > 0f))
                {
                    if (!theAudioManager.isPlaying("WalkTile 1"))
                    {
                        theAudioManager.Play("WalkTile 1");
                    }
                    isMoving = true;
                    thePlayerAnim.SetBool("isMoving", true);
                    thePlayerAnim.SetFloat("HorizontalMovement", theVect.x);
                    thePlayerAnim.SetFloat("VerticalMovement", theVect.y);
                }
                else
                {
                    isMoving = false;
                    thePlayerAnim.SetBool("isMoving", false);
                }

                forMovement();
            }
            toDodge();
        }
    } 
    
    private void CheckMovement()
    {        
        theVectRaw = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;
        if(goingToDie)
        {
            Debug.Log("Going to die!");
            horMovement = 0f;
            verMovement = 0f;
            currTimeToFall -= Time.deltaTime;
            isTryingToDie = true;
        }
        else
        {
            horMovement = Input.GetAxisRaw("Horizontal");
            verMovement = Input.GetAxisRaw("Vertical");
            currTimeToFall = strTimeToFall;
        }

        theVect = new Vector2(horMovement, verMovement).normalized;
        

        if (Mathf.Abs(theVectRaw.x) > 0.1 || Mathf.Abs(theVectRaw.y) > 0.1)
        {
            lastMoveDir = theVectRaw;
        }

        speed = Mathf.Lerp(speed, normSpeed * slowModif, .1f);        
    }    


    private void forMovement()
    {
        if (!isDodging)
        {
            thePlayer.velocity = theVect * speed;
        }
    }

    private void toDodge()
    {
        //movePosition = thePlayer.position + lastMoveDir * speedDodge;
        
        if (Input.GetKeyDown(KeyCode.Space) && !isDodging)
        {
            thePlayerAnim.SetTrigger("Dodge");
            isDodging = true;
            theFXScript.MakeTheGhosts(true);            
            thePlayer.velocity = new Vector2(0, 0);
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
            thePlayer.AddForce(lastMoveDir * 1000 * speedDodge);
            Debug.Log("Dodge has started!");
            
            if (!theAudioManager.isPlaying("DodgeWoosh"))
            {
                theAudioManager.Play("DodgeWoosh");
            }
            StartCoroutine(hasDodged());     
        }
    }

    private IEnumerator hasDodged()
    {
        
        while (thePlayer.velocity.x > 0.1 && thePlayer.velocity.y > 0.1f)
        {
            yield return null;
        }
        isDodging = false;
        theFXScript.MakeTheGhosts(false);
        Debug.Log("Dodge is made!");
    }


    private void OnDrawGizmos()
    {
        Gizmos.color = new Color(1, 0, 0);
        //Gizmos.DrawLine(transform.position, transform.position + new Vector3(theVectRaw.x * 0.5f, theVectRaw.y * 0.5f, 0f));

        Gizmos.DrawLine(transform.position, movePosition);
        
        //Gizmos.DrawWireCube(transform.position, transform.position + new Vector3(theVectRaw.x, theVectRaw.y, 0f));
        //Vector3 theVectNew = Vector3.right;
        //Gizmos.color
        /*if (lastMPos != null)
        {
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + lastMPos);
        }*/
        //Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + theVectRaw);


    }


}
