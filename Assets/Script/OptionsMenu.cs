using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class OptionsMenu : MonoBehaviour
{
    Slider theMusicSlider;
    float musicModifier;
    private void Start()
    {
        theMusicSlider = GetComponentInChildren<Slider>();
    }
    // Start is called before the first frame update
    public void OnSliderChange()
    {
        musicModifier = theMusicSlider.value;
        FindObjectOfType<AudioManagerScript>().SetAudioModifier(musicModifier);
    }

}
