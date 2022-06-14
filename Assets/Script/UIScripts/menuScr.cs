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

    bool coroutineEnd = false;
    [SerializeField]
    FadeInScript theFadeInScr;    

    public void ContinueB()
    {
        StartCoroutine(theFadeInScr.toFadeInCoroutine(true));
        theFadeInScr.CoroutineEnd += StartCurrScene;
    }    

    private void StartCurrScene()
    {        
        theFadeInScr.CoroutineEnd -= StartCurrScene;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);        
    }

    private void StartFromNewScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
    }

    public void OptionsB()
    {
        mainMenu.SetActive(false);
        opMenu.SetActive(true);
    }

    public void BackToMenuB()
    {
        mainMenu.SetActive(true);
        opMenu.SetActive(false);
    }
}
