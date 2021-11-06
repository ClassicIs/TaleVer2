using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManagerScript : MonoBehaviour
{
    public static GameManagerScript gmInstance;
    private event Action nullAction;

    [SerializeField]
    private LockCanvasScript theLockScript;
    private GameObject theLockObject;
    private CypherScript theCypherScr;
    private List<string> theInventory = new List<string>();

    public List<SpawnObjects> destroyedObj;


    [SerializeField]
    private GameObject theHealthHolder;
    [SerializeField]
    private GameObject healthIcon;
    private GameObject[] allHealth;

    [SerializeField] 
    private GameObject[] allGameObj;
    [SerializeField]
    private Slider inkSlider;
    [SerializeField]
    private Text moneyCount;
    [SerializeField]
    private GameObject theLetterUI;
    private ScriptForLetter theLetterScript;
    private Text[] theLetterText;

    [SerializeField]
    private GameObject thePlayerObj;
    private Player thePlayerScr;
    private CharacterOverlap thePlayerOver;
    [SerializeField]
    private GameObject theMenu;

    private menuScr theMenuScript;

    private int playerHealth;
    private int tmpHealth;
    private int maxPlayerHealth;
    private int minPlayerHealth;
    
    private int playerMoney;
    private int tmpMoney;

    private int inkLevel;
    private int tmpInkLevel;
    private int maxInklevel;

    private FadeInScript theFadeInScr;

    private Vector2 startPos;

    private bool menuActive;

    [SerializeField]
    [Range(0, 100)]
    private int inkLevelLoss;
    [SerializeField]
    float timeToDecreaseInk;
    bool isInInk = false;
    bool nearLetter = false;
    bool nearFountain = false;
    bool nearDanger = false;
    bool notEndOfLevel = true;

    private bool isItAfterFall;

    bool isReading;
    
    private IEnumerator inkCoroutine;

    private enum DeathTypes
    {
        afterFall,
        afterInk,
        afterHealthLoss,
        none
    }
    private DeathTypes currDeath;

    void Start()
    {
        currDeath = DeathTypes.none;
        inkLevelLoss = 1;

        maxInklevel = 100;
        inkLevel = maxInklevel;
        tmpInkLevel = inkLevel;
        changeInkLevel(0);


        timeToDecreaseInk = 3;

        inkCoroutine = inkStay();

        maxPlayerHealth = 8;
        minPlayerHealth = 0;
        
        tmpHealth = 4;
        playerHealth = tmpHealth;

        isReading = false;

        ChangeHealth(0);

        tmpMoney = 0;
        playerMoney = tmpMoney;
        ChangeMoney(0);

        theCypherScr = GameObject.FindObjectOfType<CypherScript>();
        //theCypherScr.Activate();

        theLetterUI = GameObject.FindGameObjectWithTag("LetterUI");
        theLetterScript = theLetterUI.GetComponent<ScriptForLetter>();
        theLetterText = theLetterUI.GetComponentsInChildren<Text>();

        theFadeInScr = GameObject.FindGameObjectWithTag("Fade").GetComponent<FadeInScript>();

        startPos = thePlayerObj.transform.position;

        thePlayerOver = thePlayerObj.GetComponent<CharacterOverlap>();

        thePlayerOver.OnNearLetter += IfLetterClose;
        thePlayerOver.OnFarLetter += IfLetterFar;
        
        thePlayerOver.OnFalling += changeHealthWhenFalling;
        isItAfterFall = false;

        thePlayerOver.OnEnteringInk += ifEnteredInk;
        thePlayerOver.OnEscapingInk += ifEscapedInk;

        thePlayerOver.OnSaving += IfSave;
        thePlayerOver.OnTakingCoin += IfTookCoin;

        thePlayerOver.OnDangerCollision += IfEnteredDanger;

        thePlayerOver.OnFountainIn += IfEnteredFountain;
        thePlayerOver.OnFountainOut += IfExitedFountain;

        thePlayerOver.OnNearLock += IfNearLock;
        thePlayerOver.OnFarLock += IfFarLock;

        thePlayerOver.OnEndOfLevel += IfEndOfLevel;

        destroyedObj = new List<SpawnObjects>();
        thePlayerScr = thePlayerObj.GetComponent<Player>();        
        menuActive = false;
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            ChangeHealth(-1);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            ChangeHealth(1);
        }
        if (notEndOfLevel)
        {
            MenuFunc();
        }

        if (Input.GetKeyDown(KeyCode.P))
        {
            if (thePlayerScr.currState != Player.PlayerStates.stunned)
            {
                Debug.Log("P Stunned");
                thePlayerOver.Stunned();
                //thePlayerScr.currState = Player.PlayerStates.stunned;
            }
            else
            {
                Debug.Log("P Untunned");
                thePlayerOver.Unstunned();
                //thePlayerScr.currState = Player.PlayerStates.moving;
            }
        }
        
    }

    private void LetterFunc()
    {
        if (!isReading)
        {
            Debug.Log("Letter is opening");
            isReading = true;
            theLetterScript.enabled = true;
            thePlayerOver.Stunned();

        }
        else
        {
            Debug.Log("Letter is closing");
            isReading = false;
            theLetterScript.enabled = false;
            thePlayerOver.Unstunned();
        }        
    }

    private void IfEnteredFountain(object sender, EventArgs e)
    {
        Debug.Log("We are here");
        thePlayerScr.OnInteracting += FountainFunc;       
    }

    private void IfExitedFountain(object sender, EventArgs e)
    {
        thePlayerScr.OnInteracting -= FountainFunc;
    }

    private void IfNearLock(GameObject theObj)
    {
        thePlayerScr.ClearAllInter();
        thePlayerScr.OnInteracting += LockFunc;
        theLockObject = theObj;
    }

    private void IfFarLock()
    {
        theLockObject = null;
        thePlayerScr.OnInteracting -= LockFunc;
    }

    private void LockFunc()
    {
        thePlayerOver.Stunned();
        theLockScript.OnFail += NotOpenTheLock;
        theLockScript.OnSuccess += OpenTheLock;
        theLockScript.Activate();
    }

    private void OpenTheLock()
    {
        BoxScript theBoxScript = theLockObject.GetComponent<BoxScript>();
        theBoxScript.OpenTheBox();
        foreach (string theItem in theBoxScript.contentOfBox)
        {            
            theInventory.Add(theItem);
        }

        foreach (string theItemObj in theInventory)
        {
            Debug.Log(theItemObj);
        }
        thePlayerOver.Unstunned();
    }

    private void NotOpenTheLock()
    {
        thePlayerOver.Unstunned();
        Debug.Log("Box not openned!");
    }

    


    private void FountainFunc()
    {
        if (tmpInkLevel != maxInklevel)
        {
            tmpInkLevel = maxInklevel;
            changeInkLevel(0);
        }
        else
        {
            Debug.Log("You already have maxed out!");
        }
    }

    private void IfEnteredDanger(object sender, EventArgs e)
    {        
        Debug.Log("Entered danger");
        ChangeHealth(-1);
    }

    private void IfLetterClose (string sign, string contain)
    {
        theLetterText[0].text = sign;
        theLetterText[1].text = contain;
        thePlayerScr.OnInteracting += LetterFunc;
        Debug.Log("thePlayerScr.OnInteracting plus");
        nearLetter = true;
    }

    private void IfLetterFar(object sender, EventArgs e)
    {
        for (int i = 0; i < theLetterText.Length; i++)
        {
            theLetterText[i].text = "";
        }
        nearLetter = false;
        Debug.Log("thePlayerScr.OnInteracting minus");
        thePlayerScr.OnInteracting -= LetterFunc;
    }

    private void IfSave(object sender, EventArgs e)
    {
        playerHealth = tmpHealth;
        playerMoney = tmpMoney;
        inkLevel = tmpInkLevel;
        destroyedObj.Clear();
    }

    private void IfEndOfLevel(object sender, EventArgs e)
    {
        IfSave(sender, e);
        StartCoroutine(theFadeInScr.toFadeInCoroutine(false));
        MenuOn(true, "To be continued...");
        
    }

    private void IfTookCoin(int money, Vector2 pos)
    {
        ChangeMoney(money, pos);
    }

    private void ifEnteredInk(object sender, EventArgs e)
    {
        StartCoroutine(inkCoroutine);
    }

    void ifEscapedInk(object sender, EventArgs e)
    {        
        StopCoroutine(inkCoroutine);
    }

    private IEnumerator inkStay()
    {
        while(true)
        {
            changeInkLevel(inkLevelLoss);
            yield return new WaitForSeconds(timeToDecreaseInk);
        }
    }    

    private void changeHealthWhenFalling(object sender, EventArgs e)
    {
        currDeath = DeathTypes.afterFall;
        ChangeHealth(-1);        
    }

    private void RestartTheGame()
    {
        Debug.Log("Restarting!");        
        SetAllObjectsBack();
       
        tmpMoney = playerMoney;
        ChangeMoney(0);
        tmpHealth = playerHealth;

        Debug.Log("Tmp health is " + tmpHealth);
        ChangeHealth(0);
        tmpInkLevel = inkLevel;
        changeInkLevel(0);

        switch (currDeath)
        {
            case DeathTypes.afterFall:
                Debug.Log("Choosed AfterFall to restart");
                thePlayerOver.SetPlayerTo(false);
                break;
            case DeathTypes.afterHealthLoss:
                thePlayerOver.SetPlayerTo(true);
                break;
            case DeathTypes.afterInk:
                thePlayerOver.SetPlayerTo(true);
                thePlayerOver.InkDeath(false);
                break;
        }
        currDeath = DeathTypes.none;
    }

    public void addNewDestroyedObj(string theName, Vector2 theCurPos)
    {        
        SpawnObjects newSpawnObject = new SpawnObjects (theName, theCurPos);
        Debug.Log("Name of the object " + newSpawnObject.nameOfPrefab + "\nThe position of object is " + newSpawnObject.thePosition);        
        destroyedObj.Add(newSpawnObject);
        Debug.Log(destroyedObj.Count);       
    }

    public void SetAllObjectsBack()
    {
        Debug.Log("Restarting the game!");
        Debug.Log("deactiveObjs.Count " + destroyedObj.Count);

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
            MenuOn(true);
        }
        else if (Input.GetKeyDown(KeyCode.Escape) && menuActive)
        {
            MenuOn(false);
        }
    }

    public void MenuOn(bool onOrOff, string theString = "")
    {        
        if (onOrOff)
        {
            if(theString != "")
            {
                theMenu.GetComponentInChildren<Text>().text = theString;
                theMenu.GetComponentInChildren<Image>().enabled = false;
            }
            thePlayerScr.enabled = false;
            thePlayerOver.Stunned();
            //Time.timeScale = 0f;
            menuActive = true;
            theMenu.SetActive(true);
        }
        else
        {
            thePlayerOver.Unstunned();
            thePlayerScr.enabled = true;
            //Time.timeScale = 1f;
            menuActive = false;
            theMenu.SetActive(false);
        }
    }

    public void ChangeHealth(int addOrDelHealth)
    {
        int changedHealth = tmpHealth + addOrDelHealth;

        Debug.Log("playerHealth: " + tmpHealth);
        Debug.Log("changedHealth " + changedHealth);
        Debug.Log("theHealthHolder.transform.childCount " + theHealthHolder.transform.childCount);

        if (changedHealth > minPlayerHealth && changedHealth <= maxPlayerHealth)
        {
            tmpHealth = changedHealth;
            if (theHealthHolder.transform.childCount - 1 < tmpHealth)
            {
                Debug.Log("Add health ");
                int heartsToAdd = (changedHealth - theHealthHolder.transform.childCount);

                for (int j = 0; j < heartsToAdd; j++)
                {
                    Debug.Log("(currHealth - theHealthHolder.transform.childCount): " + (tmpHealth - theHealthHolder.transform.childCount));
                    Instantiate(healthIcon, theHealthHolder.transform);
                }
            }
            else if (theHealthHolder.transform.childCount > tmpHealth)
            {
                
                int heartsToDestroy = theHealthHolder.transform.childCount - (theHealthHolder.transform.childCount - tmpHealth);
                int currHeartsCount = theHealthHolder.transform.childCount;
                Debug.Log("Need to delete health ");
                Debug.Log("currHeartsCount is " + currHeartsCount);
                Debug.Log("heartsToDestroy is " + heartsToDestroy);

                for (int i = currHeartsCount - 1; i >= heartsToDestroy; i--)
                {
                    Destroy(theHealthHolder.transform.GetChild(i).gameObject);
                }

                if (currDeath == DeathTypes.afterFall)
                {
                    Debug.Log("It's time to choose deathAfterFall");
                    DeathChooser();
                }
            }
            else
            {
                Debug.Log("Health is already of that value " + changedHealth);
            }
        }
        else if (changedHealth == minPlayerHealth)
        {            
            currDeath = DeathTypes.afterHealthLoss;
            tmpHealth = changedHealth;
            Destroy(theHealthHolder.transform.GetChild(0).gameObject);
            Debug.Log("Instantiate theDeath");
            DeathChooser();                        
        }
        else
        {
            Debug.Log("Health is out of boundaries");
        }
    }

    private void DeathChooser()
    {
        switch(currDeath)
        {
            case DeathTypes.afterFall:
                Debug.Log("Choosed AfterFall");
                thePlayerOver.InterDeath();                
                break;
            case DeathTypes.afterHealthLoss:
                thePlayerOver.allInDeath();
                break;
            case DeathTypes.afterInk:
                StopCoroutine(inkCoroutine);
                thePlayerOver.InkDeath(true);                
                break;
            case DeathTypes.none:
                break;
        }
        StartCoroutine(waitToRestart());
    }
    public IEnumerator waitToRestart()
    {
        yield return new WaitForSeconds(5);        
        RestartTheGame();
    }

    public void changeInkLevel(int deltaInkLevel)
    {
        int tmpInk = tmpInkLevel - deltaInkLevel;
        if (tmpInk > 0)
        {
            tmpInkLevel -= deltaInkLevel;            
        }
        else
        {
            tmpInkLevel = 0;
            Debug.LogWarning("Ink level is out of bounds!");
        }
        inkSlider.value = tmpInkLevel;
        if (tmpInkLevel == 0)
        {
            currDeath = DeathTypes.afterInk;
            DeathChooser();
        }
    }

    public void ChangeMoney(int addMoney, Vector2 posOftheCollidedObj)
    {
        if (addMoney > 0)
        {
            tmpMoney = tmpMoney + addMoney; 
            moneyCount.text = tmpMoney.ToString();
            addNewDestroyedObj("Coin", posOftheCollidedObj);
        }
    }
    public void ChangeMoney(int justMoney)
    {
        if (justMoney > 0)
        {
            tmpMoney = justMoney;
            moneyCount.text = tmpMoney.ToString();
        }
    }
}
