using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerEffectsScript : MonoBehaviour
{
    private TrailRenderer theTrail;
    private float timeForTrail;
    public bool endTrail;
    private GhostScript ghost;
    [SerializeField]
    private GameObject theGhost;
    public bool makeTheGhost;
    SpriteRenderer SpriteRenderer;


    private void Awake()
    {
        theTrail = GetComponent<TrailRenderer>();
        ghost = GetComponent<GhostScript>();
        SpriteRenderer = GetComponent<SpriteRenderer>();
    }

    void Start()
    {       
        //For trail
        timeForTrail = 3f;
        endTrail = false;
        
        makeTheGhost = false;
    }

    public void MakeGhost(Vector3 endPos)
    {
        if(Vector3.Distance(transform.position, endPos) < 0.5f)
        {
            return;
        }
        StartCoroutine(SpawnGhosts(endPos));
    }
    

    private IEnumerator SpawnGhosts(Vector3 endPos)
    {
        float t = 0;
        Vector3 startPos = transform.position;
        Vector3 posToSpawn;
        Color curGhostColor;
        while (t <= 1f)
        {
            t += 0.1f;
            //Debug.LogFormat("T = {0}", t);
            posToSpawn = Vector3.Lerp(startPos, endPos, t);

            GameObject curGhost = Instantiate(theGhost, posToSpawn, Quaternion.identity);
            //SpriteRenderer ghostSpriteRenderer = curGhost.GetComponent<SpriteRenderer>();
            //ghostSpriteRenderer.sprite = SpriteRenderer.sprite;

            curGhostColor = new Color(t, 1 - t, 1 - t/2, 1);
            string ghostName = "Ghost # " + t;
            curGhost.GetComponent<SingleGhostScript>().BeAGhost(SpriteRenderer.sprite, curGhostColor, ghostName);
            //Destroy(curGhost, 1f);
            yield return new WaitForSeconds(0.01f);
        }

        //Debug.Log("End of ghost spawn");
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
