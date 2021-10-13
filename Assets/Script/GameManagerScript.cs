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
    
    private IEnumerator inkCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        inkLevelLoss = 1;
        inkLevel = 100;
        timeToDecreaseInk = 3;
        inkCoroutine = inkStay();

        maxPlayerHealth = 8;
        minPlayerHealth = 0;
        playerHealth = 4;//maxPlayerHealth;
        ChangeHealth(0);

        playerMoney = 0;
        ChangeMoney(playerMoney);

        startPos = thePlayerObj.transform.position;

        thePlayerOver = thePlayerObj.GetComponent<CharacterOverlap>();
        thePlayerOver.OnFalling += changeHealthWhenFalling;
        thePlayerOver.OnEnteringInk += ifEnteredInk;
        thePlayerOver.OnEscapingInk += ifEscapedInk;
        thePlayerOver.OnSaving += IfSave;
        destroyedObj = new List<SpawnObjects>();
        thePlayerScr = thePlayerObj.GetComponent<Player>();        
        menuActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.H))
        {
            ChangeHealth(- 1);
        }
        else if (Input.GetKeyDown(KeyCode.J))
        {
            ChangeHealth(1);
        }
        MenuFunc();
    }

    private void IfSave(object sender, EventArgs e)
    {
        playerHealth = tmpHealth;
        playerMoney = tmpMoney;
        inkLevel = tmpInkLevel;
        destroyedObj.Clear();
    }

    private void IfTookCoin(object sender, EventArgs e)
    {
        ChangeMoney(1);
    }

    private void ifEnteredInk(object sender, EventArgs e)
    {
        StartCoroutine(inkCoroutine);
    }

    void ifEscapedInk(object sender, EventArgs e)
    {
        Debug.Log("Escaped ink!");
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
        ChangeHealth(-1);        
    }

    private void restartTheGame()
    {
        thePlayerObj.transform.position = startPos;
        thePlayerScr.NormalizeAll();
        tmpMoney = playerMoney;
        ChangeMoney(0);
        tmpHealth = playerHealth;
        ChangeHealth(0);
        tmpInkLevel = inkLevel;
        changeInkLevel(0);
    }

    public void addNewDestroyedObj(string theName, Vector2 theCurPos)
    {
        
        SpawnObjects newSpawnObject = new SpawnObjects (theName, theCurPos);
        Debug.Log("Name of the object " + newSpawnObject.nameOfPrefab + "\nThe position of object is " + newSpawnObject.thePosition);
        
        destroyedObj.Add(newSpawnObject);
        Debug.Log(destroyedObj.Count);       
    }

    public void RestartGame()
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

    public void ChangeHealth(int addOrDelHealth, bool afterFall)
    {        
        int changedHealth = playerHealth + addOrDelHealth;

        Debug.Log("playerHealth: " + playerHealth);
        Debug.Log("changedHealth " + changedHealth);
        Debug.Log("theHealthHolder.transform.childCount " + theHealthHolder.transform.childCount);

        if (changedHealth > minPlayerHealth && changedHealth <= maxPlayerHealth)
        {
            playerHealth = changedHealth;
            if (theHealthHolder.transform.childCount - 1 < playerHealth)
            {
                Debug.Log("Add health ");
                int heartsToAdd = (changedHealth - theHealthHolder.transform.childCount);

                for (int j = 0; j < heartsToAdd; j++)
                {
                    Debug.Log("(currHealth - theHealthHolder.transform.childCount): " + (playerHealth - theHealthHolder.transform.childCount));
                    Instantiate(healthIcon, theHealthHolder.transform);
                }
            }
            else if (theHealthHolder.transform.childCount > playerHealth)
            {
                int heartsToDestroy = theHealthHolder.transform.childCount - (theHealthHolder.transform.childCount - playerHealth);
                int currHeartsCount = theHealthHolder.transform.childCount;
                Debug.Log("Need to delete health ");
                Debug.Log("currHeartsCount is " + currHeartsCount);
                Debug.Log("heartsToDestroy is " + heartsToDestroy);
                
                for (int i = currHeartsCount - 1; i >= heartsToDestroy; i--)
                {
                    Destroy(theHealthHolder.transform.GetChild(i).gameObject);
                }
                if(afterFall)
                {
                    thePlayerOver.NearlyDeath();
                }
            }
            else
            {
                Debug.Log("Health is already of that value " + changedHealth);
            }
        }
        else if(changedHealth == minPlayerHealth)
        {            
            playerHealth = changedHealth;
            Destroy(theHealthHolder.transform.GetChild(0).gameObject);
            Debug.Log("Instantiate theDeath");
            restartTheGame();
        }
        else
        {
            Debug.Log("Health is out of boundaries");
        }
    }

    public void ChangeHealth(int addOrDelHealth)
    {
        int changedHealth = playerHealth + addOrDelHealth;

        Debug.Log("playerHealth: " + playerHealth);
        Debug.Log("changedHealth " + changedHealth);
        Debug.Log("theHealthHolder.transform.childCount " + theHealthHolder.transform.childCount);

        if (changedHealth > minPlayerHealth && changedHealth <= maxPlayerHealth)
        {
            tmpHealth = changedHealth;
            if (theHealthHolder.transform.childCount - 1 < playerHealth)
            {
                Debug.Log("Add health ");
                int heartsToAdd = (changedHealth - theHealthHolder.transform.childCount);

                for (int j = 0; j < heartsToAdd; j++)
                {
                    Debug.Log("(currHealth - theHealthHolder.transform.childCount): " + (playerHealth - theHealthHolder.transform.childCount));
                    Instantiate(healthIcon, theHealthHolder.transform);
                }
            }
            else if (theHealthHolder.transform.childCount > playerHealth)
            {
                int heartsToDestroy = theHealthHolder.transform.childCount - (theHealthHolder.transform.childCount - playerHealth);
                int currHeartsCount = theHealthHolder.transform.childCount;
                Debug.Log("Need to delete health ");
                Debug.Log("currHeartsCount is " + currHeartsCount);
                Debug.Log("heartsToDestroy is " + heartsToDestroy);

                for (int i = currHeartsCount - 1; i >= heartsToDestroy; i--)
                {
                    Destroy(theHealthHolder.transform.GetChild(i).gameObject);
                }
            }
            else
            {
                Debug.Log("Health is already of that value " + changedHealth);
            }
        }
        else if (changedHealth == minPlayerHealth)
        {
            playerHealth = changedHealth;
            Destroy(theHealthHolder.transform.GetChild(0).gameObject);
            Debug.Log("Instantiate theDeath");
            restartTheGame();
        }
        else
        {
            Debug.Log("Health is out of boundaries");
        }
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
            StopCoroutine(inkCoroutine);
            thePlayerOver.InkDeath();
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
