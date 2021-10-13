using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsScript : MonoBehaviour
{
    private TrailRenderer theTrail;
    private float timeForTrail;
    public bool endTrail;
    private GhostScript ghost;
    public bool makeTheGhost;

    void Start()
    {       
        //For trail
        timeForTrail = 3f;
        endTrail = false;
        theTrail = GetComponent<TrailRenderer>();
        ghost = GetComponent<GhostScript>();
        makeTheGhost = false;
    }

    
    public void MakeTheGhosts(bool endOrStart)
    {
        if (endOrStart)
        {
            ghost.enabled = true;
            
        }
        else
        {
            ghost.enabled = false;
        }
    }
}
