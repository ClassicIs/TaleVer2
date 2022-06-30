using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class UIManager : MonoBehaviour
{
    [SerializeField]
    private PlayerManager PlayerManager;
    
    [SerializeField]
    private GameObject HealthObject;

    [SerializeField]
    private GameObject HealthUI;

    [SerializeField]
    private Text MoneyCount;
    [SerializeField]
    private GameObject CanInteract;
    [SerializeField]
    private Slider InkLevelDisplay;
    private float InkChangeSpeed;

    // Start is called before the first frame update
    void Awake()
    {
        InkChangeSpeed = 0.02f;
        PlayerManager = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        InteractionSignUI(false);
        PlayerManager.OnHealthChange += ChangeHealth;
        PlayerManager.OnInkLevelChange += ChangeInkLevelUI;
        PlayerManager.OnCoinChange += ChangeCoinCountUI;
        PlayerManager.OnSettingValues += SettingUI;
    }
    private void Start()
    {
        
    }

    private void SettingUI(int health, float inkLevel, int coinCount)
    {
        ChangeHealth(health);
        InkLevelDisplay.value = inkLevel;
        MoneyCount.text = coinCount.ToString();
    }
    private void ChangeHealth(int Health)
    {
        int HealthNow = HealthUI.transform.childCount;
        int HealthChange = Health - HealthNow;

        Debug.Log("Changing health " + Health + "\nCurrent health is " + HealthNow);

        if (HealthChange > 0)
        {
            for(int i = 0; i < HealthChange; i++)
            {
                Instantiate(HealthObject, HealthUI.transform);
            }
        }
        else if (HealthChange < 0)
        {
            for(int i = HealthNow; i > Health; i--)
            {
                Debug.Log("Destroying heart " + i);
                Destroy(HealthUI.transform.GetChild(i - 1).gameObject);
            }
        }
        else
        {
            Debug.Log("Health is already " + Health + ".");
        }
    }

    private void ChangeInkLevelUI(int inkLevel)
    {
        if (inkLevel < 0)
            return;
        //Debug.Log("Changing ink level " + inkLevel);
        StartCoroutine(ChangeInkLevelUIInTime(InkLevelDisplay.value, inkLevel));
    }

    private IEnumerator ChangeInkLevelUIInTime(float currentInkLevel, float neededInkLevel)
    {
        while(Mathf.Abs(currentInkLevel - neededInkLevel) > 0.3f)
        {
            currentInkLevel = Mathf.Lerp(currentInkLevel, neededInkLevel, InkChangeSpeed);
            InkLevelDisplay.value = currentInkLevel;
            //Debug.LogFormat("Current Ink Level {0}", currentInkLevel);
            yield return null;
        }
        InkLevelDisplay.value = neededInkLevel;
        //Debug.Log("Changed ink level.");
    }

    private void ChangeCoinCountUI(int coinCount)
    {
        StartCoroutine(ChangeCoinCountInTime(int.Parse(MoneyCount.text), coinCount));
    }

    private IEnumerator ChangeCoinCountInTime(int curCoinCount, int needCoinCount)
    {
        while(curCoinCount != needCoinCount)
        {
            if (curCoinCount < needCoinCount)
            {
                curCoinCount += 1;
            }
            else
            {
                curCoinCount -= 1;
            }
            MoneyCount.text = curCoinCount.ToString();
            yield return new WaitForSeconds(0.2f);
        }
    }

    // TODO Implement a Sign
    private void InteractionSignUI(bool canInteract)
    {
        CanInteract.SetActive(canInteract);
    }


}
