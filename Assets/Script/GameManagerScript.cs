using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

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

    private int playerHealth;
    private int tmpHealth;
    private int maxPlayerHealth;
    private int minPlayerHealth;
    
    private int playerMoney;
    private int tmpMoney;

    private int inkLevel;
    private int tmpInkLevel;

    private Vector2 startPos;

    private bool menuActive;

    [SerializeField]
    [Range(0, 100)]
    private int inkLevelLoss;
    [SerializeField]
    float timeToDecreaseInk;
    bool isInInk = false;
    bool nearLetter = false;

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
        
        inkLevel = 100;
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


        theLetterUI = GameObject.FindGameObjectWithTag("LetterUI");
        theLetterScript = theLetterUI.GetComponent<ScriptForLetter>();
        theLetterText = theLetterUI.GetComponentsInChildren<Text>();

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
        MenuFunc();        
    }

    private void LetterFunc(object sender, EventArgs e)
    {
        if (!isReading)
        {

            Debug.Log("Letter is opening");
            isReading = true;
            theLetterScript.enabled = true;
            thePlayerScr.Stunned();

        }
        else
        {
            Debug.Log("Letter is closing");
            isReading = false;
            theLetterScript.enabled = false;
            thePlayerScr.Unstunned();
        }        
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
