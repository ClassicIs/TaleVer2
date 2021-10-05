using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private Rigidbody2D thePlayer;
    private Animator thePlayerAnim;
    private PlayerManager thePlayerManager;
    private PlayerEffectsScript theFXScript;

    public bool isMoving;
    private AudioManagerScript theAudioManager;
    [SerializeField]
    public bool isRestarting;  
    private Vector2 lastMovePosition;    
    private Vector2 lastMoveDir;
    public Vector2 theVectRaw;
    public Vector2 lastMovePos;

    private int dodgeCount;
    [SerializeField]
    public bool isDodging;
    private float speedDodge;   
    
    private float speed;    
    private float normSpeed;
    [HideInInspector]
    public float slowModif;
    public bool canWalk;    
    private bool isGrounded;


    // Start is called before the first frame update
    void Start()
    {
        canWalk = true;
        theAudioManager = FindObjectOfType<AudioManagerScript>();
        thePlayerManager = GetComponent<PlayerManager>();
        theFXScript = GetComponent<PlayerEffectsScript>();

        isDodging = false;

        isRestarting = false;         

        //Speed variables        
        normSpeed = 1.5f;
        
        speed = normSpeed;
        slowModif = 1f; //Modifikator for going in ink        

        //For dodge
        speedDodge = 2.6f; //Speed of dodge
        dodgeCount = 3;
        
        lastMovePosition = new Vector2(0, 0);
        //To check for last movement vector
        lastMoveDir = new Vector2(0, 0);
        StartCoroutine(checkLastPos());
           
    }

    private void Awake()
    {
        theVectRaw = new Vector2(0, 0);
        //Taking all variables from game object
        thePlayer = GetComponent<Rigidbody2D>();
        thePlayerAnim = GetComponent<Animator>();
    }

    public void NormalizeAll()
    {                     
        isRestarting = false;        
        thePlayerManager.NormalizeAll();
        StartCoroutine(checkLastPos());
    }

    // Update is called once per frame
    void Update()
    {        
        CheckMovement();           
    }

    void FixedUpdate()
    {
        if (!isRestarting)
        {
            if (canWalk)
            {
                if (isMoving)
                {
                    //Debug.Log("We are here!");
                    forMovement();
                }
                
            }            
            toDodge();
        }
        thePlayerAnim.SetBool("isMoving", isMoving);
    } 

    IEnumerator checkLastPos()
    {
        while (!isRestarting)
        {
            if (canWalk)
            {
                lastMovePos = transform.position;
            }
            yield return new WaitForSeconds(1);
        }
    }
    
    private void CheckMovement()
    {        
        theVectRaw = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical")).normalized;        

        if((Mathf.Abs(theVectRaw.x) > 0f) || (Mathf.Abs(theVectRaw.y) > 0f))
        {
            lastMoveDir = theVectRaw;
            isMoving = true;            
        }
        else
        {
            thePlayer.velocity = Vector2.zero;
            isMoving = false;                  
        }
        
    }    

    private void forMovement()
    {
        if (theAudioManager != null)
        {
            if (!theAudioManager.isPlaying("WalkTile 1"))
            {
                theAudioManager.Play("WalkTile 1");
            }
        }
        thePlayerAnim.SetFloat("HorizontalMovement", theVectRaw.x);
        thePlayerAnim.SetFloat("VerticalMovement", theVectRaw.y);
        thePlayer.velocity = theVectRaw * speed;        
    }

    private void toDodge()
    {
        Vector2 movePosition = thePlayer.position + lastMoveDir * speedDodge;
        
        if (Input.GetKeyDown(KeyCode.Space) && !isDodging)
        {
            canWalk = false;
            isDodging = true;
            //theFXScript.MakeTheGhosts(thePlayer.position, movePosition);
            theFXScript.makeTheGhost = true;
            thePlayer.velocity = new Vector2(0, 0);
            lastMovePosition = movePosition;

            //!TODO
            RaycastHit2D theCheckForObjs = Physics2D.Raycast(transform.position, new Vector2(transform.position.x, transform.position.y) + movePosition);
            if(theCheckForObjs.collider.tag == "Walls" && theCheckForObjs.collider.tag == "Enemy")
            {
                movePosition = theCheckForObjs.collider.gameObject.transform.position;
                Debug.Log("New Move Position is " + movePosition);
            }
            
            thePlayer.MovePosition(movePosition);
            Debug.Log("Dodge has started!");
            
            if (!theAudioManager.isPlaying("DodgeWoosh"))
            {
                theAudioManager.Play("DodgeWoosh");
            }
            StartCoroutine(hasDodged(movePosition));            
        }
    }

    private IEnumerator hasDodged(Vector2 lastMPos)
    {
        int counter1 = 0;
        
        while (thePlayer.position != lastMPos)
        {
            counter1++;
            Debug.Log("Dodge is in process!" + counter1);
            yield return null;
        }
        isDodging = false;
        canWalk = true;
        //theFXScript.makeTheGhost = false;
        Debug.Log("Dodge is made!");
    }


    private void OnDrawGizmos()
    {
        //Vector3 theVectNew = Vector3.right;
        //Gizmos.color
        /*if (lastMPos != null)
        {
            Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + lastMPos);
        }*/
        //Gizmos.DrawLine(transform.position, new Vector2(transform.position.x, transform.position.y) + theVectRaw);

        
    }


}
