using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LockControllerScript : QTEObject
{
    [SerializeField]
    private GameObject thePoint;
   
    private bool inCapsule;
    [SerializeField]
    private GameObject thePlayer;    
    private Vector3 theRotation;

    [SerializeField]
    int countToWin;
    [SerializeField]
    int countToFail;
    
    int posOfPl;
    int dirOfRot;
    bool hasEnd;
    Vector3 thePlayerPos;

    [SerializeField]
    int tail;

    void Start()
    {
        TurnSprites(false);        
    }
        
    public void Activate(int theCountToWin = 3, int theCountToFail = 5)
    {
        countToWin = theCountToWin;       
        countToFail = theCountToFail;
        theRotation = new Vector3(0f, 0f, 0f);
        posOfPl = 0;
        dirOfRot = 1;

        inCapsule = false;
        hasEnd = false;
        gameObject.transform.position = thePlayerPos;
        StartCoroutine(TheCapsule());
        TurnSprites(true);
    }   

    void EndOfLock()
    {
        thePlayer.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        thePoint.transform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        TurnSprites(false);
    }

    void TurnSprites(bool OnOrOff)
    {
        SpriteRenderer[] theSprites = gameObject.GetComponentsInChildren<SpriteRenderer>();
        foreach (SpriteRenderer theSpriteTMP in theSprites)
        {
            theSpriteTMP.enabled = OnOrOff;
        }
    }

    IEnumerator TheCapsule()
    {
        while (!hasEnd)
        {
            if (Mathf.Abs(posOfPl) >= 360)
            {
                posOfPl = 0;
            }
            else
            {
                posOfPl += 1 * dirOfRot;
            }
            thePlayer.transform.rotation = Quaternion.Euler(0, 0, posOfPl);
            if (Input.GetKeyDown(KeyCode.K))
            {
                int currPlayerRot = Mathf.RoundToInt(Mathf.Rad2Deg * thePlayer.transform.rotation.z);
                
                Debug.Log("currPlayerRot" + currPlayerRot);
                int currCapsuleRot = Mathf.RoundToInt(Mathf.Rad2Deg * thePoint.transform.rotation.z);                
                if ((Mathf.Abs(currPlayerRot) <= (currCapsuleRot +  tail)) && (Mathf.Abs(currPlayerRot) >= (currCapsuleRot - tail)))
                {
                    Debug.Log(("(Mathf.Abs(currPlayerRot):" + Mathf.Abs(currPlayerRot) + "\n" + "currCapsuleRot +  tail" + (currCapsuleRot + tail) + "\n(currCapsuleRot - tail): " + (currCapsuleRot - tail)));
                    countToWin--;
                    Debug.Log("Success!");
                    if (countToWin == 0)
                    {
                        Success();
                        hasEnd = true;                        
                    }
                }
                else
                {
                    countToFail--;
                    Debug.Log("Not success...");
                    if(countToFail == 0)
                    {
                        Failed();
                        Debug.Log("You failed");
                        hasEnd = true;
                    }
                }
                if (!hasEnd)
                {
                    theRotation.z = Random.Range(0, 361);
                    dirOfRot *= -1;
                    thePoint.transform.rotation = Quaternion.Euler(theRotation);
                }

            }
            yield return null;
        }
        EndOfLock();
    }
    protected override void Failed()
    {
        Debug.Log("Failed to open the lock!");
        base.Failed();

    }

    protected override void Success()
    {
        Debug.Log("Successfully openned the lock!");
        base.Success();
    }
}


