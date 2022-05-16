using System.Collections;
using System.Collections.Generic;
using UnityEngine;
//using UnityEngine.UI;
using System;

public class GameManagerScript : MonoBehaviour
{
    private SaveManager SaveManagement;
    
    private Player thePlayerScr;
    private PlayerManager PlayerManager;
    private PlayerOtherInput AllOtherInput;

    InteractObject ObjToInteract = null;
    DangerObject ObjectDanger = null;

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
    private GameObject thePlayerObj;
    
    /*[SerializeField]
    private GameObject theMenu;*/
    [SerializeField]
    private MenuScript MenuScript;    

    private FadeInScript theFadeInScr;

    private Vector2 startPos;

    private bool menuActive;

    [SerializeField]
    [Range(0, 100)]
    private int inkLevelLoss;
    [SerializeField]
    float timeToDecreaseInk;
    bool isInInk = false;
    
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
        AssigningValues();
        inkCoroutine = InkStay();
        
        EnterAllActions();
        Normalize();
    }

    private void AssigningValues()
    {
        SaveManagement = GetComponent<SaveManager>();
        theFadeInScr = GameObject.FindGameObjectWithTag("Fade").GetComponent<FadeInScript>();
        PlayerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        thePlayerOver = thePlayerObj.GetComponent<CharacterOverlap>();
        destroyedObj = new List<SpawnObjects>();
        thePlayerScr = thePlayerObj.GetComponent<Player>();
        AllOtherInput = thePlayerObj.GetComponent<PlayerOtherInput>();
    }

    private void EnterAllActions()
    {
        AllOtherInput.OnEscape += IfMenu;
        AllOtherInput.OnInteracting += IfInteracted;


        //Change        
        //thePlayerOver.OnNearLock += IfNearLock;
        //thePlayerOver.OnFarLock += IfFarLock;
        
        // Leave
        thePlayerOver.OnFalling += changeHealthWhenFalling;
        
        thePlayerOver.OnSaving += IfSave;
        thePlayerOver.OnTakingCoin += IfTookCoin;

        thePlayerOver.OnDangerousObject += IfEnteredDanger;
        thePlayerOver.OnNearInterObject += IfInteractedNear;
        thePlayerOver.OnFarInterObject += IfInteractedAway;
        //thePlayerOver.OnEndOfLevel += IfEndOfLevel;
    }

    private void Normalize()
    {
        menuActive = false;
        startPos = thePlayerObj.transform.position;
        isItAfterFall = false;
        currDeath = DeathTypes.none;

        /*maxInklevel = 100;
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
        ChangeMoney(0);*/
    }

    void Update()
    {        
        if (notEndOfLevel)
        {
            //MenuFunc();
        }        
    }
    
    private void LockFunc()
    {
        thePlayerOver.Stunned();/*
        theLockScript.OnFail += NotOpenTheLock;
        theLockScript.OnSuccess += OpenTheLock;
        theLockScript.Activate();*/
    }
    /*
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
    */



    private void NotOpenTheLock()
    {
        thePlayerOver.Unstunned();
        Debug.Log("Box not openned!");
    }

    // TODO Move to Restart Manager

    private void changeHealthWhenFalling()
    {
        currDeath = DeathTypes.afterFall;
        //ChangeHealth(-1);        
    }

    
    private void RestartTheGame()
    {
        Debug.Log("Restarting!");        
        SetAllObjectsBack();
       /*
        tmpMoney = playerMoney;
        //ChangeMoney(0);
        tmpHealth = playerHealth;

        Debug.Log("Tmp health is " + tmpHealth);
        //ChangeHealth(0);
        tmpInkLevel = inkLevel;
        //ChangeInkLevel(0);
       */
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

    private void IfMenu()
    {
        MenuScript.MenuOn();
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

    // Interaction
    
    private void IfInteractedNear(InteractObject InteractObject)
    {
        InteractObject.IFPlayerNear();
    }

    private void IfInteractedAway(InteractObject InteractObject)
    {
        InteractObject.IFPlayerIsAway();
    }

    private void IfInteracted()
    {
        ObjToInteract = thePlayerOver.theNearestObject();
        if (ObjToInteract != null)
        {
            ObjToInteract.InterAction();
            if(ObjToInteract.LongInteraction)
            {
                thePlayerOver.Stunned();

                AllOtherInput.OnInteracting += ObjToInteract.FutherAction;
                AllOtherInput.OnInteracting -= IfInteracted;
                
                ObjToInteract.OnEndOfInteraction += InteractionEnded;
            }
        }
        else
        {
            Debug.LogWarning("Oops! Looks like there's realy nothing to interact with!");
        }
    }

    void InteractionEnded()
    {
        if (ObjToInteract != null)
        {
            Debug.Log("Null the Actions!");
            AllOtherInput.OnInteracting -= ObjToInteract.FutherAction;
            ObjToInteract.OnEndOfInteraction -= InteractionEnded;
            AllOtherInput.OnInteracting += IfInteracted;
            ObjToInteract = null;
        }

        thePlayerOver.Unstunned();
        Debug.Log("Interation has ended!");
    }

    private void IfEnteredDanger(DangerObject ObjectThatDanger)
    {
        Debug.Log("Entered danger");
        ObjectDanger = ObjectThatDanger;        
    }

    private void DangerAway()
    {
        Debug.Log("Danger is away!");
        if (ObjectDanger.LongAction)
        {
            
        }       
    }

    private void OnPressingEscape()
    {

    }

    //To make actions
    /*
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
    */


    private void IfSave()
    {
        SaveManagement.MakeASave(true);
        /*playerHealth = tmpHealth;
        playerMoney = tmpMoney;
        inkLevel = tmpInkLevel;
        destroyedObj.Clear();*/
    }

    private void IfEndOfLevel()
    {
        SaveManagement.MakeASave(true);
        //IfSave();
        StartCoroutine(theFadeInScr.toFadeInCoroutine(false));
        //MenuOn(true, "To be continued...");

    }

    // TODO Change
    private void IfTookCoin(int money, Vector2 pos)
    {
        //ChangeMoney(money, pos);
    }


    //TODO To InkDrop

    private void IfEnteredInk()
    {
        StartCoroutine(inkCoroutine);
    }

    void IfEscapedInk()
    {
        StopCoroutine(inkCoroutine);
    }


    // TODO Change
    private IEnumerator InkStay()
    {
        while (true)
        {
            Debug.Log("Ink level loss is " + inkLevelLoss);
            //ChangeInkLevel(inkLevelLoss);
            PlayerManager.AddInkLevel(-inkLevelLoss);
            yield return new WaitForSeconds(timeToDecreaseInk);
        }
    }
    
}
