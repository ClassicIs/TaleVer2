using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FadeInScript : MonoBehaviour
{
    private Image theBlackBG;
    public event Action CoroutineEnd;

    public static FadeInScript instance;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            //Debug.Log()
            Debug.Log("Destroying gameobject");
            Destroy(gameObject);
            return;
        }

        DontDestroyOnLoad(gameObject);
    }

    private void Start()
    {
        /*
        foreach(Transform child in transform)
        {
            child.gameObject.SetActive(true);
        }*/
        theBlackBG = GetComponentInChildren<Image>();
    }

    public void Fade(bool fade)
    {
        if (fade)
        {
            //Debug.LogError("Starting fading in.");
            StartCoroutine(toFadeInCoroutine(false));
        }
        else
        {
            //Debug.Log("Starting fading out.");
            StartCoroutine(toFadeOutCoroutine());
        }
    }

    IEnumerator toFadeInCoroutine(bool withFadeOut)
    {
        theBlackBG.enabled = true;
        float theOpacity = 0;
        while (theOpacity < 0.99f)
        {
            theOpacity += Time.deltaTime;

            Color tmp = theBlackBG.color;
            tmp.a = theOpacity;

            theBlackBG.color = tmp;
            yield return null;
        }
        if(CoroutineEnd != null)
        {
            CoroutineEnd();
        }
        if (withFadeOut)
        {
            StartCoroutine(toFadeOutCoroutine());
        }
    }

    IEnumerator toFadeOutCoroutine()
    {
        float theOpacity = 1;
        while (theOpacity > 0.01f)
        {
            theOpacity -= Time.deltaTime;

            Color tmp = theBlackBG.color;
            tmp.a = theOpacity;

            theBlackBG.color = tmp;
            yield return null;
        }
        theBlackBG.enabled = false;
    }
}
