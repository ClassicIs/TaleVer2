using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerManager : MonoBehaviour
{
    private int health;
    private int inkLevel;
    private int maxHealth;
    private int maxInkLevel;
    
    private int coinCount;
    private Player thePlayerScript;
    private Rigidbody2D thePlayerBody;

    [SerializeField]
    private GameObject theGameManager;
    private GameManagerScript GMScript;
    [SerializeField]
    private GameObject restartMenu;
    private CharacterOverlap charCollisionScript;
    private BoxCollider2D theBoxCol;
    private CircleCollider2D theCircleTriggerCol;
    private Animator playerAnimator;

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = 4;
        health = maxHealth;

        coinCount = 0;

        theBoxCol = GetComponent<BoxCollider2D>();
        theCircleTriggerCol = GetComponent<CircleCollider2D>();
        thePlayerBody = GetComponent<Rigidbody2D>();
        GMScript = theGameManager.GetComponent<GameManagerScript>();
        GMScript.ChangeHealth(health);
        GMScript.ChangeMoney(coinCount);
        charCollisionScript = GetComponent<CharacterOverlap>(); ;
        thePlayerScript = GetComponent<Player>();
        playerAnimator = GetComponent<Animator>();
    }

    public void ToChangeHealth(int theChangedHealth)
    {
        if ((health + theChangedHealth) <= maxHealth && (health + theChangedHealth) > 0)
        {
            health -= theChangedHealth;
            GMScript.ChangeHealth(health);
        }
        else
        {
            Debug.Log("Health is at maximum value.");
        }
    }

    void changedHealth(int curHealth)
    {
        health = curHealth;
        GMScript.ChangeHealth(health);
    }

    public int GetCurrHealth()
    {
        return health;
    }

    public void NormalizeAll()
    {
        changedHealth(maxHealth);
        charCollisionScript.InstadeathTime = 0f;
        
        ToChangeCoins(0);
        
        theBoxCol.enabled = true;
        theCircleTriggerCol.enabled = true;
        thePlayerScript.enabled = true;
        charCollisionScript.enabled = true;
    }

    public void ToChangeCoins(int newCoin)
    {
        coinCount += newCoin;
        GMScript.ChangeMoney(coinCount);        
    }

    public void ToChangeCoins(int newCoin, GameObject collision)
    {
        coinCount += newCoin;
        GMScript.ChangeMoney(coinCount, collision.gameObject.transform.position);        
        Destroy(collision.gameObject);
    }

    private void checkForDeath()
    {
        if (health <= 0)
        {
            theDeath();
        }
    }


    public void theDeath()
    {
        thePlayerBody.velocity = new Vector2(0, 0);
        playerAnimator.SetBool("isMoving", false);
        theBoxCol.enabled = false;
        theCircleTriggerCol.enabled = false;
        thePlayerScript.enabled = false;
        charCollisionScript.enabled = false;
        restartMenu.SetActive(true);
        
    }


}
