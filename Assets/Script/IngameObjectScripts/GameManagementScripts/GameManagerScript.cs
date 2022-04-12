using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class GameManagerScript : MonoBehaviour
{
    [SerializeField]
    private DialogueScript DialogueHolder;
    private List <DialogueLine> theCurrLines;
    private Player thePlayerScr;


    private PlayerOtherInput AllOtherInput;

    public static GameManagerScript gmInstance;
    private event Action nullAction;
    private GameObject theLockObject;
        
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
    
    
    [SerializeField]
    private GameObject theMenu;

    private menuScr theMenuScript;

    private int playerHealth;
    private int tmpHealth;
    private int maxPlayerHealth;
    private int minPlayerHealth;
    
    private int playerMoney;
    private int tmpMoney;

    [SerializeField]
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

    private CharacterOverlap thePlayerOver;

    public event Action<int> OnHealthChange;
    //public event Action<int> On

    private IEnumerator inkCoroutine;

    private enum DeathTypes
    {
        afterFall,
        afterInk,
        afterHealthLoss,
        none
    }
    private DeathTypes currDeath;

    void Awake()
    {
        theCurrLines = new List<DialogueLine>();        

        theLetterUI = GameObject.FindGameObjectWithTag("LetterUI");
        theLetterScript = theLetterUI.GetComponent<ScriptForLetter>();
        theLetterText = theLetterUI.GetComponentsInChildren<Text>();

        theFadeInScr = GameObject.FindGameObjectWithTag("Fade").GetComponent<FadeInScript>();
        

        thePlayerOver = thePlayerObj.GetComponent<CharacterOverlap>();       

        destroyedObj = new List<SpawnObjects>();
        thePlayerScr = thePlayerObj.GetComponent<Player>();
        AllOtherInput = thePlayerObj.GetComponent<PlayerOtherInput>();
        inkCoroutine = InkStay();
        
        EnterAllActions();
        Normalize();
    }

    private void Start()
    {
        DialogueHolder.OnDialogueEnd += IfEndDialogue;
    }

    private void EnterAllActions()
    {
        thePlayerOver.OnNearLetter += IfLetterClose;
        thePlayerOver.OnFarLetter += IfLetterFar;

        thePlayerOver.OnFalling += changeHealthWhenFalling;

        thePlayerOver.OnEnteringInk += IfEnteredInk;
        thePlayerOver.OnEscapingInk += IfEscapedInk;

        thePlayerOver.OnSaving += IfSave;
        thePlayerOver.OnTakingCoin += IfTookCoin;

        thePlayerOver.OnDangerCollision += IfEnteredDanger;

        thePlayerOver.OnFountainIn += IfEnteredFountain;
        thePlayerOver.OnFountainOut += IfExitedFountain;

        thePlayerOver.OnNearLock += IfNearLock;
        thePlayerOver.OnFarLock += IfFarLock;

        thePlayerOver.OnDialogueEnter += IfEnteredDialogue;
        thePlayerOver.OnDialogueExit += IfExitedDialogue;

        thePlayerOver.OnEndOfLevel += IfEndOfLevel;
    }

    private void Normalize()
    {

        menuActive = false;
        startPos = thePlayerObj.transform.position;
        isItAfterFall = false;
        currDeath = DeathTypes.none;

        maxInklevel = 100;
        inkLevel = maxInklevel;
        tmpInkLevel = inkLevel;
        ChangeInkLevel(0);

        maxPlayerHealth = 8;
        minPlayerHealth = 0;

        tmpHealth = 4;
        playerHealth = tmpHealth;

        isReading = false;

        ChangeHealth(0);

        tmpMoney = 0;
        playerMoney = tmpMoney;
        ChangeMoney(0);
    }

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

    private void LockFunc()
    {
        thePlayerOver.Stunned();/*
        theLockScript.OnFail += NotOpenTheLock;
        theLockScript.OnSuccess += OpenTheLock;
        theLockScript.Activate();*/
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

    public void FountainFunc()
    {
        if (tmpInkLevel != maxInklevel)
        {
            tmpInkLevel = maxInklevel;
            ChangeInkLevel(0);
        }
        else
        {
            Debug.Log("You already have maxed out!");
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
        ChangeInkLevel(0);

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

    public void AddNewDestroyedObj(string theName, Vector2 theCurPos)
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

        if (changedHealth > minPlayerHealth && changedHealth <= maxPlayerHealth)
        {
            tmpHealth = changedHealth;
            if (theHealthHolder.transform.childCount - 1 < tmpHealth)
            {
                int heartsToAdd = (changedHealth - theHealthHolder.transform.childCount);

                for (int j = 0; j < heartsToAdd; j++)
                {                    
                    Instantiate(healthIcon, theHealthHolder.transform);
                }
            }
            else if (theHealthHolder.transform.childCount > tmpHealth)
            {
                
                int heartsToDestroy = theHealthHolder.transform.childCount - (theHealthHolder.transform.childCount - tmpHealth);
                int currHeartsCount = theHealthHolder.transform.childCount;
                for (int i = currHeartsCount - 1; i >= heartsToDestroy; i--)
                {
                    Destroy(theHealthHolder.transform.GetChild(i).gameObject);
                }

                if (currDeath == DeathTypes.afterFall)
                {
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

    public void ChangeInkLevel(int deltaInkLevel)
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
            AddNewDestroyedObj("Coin", posOftheCollidedObj);
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

    //To make actions
    private void IfEnteredFountain(object sender, EventArgs e)
    {
        Debug.Log("We are here");
        AllOtherInput.OnInteracting += FountainFunc;
    }

    private void IfExitedFountain(object sender, EventArgs e)
    {
        AllOtherInput.OnInteracting -= FountainFunc;
    }

    private void IfNearLock(GameObject theObj)
    {
        AllOtherInput.ClearAllInter();
        AllOtherInput.OnInteracting += LockFunc;
        theLockObject = theObj;
    }

    private void IfFarLock()
    {
        theLockObject = null;
        AllOtherInput.OnInteracting -= LockFunc;
    }
    private void IfEnteredDanger(object sender, EventArgs e)
    {
        Debug.Log("Entered danger");
        ChangeHealth(-1);
    }

    private void IfLetterClose(string sign, string contain)
    {
        theLetterText[0].text = sign;
        theLetterText[1].text = contain;
        AllOtherInput.OnInteracting += LetterFunc;
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
        AllOtherInput.OnInteracting -= LetterFunc;
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

    private void IfEnteredInk(object sender, EventArgs e)
    {
        StartCoroutine(inkCoroutine);
    }

    void IfEscapedInk(object sender, EventArgs e)
    {
        StopCoroutine(inkCoroutine);
    }

    private IEnumerator InkStay()
    {
        while (true)
        {
            Debug.Log("Ink level loss is " + inkLevelLoss);
            ChangeInkLevel(inkLevelLoss);
            yield return new WaitForSeconds(timeToDecreaseInk);
        }
    }

    private void IfEnteredDialogue(DialogueLine[] theLines)
    {
        foreach (DialogueLine line in theLines)
        {
            theCurrLines.Add(line);
        }        
        AllOtherInput.OnInteracting += ThrowDialogue;
    }

    private void ThrowDialogue()
    {
        AllOtherInput.OnInteracting -= ThrowDialogue;
        thePlayerOver.Stunned();
        Debug.Log("Throwing the Dialogue!");
        
        DialogueHolder.ToStartDialogue(theCurrLines);
        AllOtherInput.OnInteracting += DialogueHolder.NextLine;
    }

    private void IfEndDialogue()
    {
        AllOtherInput.OnInteracting += ThrowDialogue;
        thePlayerOver.Unstunned();
    }

    private void IfExitedDialogue()
    {
        AllOtherInput.OnInteracting -= ThrowDialogue;
        theCurrLines.Clear();
    }    
}
