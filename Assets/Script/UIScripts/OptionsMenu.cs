using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour, IMenu
{

    Slider theMusicSlider;
    float musicModifier;
    [SerializeField]
    //GameObject OptionsHolder;

    private void Start()
    {
        theMusicSlider = GetComponentInChildren<Slider>();
    }

    public void OnSliderChange()
    {
        musicModifier = theMusicSlider.value;
        FindObjectOfType<AudioManagerScript>().SetAudioModifier(musicModifier);
    }

    public void MenuOn(bool on)
    {
        Debug.LogFormat("Options ", on);
        gameObject.SetActive(on);
    }
}
