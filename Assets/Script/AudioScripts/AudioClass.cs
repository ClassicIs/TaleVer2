using UnityEngine;
using UnityEngine.Audio;

[System.Serializable]
public class AudioClass
{
    public enum statesOfPlay
    {
        SFX,
        Music
    };

    public statesOfPlay thePlayStates;

    public string name;
    public float volume;
    public AudioSource source;
    public AudioClip theClip;
    public float pitch;    
    public bool loopOrNot;
    //public isItMusicOrSfx = thePlayStates;
}
