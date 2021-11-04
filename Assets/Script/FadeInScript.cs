using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class FadeInScript : MonoBehaviour
{
    private Image theBlackBG;
    public event Action CoroutineEnd;

    private void Start()
    {
        theBlackBG = GetComponentInChildren<Image>();
    }
    private void Awake()
    {
        DontDestroyOnLoad(gameObject);
    }

    public IEnumerator toFadeInCoroutine(bool withFadeOut)
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

    public IEnumerator toFadeOutCoroutine()
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
