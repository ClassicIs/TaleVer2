using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
public class ScriptForUITables : MonoBehaviour
{
    Image[] imageElements;
    TextMeshProUGUI[] texts;
    bool isActive;
    FadeInScript FadeInScript;
    [SerializeField]
    GameObject menuObject;
    private void Awake()
    {
        FadeInScript = GameObject.FindObjectOfType<FadeInScript>();
        isActive = false;
        imageElements = GetComponentsInChildren<Image>();
        texts = GetComponentsInChildren<TextMeshProUGUI>();
    }

    // Start is called before the first frame update
    void Start()
    {
        TurnMenuOn(false);
    }

    public void MenuOn(bool on)
    {
        FadeInScript.Fade(on);
        TurnMenuOn(on);
        StartCoroutine(CheckEscape());
    }

    IEnumerator CheckEscape()
    {
        while (true)
        {
            if (Input.GetKeyDown(KeyCode.Escape))
            {
                menuObject.SetActive(true);
                TurnMenuOn(false);
                FadeInScript.Fade(false);
                break;
            }
            yield return null;
        }
        //Debug.Log("End credits.");
    }


    private void TurnMenuOn(bool on)
    {
        foreach(Image imageElement in imageElements)
        {
            imageElement.enabled = on;
        }
        foreach(TextMeshProUGUI text in texts)
        {
            text.enabled = on;
        }
    }

}
