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
    private Slider healthSlider;
    [SerializeField]
    private Text moneyCount;

    [SerializeField]
    private GameObject thePlayerObj;
    private Player thePlayerScr;
    [SerializeField]
    private GameObject theMenu;


    private bool menuActive;

    // Start is called before the first frame update
    void Start()
    {        
        destroyedObj = new List<SpawnObjects>();
        thePlayerScr = thePlayerObj.GetComponent<Player>();        
        menuActive = false;
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

    public void RestartGame()
    {
        Debug.Log("Restarting the game!");
        Debug.Log("deactiveObjs.Count " + destroyedObj.Count);

        /*for(int i = 0; i < destroyedObj.Count; i++)
        {
            Debug.Log("\ntheObjToSpawn.nameOfPrefab: " + destroyedObj[i].nameOfPrefab);
            for (int j = 0; j < allGameObj.Length; j++)
            {
                Debug.Log("theObjectZero.name: " + allGameObj[j].name + "\ntheObjToSpawn.nameOfPrefab: " + destroyedObj[i].nameOfPrefab);
                if (destroyedObj[i].nameOfPrefab == allGameObj[j].name)
                {
                    Instantiate(allGameObj[j], destroyedObj[i].thePosition, Quaternion.identity);
                    continue;
                }
            }
        }*/

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

    public void ChangeHealth(int currHealth)
    {
        Debug.Log("currHealth: " + currHealth);
        Debug.Log("(currHealth - theHealthHolder.transform.childCount)" + (currHealth - theHealthHolder.transform.childCount));
        Debug.Log("theHealthHolder.transform.childCount" + theHealthHolder.transform.childCount);

        if (theHealthHolder.transform.childCount < currHealth)
        {
            int heartsToAdd = (currHealth - theHealthHolder.transform.childCount);
            Debug.Log("Health is less ");
            for (int j = 0; j < heartsToAdd; j++)
            {
                Debug.Log("(currHealth - theHealthHolder.transform.childCount): " + (currHealth - theHealthHolder.transform.childCount));
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

    public void changeInkLevel(int currInkLevel)
    {

    }

    public void ChangeMoney(int currMoney, Vector2 posOftheCollidedObj)
    {
        moneyCount.text = currMoney.ToString();
        addNewDestroyedObj("Coin", posOftheCollidedObj);
    }

    public void ChangeMoney(int currMoney)
    {
        moneyCount.text = currMoney.ToString();
    }
}
