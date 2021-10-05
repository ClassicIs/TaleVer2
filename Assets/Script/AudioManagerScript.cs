using UnityEngine;
using UnityEngine.Audio;
using System.Collections;

public class AudioManagerScript : MonoBehaviour
{
    private float speedToFade;
    public AudioClass [] AudioClips;
    private float TheAudioModifier;

    public static AudioManagerScript instance;
    public void SetAudioModifier(float theAudioMod)
    { 
        if(theAudioMod >= 0f || theAudioMod <= 1f)
        {
            
            TheAudioModifier = Mathf.Round(theAudioMod * 10f) / 10f;
            Debug.Log("AudioModifier is: " + TheAudioModifier);
            foreach(AudioClass sound in AudioClips)
            {
                sound.source.volume = sound.volume * TheAudioModifier;
            }
        }
    }
    // Start is called before the first frame update
    void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
            return;
        }

        TheAudioModifier = 1f;
        DontDestroyOnLoad(gameObject);
        foreach(AudioClass sound in AudioClips)
        {
            sound.source = gameObject.AddComponent<AudioSource>();
            sound.source.pitch = sound.pitch;
            sound.source.volume = sound.volume * TheAudioModifier;
            sound.source.clip = sound.theClip;
            sound.source.loop = sound.loopOrNot;
        }
    }

    private void Start()
    {
        speedToFade = 0.03f; 
        Play("MainTheme");
    }

    public void Play(string soundName)
    {
        
        int theClipToPlay = theClipToFind(soundName);
        if(theClipToPlay != 404)
        {
            AudioClips[theClipToPlay].source.Play();
        }
        else
        {
            Debug.LogError("Sound " + soundName + " not found.");
        }
    }

    public bool isPlaying(string soundName)
    {
        foreach(AudioClass sound in AudioClips)
        {
            if(sound.name == soundName)
            {
                return sound.source.isPlaying;
            }
        }
        Debug.LogError("There is no sound " + soundName + " in sounds.");
        return true;
    }

    public void StartFromFade (string soundName)
    {
        foreach(AudioClass sound in AudioClips)
        {
            if(sound.name == soundName)
            {               
                StartCoroutine(MakeLoud(sound.source));
                //Debug.Log("Is transitioning " + soundName + " now.");
                return;
            }            
        }
        
        Debug.LogError("Sound " + soundName + " not found.");
        
    }

    public void Stop(string soundName)
    {
        foreach(AudioClass sound in AudioClips)
        {
            if(sound.name == soundName)
            {
                StartCoroutine(MakeQuiet(sound.source));                
            }
        }
    }

    private int theClipToFind(string Name)
    {
        int toReturn = 0;
        foreach (AudioClass sound in AudioClips)
        {            
            if (sound.name == Name)
            {
                return toReturn;
            }
            toReturn++;
        }
        return 404;
        
    }

    private IEnumerator MakeQuiet(AudioSource theSourceToMake)
    {
        while (theSourceToMake.volume >= 0.1f)
        {
            theSourceToMake.volume = Mathf.MoveTowards(theSourceToMake.volume, 0f, speedToFade);
            yield return null;
        }
        theSourceToMake.Stop();
    }

    private IEnumerator MakeLoud(AudioSource theSourceToMake)
    {

        theSourceToMake.Play();
        while (!Mathf.Approximately(theSourceToMake.volume, 1f))
        {
            theSourceToMake.volume = Mathf.MoveTowards(theSourceToMake.volume, 1f, speedToFade);
            yield return null;
        }
        theSourceToMake.volume = 1f;
        
    }

}
