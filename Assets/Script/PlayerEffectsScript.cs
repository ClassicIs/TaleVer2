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

    
    public void MakeTheGhosts(Vector2 currPos, Vector2 NeedPosition)
    {
        ghost.MakeTheGhost();

    }

    private void ToMakeTrail()
    {
        if (timeForTrail >= 0)
        {
            if (endTrail)
            {
                theTrail.time = Mathf.Lerp(theTrail.time, 0, .1f);

                if (theTrail.time <= 0)
                {
                    theTrail.enabled = true;
                }

            }
        }
        else
        {
            timeForTrail = 3f;
            theTrail.enabled = false;
            theTrail.time = 0.53f;
            endTrail = false;
        }
    }
}
