﻿using System.Collections;
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
    private IEnumerator LongActionDebuf;

    // Start is called before the first frame update
    void Start()
    {
        thePlayerScript = GetComponent<Player>();
        AsignValues();
        SetValues(5, 100, 10001);
    }
    
    private void DangerInTheWay(DangerObject DangerObject, bool LongAction)
    {
        if(LongAction)
        {
            LongActionDebuf = TakeDamageInTime(DangerObject.StartDamage);
            StartCoroutine(LongActionDebuf);
            DangerObject.OnBeingFree += DangerAway;
        }
        else
        {
            TakeDamage(DangerObject.StartDamage);
        }       
    }

    private void DangerAway(Damage DangerObject)
    {
        if (LongActionDebuf != null)
        {
            StopCoroutine(LongActionDebuf);
            LongActionDebuf = null;
        }
        TakeDamage(DangerObject);
    }

    private void TakeDamage(Damage DangerObject)
    {
        int TheHealthDamage;
        int TheInkGain;
        float TheSlowModifier;
        bool MakeStun;
        float TimeForDebuf;

        DangerObject.GiveDamage(out TheHealthDamage, out TheInkGain, out TheSlowModifier, out MakeStun, out TimeForDebuf);

        if (TimeForDebuf > 0f)
        {
            
            if (TheHealthDamage != 0)
            {
                AddHealth(TheHealthDamage);
            }
            if (TheInkGain != 0)
            {
                AddInkLevel(TheInkGain);
            }
            if (TheSlowModifier != 0)
            {
                if (!thePlayerScript.isSlowDown)
                {
                    thePlayerScript.SlowEffectOn(TheSlowModifier);
                }
            }
        }
    }

    private IEnumerator TakeDamageInTime(Damage DangerObject)
    {        
        while (true)
        {
            TakeDamage(DangerObject);
            yield return new WaitForSeconds(1);
        }
    }
    /*
    private IEnumerator TakeDamageInTime(Damage DangerObject)
    {
        float TheTimeForDebuf = TimeForDebuf;
        while (TheTimeForDebuf <= 0)
        {
            TheTimeForDebuf -= Time.deltaTime;
            TakeDamage(HealthDamage, InkGain, SlowModifier);
            yield return new WaitForSeconds(1);
        }
        DangerAway(0, 0, 0, 0);
    }*/


    // Start functions
    public void SetValues(int Health, int InkLevel, int Coins, int MaxHealth = 4, int MaxInkLevel = 100)
    {
        Debug.Log("Setting values");
        this.MaxHealth = MaxHealth;
        this.MaxInkLevel = MaxInkLevel;

        AddHealth(Health);
        AddCoins(Coins);
        AddInkLevel(InkLevel);

        /*if (OnCoinChange != null)
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
        }*/
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