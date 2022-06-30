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
    FadeInScript theFadeInScr;

    private void Awake()
    {
        theFadeInScr = GameObject.FindGameObjectWithTag("FadeIn").GetComponent<FadeInScript>();
    }


    private void Start()
    {
        theFadeInScr.Fade(false);
    }

    public void ContinueB()
    {
        theFadeInScr.Fade(true);
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
