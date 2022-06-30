using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CreditsScript : MonoBehaviour, IMenu
{
    
    FadeInScript FadeInScript;
    [SerializeField]
    TextMeshProUGUI[] texts;

    void Start()
    {
        FadeInScript = GameObject.FindGameObjectWithTag("FadeIn").GetComponent<FadeInScript>();
        TextOn(false);
    }
    public void MenuOn(bool on)
    {
        TextOn(on);
    }

    public void TextOn(bool on)
    {
        foreach (TextMeshProUGUI text in texts)
        {
            text.gameObject.SetActive(on);
        }
    }

    
}
