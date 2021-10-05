using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class menuScr : MonoBehaviour
{
    [SerializeField]
    private GameObject mainMenu;
    [SerializeField]
    private GameObject opMenu;

    [SerializeField]
    private GameObject theBlackPanel;
    private Image theBlackBG;

    private void Start()
    {
        
        theBlackBG = theBlackPanel.GetComponent<Image>();
    }

    public void continueB()
    {
        theBlackPanel.SetActive(true);
        StartCoroutine(toFadeInCoroutine());
    }

    IEnumerator toFadeInCoroutine()
    {
        float theOpacity = 0;
        while (theOpacity < 0.99f)
        {
            theOpacity += Time.deltaTime;

            Color tmp = theBlackBG.color;
            tmp.a = theOpacity;

            theBlackBG.color = tmp;
            yield return null;
        }
        SceneManager.LoadScene("Level 1");
    }

    IEnumerator toFadeOutCoroutine()
    {
        float theOpacity = 1;
        while(theOpacity > 0.01f)
        {
            theOpacity -= Time.deltaTime;

            Color tmp = theBlackBG.color;
            tmp.a = theOpacity;
            
            theBlackBG.color = tmp;
            yield return null;
        }        
    }

    public void optionsB()
    {
        mainMenu.SetActive(false);
        opMenu.SetActive(true);
    }

    public void backToMenuB()
    {
        mainMenu.SetActive(true);
        opMenu.SetActive(false);
    }
    
    
}
