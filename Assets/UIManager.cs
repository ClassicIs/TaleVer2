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
        PlayerManager.OnHealthChange += ChangeHealth;
        PlayerManager.OnInkLevelChange += ChangeInkLevelUI;
        PlayerManager.OnCoinChange += ChangeCoinCountUI;        
    }

    private void ChangeHealth(int Health)
    {
        Debug.Log("Changing health " + Health);
        int HealthNow = HealthObject.transform.childCount;
        int HealthChange = Health - HealthNow;
        if(HealthChange > 0)
        {
            for(int i = 0; i < HealthChange; i++)
            {
                Instantiate(HealthObject, HealthUI.transform);
            }
        }
        else if (HealthChange < 0)
        {
            for(int i = HealthNow; i > HealthChange; i--)
            {
                Destroy(HealthUI.transform.GetChild(i));
            }
        }
        else
        {
            Debug.Log("Health is already " + Health + ".");
        }
    }

    private void ChangeInkLevelUI(int InkLevel)
    {
        Debug.Log("Changing int level " + InkLevel);
        //InkLevelDisplay.value = InkLevel;
        InkLevelDisplay.value = Mathf.Lerp(InkLevelDisplay.value, InkLevel, InkChangeSpeed);       
    }

    private void ChangeCoinCountUI(int CoinCount)
    {
        MoneyCount.text = CoinCount.ToString();
    }

    // TODO Implement a Sign
    private void InteractionSignUI()
    {
        CanInteract.SetActive(true);
    }
    private void NoInteractionSignUI()
    {
        CanInteract.SetActive(false);
    }


}
