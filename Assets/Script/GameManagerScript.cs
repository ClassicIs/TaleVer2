using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameManagerScript : MonoBehaviour
{
    public List<SpawnObjects> destroyedObj;

    [SerializeField]
    private GameObject theHealthHolder;
    [SerializeField]
    private GameObject healthIcon;
    private GameObject[] allHealth;

    [SerializeField] 
    private GameObject[] allGameObj;
    [SerializeField]
    private Slider inkLevelSlider;
    [SerializeField]
    private Text moneyCount;

    
    private Player thePlayerScr;
    [SerializeField]
    private GameObject theMenu;
    [SerializeField]
    private GameObject restartMenu;

    private int health;
    private int inkLevel;
    private int maxHealth;
    private int maxInkLevel;

    private int coinCount;

    [SerializeField]
    private GameObject thePlayerObj;
    [SerializeField]
    private Rigidbody2D thePlayerRB;
    [SerializeField]
    private CharacterOverlap charCollisionScript;
    [SerializeField]
    private BoxCollider2D theBoxCol;
    [SerializeField]
    private CircleCollider2D theCircleTriggerCol;
    [SerializeField]
    private Animator playerAnimator;

    private bool menuActive;

    // Start is called before the first frame update
    void Start()
    {
        normalizeGM();

        charCollisionScript = thePlayerObj.GetComponent<CharacterOverlap>();
        theBoxCol = thePlayerObj.GetComponent<BoxCollider2D>();
        theCircleTriggerCol = thePlayerObj.GetComponent<CircleCollider2D>();
        playerAnimator = thePlayerObj.GetComponent<Animator>();        
        thePlayerScr = thePlayerObj.GetComponent<Player>();
        thePlayerRB = thePlayerObj.GetComponent<Rigidbody2D>();

        destroyedObj = new List<SpawnObjects>();

        menuActive = false;
    }

    void normalizeGM()
    {
        maxHealth = 4;
        health = maxHealth;
        coinCount = 0;
        inkLevel = 0;
        changeInkLevel(inkLevel);
        ChangeHealth(health);
        ChangeMoney(coinCount);        
        charCollisionScript.InstadeathTime = 0f;
        theBoxCol.enabled = true;
        theCircleTriggerCol.enabled = true;
        thePlayerScr.enabled = true;
        charCollisionScript.enabled = true;
    }

    // Update is called once per frame
    void Update()
    {
        MenuFunc();
    }

    public void addNewDestroyedObj(string theName, Vector2 theCurPos)
    {
        
        SpawnObjects newSpawnObject = new SpawnObjects (theName, theCurPos);
        Debug.Log("Name of the object " + newSpawnObject.nameOfPrefab + "\nThe position of object is " + newSpawnObject.thePosition);
        
        destroyedObj.Add(newSpawnObject);
        Debug.Log(destroyedObj.Count);       
    }

    private void midRestart()
    {
        thePlayerObj.transform.position = thePlayerScr.lastMovePos;
        //he
    }

    public void RestartGame()
    {
        foreach (SpawnObjects theObjToSpawn in destroyedObj)
        {
            foreach (GameObject theObjectZero in allGameObj)
            {
                Debug.Log("theObjectZero.name" + theObjectZero.name + "\ntheObjToSpawn.nameOfPrefab: " + theObjToSpawn.nameOfPrefab);
                if (theObjectZero.name == theObjToSpawn.nameOfPrefab)
                {
                    Instantiate(theObjectZero, theObjToSpawn.thePosition, Quaternion.identity);
                    continue;
                }
            }
        }

        destroyedObj.Clear();
    }

    private void MenuFunc()
    {
        if (Input.GetKeyDown(KeyCode.Escape) && !menuActive)
        {
            MenuOn();
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && menuActive)
        {
            MenuOff();
        }
    }

    public void MenuOn()
    {
        thePlayerScr.enabled = false;
        Time.timeScale = 0f;
        menuActive = true;
        theMenu.SetActive(true);
    }
    public void MenuOff()
    {
        thePlayerScr.enabled = true;
        Time.timeScale = 1f;
        menuActive = false;
        theMenu.SetActive(false);
    }

   public void toChangeHealth(int healthToChange)
    {
        if ((health + healthToChange) <= maxHealth && (health + healthToChange) > 0)
        {
            health -= healthToChange;
            ChangeHealth(health);
        }
        else if((health + healthToChange) == 0)
        {
            theDeath();
            
        }
        else if((health + healthToChange) > maxHealth)
        {
            Debug.LogWarning("Health is at maximum value.");
        }
        else if((health + healthToChange) < 0)
        {
            Debug.LogWarning("Health is at minimum value.");
        }
    }

    public void theDeath()
    {
        //thePlayerRB.velocity = new Vector2(0, 0);
        playerAnimator.SetBool("isMoving", false);
        theBoxCol.enabled = false;
        theCircleTriggerCol.enabled = false;
        thePlayerScr.enabled = false;
        charCollisionScript.enabled = false;
        restartMenu.SetActive(true);
    }    

    public void ChangeHealth(int currHealth)
    {
        if (theHealthHolder.transform.childCount < currHealth)
        {
            int heartsToAdd = (currHealth - theHealthHolder.transform.childCount);
            Debug.Log("Health is less ");
            for (int j = 0; j < heartsToAdd; j++)
            {                
                Instantiate(healthIcon, theHealthHolder.transform);                
            }
        }        
        else
        {
            int heartsToDestroy = (theHealthHolder.transform.childCount - currHealth);
            int currHeartsCount = theHealthHolder.transform.childCount;
            Debug.Log("Health is more ");
            for (int i = currHeartsCount; i >= heartsToDestroy; i--)
            {
                Destroy(theHealthHolder.transform.GetChild(i));
            }
        }       
    }

    public void changeInkLevel(int newInk)
    {
        if ((inkLevel + newInk) < maxInkLevel)
        {
            float tmpInk = inkLevel;
            float howFast = 0.2f;
            inkLevel = inkLevel + newInk;
            inkLevelSlider.value = Mathf.Lerp(tmpInk, inkLevel, howFast);
        }
        else
        {
            //theInkDeath()
            Debug.Log("Ink level is full!");
            theDeath();
        }
        
    }

    public int GetCurrHealth()
    {
        return health;
    }

    public void ChangeMoney(int currMoney, GameObject theObj)
    {
        coinCount += currMoney;
        Destroy(theObj.gameObject);
        moneyCount.text = currMoney.ToString();
        addNewDestroyedObj("Coin", theObj.transform.position);
    }

    public void ChangeMoney(int currMoney)
    {
        moneyCount.text = currMoney.ToString();
    }
}
