using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAudioEffectsScript : MonoBehaviour
{
    private Player thePlayerScript;
    private AudioSource playerAudio;
    // Start is called before the first frame update
    void Start()
    {
        playerAudio = GetComponent<AudioSource>();
        thePlayerScript = GetComponent<Player>();
    }

    // Update is called once per frame
    void Update()
    {
        if(thePlayerScript.isMoving)
        {
            if (!playerAudio.isPlaying)
            {
                playerAudio.Play();
            }
        }
        else
        {
            playerAudio.Stop();
        }
    }
}
