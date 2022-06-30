using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using System;

public class ButtonManager : MonoBehaviour
{
    [SerializeField]
    CreditsScript CreditsScript;
    [SerializeField]
    ControlsScript ControlsScript;
    [SerializeField]
    OptionsMenu OptionsMenu;
    [SerializeField]
    GameObject menuObject;

    FadeInScript FadeInScript;

    private void Start()
    {
        FadeInScript = GameObject.FindGameObjectWithTag("FadeIn").GetComponent<FadeInScript>();
    }

    public void CloseTheGame()
    {
        Application.Quit();
        Debug.Log("Quit");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f;
        int curScene = SceneManager.GetActiveScene().buildIndex;
        Debug.LogFormat("Scene loading {0}", curScene);
        SceneManager.LoadScene(curScene);
    }
    public void ReturnToMenu()
    {
        Time.timeScale = 1f;
        Debug.Log("Menu is loading...");
        SceneManager.LoadScene(0);
    }


    private void Show(IMenu obj)
    {
        FadeInScript.Fade(true);
        obj.MenuOn(true);
        Action tmpAction = null;
        tmpAction += delegate {
            obj.MenuOn(false);
        };
        StartCoroutine(CheckEscape(tmpAction));
    }

    public void ShowCredits()
    {
        Show((IMenu)CreditsScript);
    }

    public void ShowControls()
    {
        Show((IMenu)ControlsScript);
    }


    IEnumerator CheckEscape(Action OnEscape)
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                if (OnEscape != null)
                {
                    OnEscape();
                }                                
                FadeInScript.Fade(false);
                break;
            }
            yield return null;
        }
    }

}
