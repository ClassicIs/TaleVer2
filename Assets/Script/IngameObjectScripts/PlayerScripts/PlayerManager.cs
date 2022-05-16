using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerManager : MonoBehaviour
{
    // Health Values
    private int Health;    
    private int MaxHealth;

    // Ink Level Values
    private int InkLevel;
    private int MaxInkLevel;

    // Coins Count
    private int CoinCount;    
    private int MaxCoins = 10000;

    private Player thePlayerScript;    
    
    // References to Collisions
    private CharacterOverlap charCollisionScript;
    private BoxCollider2D theBoxCol;
    private CircleCollider2D theCircleTriggerCol;
    

    // Actions to coordinate Activity of Classes
    public event Action<int> OnCoinChange;
    public event Action<int> OnHealthChange;
    public event Action<int> OnInkLevelChange;

    // Death Actions
    public event Action OnInkDeath;
    public event Action OnDeath;

    private IEnumerator Debuf;

    // Start is called before the first frame update
    void Start()
    {
        thePlayerScript = GetComponent<Player>();
        SetValues(4, 100, 0);

        AsignValues();
    }
    
    private void DangerInTheWay(DangerObject DangerObject)
    {
        if(DangerObject.LongAction)
        {
            Debuf = TakeDamageInTime(DangerObject.HealthDamage, DangerObject.InkGain, DangerObject.SlowModifier);
            StartCoroutine(Debuf);
            DangerObject.OnBeingFree += DangerAway;
        }
        if (DangerObject.MakeStun)
        {
            charCollisionScript.Stunned();
            DangerObject.OnBeingFree += DangerAway;
        }
       
    }

    private void DangerAway(int HealthDamage, int InkGain, float SlowModifier, float TimeForDebuf)
    {
        if (Debuf != null)
        {
            StopCoroutine(Debuf);
            Debuf = null;
        }

        if (charCollisionScript.isStunned)
        {
            charCollisionScript.Unstunned();
        }
        if (SlowModifier == 0)
        {
            if (!thePlayerScript.isSlowDown)
            {
                thePlayerScript.SlowEffectOff();
            }
        }
        else
        {
            TakeDamageInTime(HealthDamage, InkGain, SlowModifier, TimeForDebuf);
        }
    }

    public void TakeDamage(int HealthDamage, int InkGain, float SlowModifier)
    {
        int TheHealthDamage = HealthDamage;
        int TheInkGain = InkGain;
        float TheSlowModifier = SlowModifier;
        
        if (HealthDamage != 0)
        {
            AddHealth(HealthDamage);
        }
        if (InkGain != 0)
        {
            AddInkLevel(InkGain);
        }
        if (SlowModifier != 0)
        {
            if (!thePlayerScript.isSlowDown)
            {
                thePlayerScript.SlowEffectOn(SlowModifier);
            }
        }
    }

    private IEnumerator TakeDamageInTime(int HealthDamage, int InkGain, float SlowModifier)
    {
        while (true)
        {
            TakeDamage(HealthDamage, InkGain, SlowModifier);
            yield return new WaitForSeconds(1);
        }
    }

    private IEnumerator TakeDamageInTime(int HealthDamage, int InkGain, float SlowModifier, float TimeForDebuf)
    {
        float TheTimeForDebuf = TimeForDebuf;
        while (TheTimeForDebuf <= 0)
        {
            TheTimeForDebuf -= Time.deltaTime;
            TakeDamage(HealthDamage, InkGain, SlowModifier);
            yield return new WaitForSeconds(1);
        }
        DangerAway(0, 0, 0, 0);
    }


    // Start functions
    public void SetValues(int Health, int InkLevel, int Coins, int MaxHealth = 4, int MaxInkLevel = 100)
    {
        this.MaxHealth = MaxHealth;
        this.MaxInkLevel = MaxInkLevel;

        this.Health = Health;
        CoinCount = Coins;
        this.InkLevel = InkLevel;

        if (OnCoinChange != null)
        {
            OnCoinChange(CoinCount);
        }
        if (OnHealthChange != null)
        {
            OnHealthChange(Health);
        }
        if (OnInkLevelChange != null)
        {
            OnInkLevelChange(InkLevel);
        }
}

    public void SetValues(SavePoint PointToLoad)
    {
        /*this.MaxHealth = MaxHealth;
        this.MaxInkLevel = MaxInkLevel;
        */
        Vector2 PlayerPosition = new Vector2();
        List<String> Inventory;
        PointToLoad.ReturnPoint(out Health, out InkLevel, out CoinCount, out PlayerPosition, out Inventory);
        transform.position = PlayerPosition;
    }

    private void AsignValues()
    {
        theBoxCol = GetComponent<BoxCollider2D>();
        theCircleTriggerCol = GetComponent<CircleCollider2D>();
        //thePlayerBody = GetComponent<Rigidbody2D>();
        //GMScript = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManagerScript>();
        charCollisionScript = GetComponent<CharacterOverlap>();
        thePlayerScript = GetComponent<Player>();
        //playerAnimator = GetComponent<Animator>();
    }

    public void NormalizeAll()
    {
        theBoxCol.enabled = true;
        theCircleTriggerCol.enabled = true;
        thePlayerScript.enabled = true;
        charCollisionScript.enabled = true;
    }

    // To Set Values
    public void AddHealth(int AddHealth)
    {
        StateOfValue HealthValue = CheckInRange(out Health, Health, AddHealth, 0, MaxHealth);
        if(HealthValue == StateOfValue.InRange)
        {
            Debug.Log("Health is changed.");
        }
        else if(HealthValue == StateOfValue.Less)
        {
            Debug.Log("Health is zero. Player died.");
            if (OnDeath != null)
            {
                OnDeath();
            }
        }
        else
        {
            Debug.Log("Health is already at maximum. Player is full.");
        }

        if (OnHealthChange != null)
        {
            OnHealthChange(Health);
        }
    }

    public void AddInkLevel(int AddInk)
    {
        StateOfValue InkValue = CheckInRange(out InkLevel, InkLevel, AddInk, 0, MaxInkLevel);
        if(InkValue == StateOfValue.InRange)
        {
            Debug.Log("Ink Level changed.");
        }
        else if(InkValue == StateOfValue.Less)
        {
            if(OnInkDeath != null)
            {
                OnInkDeath();
            }
        }
        else
        {
            Debug.Log("Ink Level is maximum.");
        }

        if (OnInkLevelChange != null)
        {
            OnInkLevelChange(InkLevel);
        }
    }

    public void MinimumInkLevel()
    {
        InkLevel = MaxInkLevel;
        Debug.Log("Ink Level is " + InkLevel + ".");        
    }

    public void AddCoins(int AddCoins)
    {
        StateOfValue CoinsValue = CheckInRange(out CoinCount, CoinCount, AddCoins, 0, MaxCoins);
        OnCoinChange(CoinCount);
        if (CoinsValue == StateOfValue.Less)
        {
            Debug.Log("Coins cannot go less than zero. They are already null.");
        }
        else if(CoinsValue == StateOfValue.More)
        {
            Debug.Log("Coins cannot go more than" + MaxCoins + ". You are already rich!");
        }
        else
        {
            Debug.Log("Coins are added. New coins count is " + CoinCount + ".");
        }            
    }

    public void SetSlowModifier(float DebufTime = 0, float SlowModifier = 0.6f)
    {
        if(DebufTime == 0)
        {

        }
    }

    // Service Functions
    enum StateOfValue
    {
        More, 
        Less,
        InRange
    }

    private StateOfValue CheckInRange(out int CurrentValue, int Value, int AddValue, int MinValue, int MaxValue)
    {
        StateOfValue StateOfTheValue;

        int TMPValue = Value + AddValue;

        if ((MinValue  < TMPValue) && (TMPValue < MaxValue))
        {
            StateOfTheValue = StateOfValue.InRange;
            CurrentValue = TMPValue;
        }
        else if (TMPValue >= MaxValue)
        {
            StateOfTheValue = StateOfValue.More;
            CurrentValue = MaxValue;            
        }
        else
        {
            StateOfTheValue = StateOfValue.Less;
            CurrentValue = MinValue;
        }

        return StateOfTheValue;
    }

    // To Get Values
    public int GetHealth()
    {
        return Health;
    }

    public int GetCoins()
    {
        return CoinCount;
    }

    public int GetInkLevel()
    {
        return InkLevel;
    }

    public void GetAllValues(out int Health, out int InkLevel, out int CoinCount)
    {
        Health = this.Health;        
        InkLevel = this.InkLevel;
        CoinCount = this.CoinCount;
    }
    
    /*
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
    }*/
}
