using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class GameManagerScript : MonoBehaviour
{
    public SaveManager SaveManagement;

    private GameObject player;
    private PlayerOtherInput AllOtherInput;
    private PlayerManager PlayerManager;
    private Player Player;
    private CharacterOverlap thePlayerOver;

    [SerializeField]
    private GameObject[] allGameObj;

    [SerializeField]
    private MenuScript MenuScript;

    [SerializeField]
    private InventoryScript PlayerInventory;
    [SerializeField]
    private UIInventoryScript UIInventory;

    InteractObject ObjToInteract = null;
    DangerObject ObjectDanger = null;

    public static GameManagerScript gmInstance;    

    public List<SpawnObjects> destroyedObj;
    private FadeInScript theFadeInScr;

    private Vector2 startPos;
    [SerializeField]
    [Range(0, 100)]
    private int inkLevelLoss;
    [SerializeField]
    float timeToDecreaseInk;
    
    bool notEndOfLevel = true;
    bool inventoryOpen;

    public event Action<int> OnHealthChange;


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

    private void Start()
    {        
        PlayerInventory = PlayerManager.Inventory;
        inventoryOpen = false;
        UIInventory.SetInventory(PlayerManager.Inventory);
    }

    public void AssigningValues()
    {
        player = GameObject.FindGameObjectWithTag("Player");
        PlayerManager = player.GetComponent<PlayerManager>();
        PlayerInventory = PlayerManager.Inventory;

        GameObject Canvas = GameObject.FindGameObjectWithTag("AllCanvas");
        MenuScript = Canvas.GetComponentInChildren<MenuScript>();
        UIInventory = Canvas.GetComponentInChildren<UIInventoryScript>();
        
        SaveManagement = GameObject.FindGameObjectWithTag("SaveManager").GetComponent<SaveManager>();
        theFadeInScr = GameObject.FindGameObjectWithTag("Fade").GetComponent<FadeInScript>();

        
        Player = player.GetComponent<Player>();
        thePlayerOver = Player.GetComponent<CharacterOverlap>();
        AllOtherInput = Player.GetComponent<PlayerOtherInput>();

        destroyedObj = new List<SpawnObjects>();
    }

    private void EnterAllActions()
    {
        AllOtherInput.OnEscape += IfMenu;
        AllOtherInput.OnInteracting += IfInteracted;

        // Leave
        thePlayerOver.OnFalling += changeHealthWhenFalling;
        
        thePlayerOver.OnSaving += IfSave;
        thePlayerOver.OnTakingCoin += IfTookCoin;

        thePlayerOver.OnDangerousObject += IfEnteredDanger;
        thePlayerOver.OnDangerFar += IfExitedDanger;

        thePlayerOver.OnNearInterObject += IfInteractedNear;
        thePlayerOver.OnFarInterObject += IfInteractedAway;
        thePlayerOver.OnPickableObject += IfPickableNear;
        thePlayerOver.OnEnteredQTE += IfEnteredQTE;
    }

    private void Normalize()
    {        
        startPos = player.transform.position;
        currDeath = DeathTypes.none;
    }

    void Update()
    {        
        if(Input.GetKeyDown(KeyCode.I))
        {
            inventoryOpen = !inventoryOpen;
            Debug.LogFormat("Inventory open is {0}", inventoryOpen);
            Player.ToStun(inventoryOpen);
            UIInventory.ShowInventoryUI();
        }
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
                Player.ToStun(true);

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
    
    private void IfEnteredQTE(AutoQTEScript theScript)
    {
        Player.ToStun(true);
        theScript.StartOfQTE();
        theScript.OnQTEEnd += IfEndedQTE;
    }

    private void IfEndedQTE()
    {
        Player.ToStun(false);
    }

    private void IfPickableNear(PickableObject PickableObject)
    {
        PickableObject.thisItem.PrintItem();
        if (PlayerInventory.AddItem(PickableObject.thisItem))
        {
            PickableObject.OnPlayerCollision();
        }
    }

    void InteractionEnded()
    {
        if (ObjToInteract != null)
        {
            //Debug.Log("Null the Actions!");
            AllOtherInput.OnInteracting -= ObjToInteract.FutherAction;
            ObjToInteract.OnEndOfInteraction -= InteractionEnded;
            AllOtherInput.OnInteracting += IfInteracted;
            ObjToInteract = null;
        }

        Player.ToStun(false);
        Debug.Log("Interation has ended!");
    }

    private void IfEnteredDanger(DangerObject ObjectThatDanger)
    {
        Debug.Log("Entered danger");
        ObjectDanger = ObjectThatDanger;
        PlayerManager.DangerInTheWay(ObjectThatDanger);
        ObjectThatDanger.OnBeingFree += PlayerManager.DangerAway;
    }

    private void IfExitedDanger(DangerObject ObjectThatDanger)
    {
        ObjectThatDanger.Freedom();
    }

    private void IfSave()
    {
        SaveManagement.MakeASave();
    }

    private void IfEndOfLevel()
    {
        SaveManagement.MakeASave();
        
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
