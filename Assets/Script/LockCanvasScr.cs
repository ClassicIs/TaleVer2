using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LockConvasScr : QTEObject
{
    [SerializeField]
    private GameObject thePoint;
    [SerializeField]
    private GameObject thePlayer;
    private RectTransform thePointTransform;
    private RectTransform thePlayerTransform;

    private Vector3 theRotation;

    [SerializeField]
    int countToWin;
    [SerializeField]
    int countToFail;

    int posOfPl;
    int dirOfRot;
    bool hasEnd;

    [SerializeField]
    int tail;

    void Start()
    {
        thePointTransform = thePoint.GetComponent<RectTransform>();
        thePlayerTransform = thePlayer.GetComponent<RectTransform>();
        TurnSprites(false);
    }

    public void Activate(int theCountToWin = 3, int theCountToFail = 5)
    {
        countToWin = theCountToWin;
        countToFail = theCountToFail;

        theRotation = new Vector3(0f, 0f, 0f);
        posOfPl = 0;
        dirOfRot = 1;
        hasEnd = false;

        TurnSprites(true);
        StartCoroutine(TheCapsule());
    }

    void EndOfLock()
    {
        thePlayerTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        thePointTransform.rotation = Quaternion.Euler(new Vector3(0, 0, 0));
        TurnSprites(false);
    }

    void TurnSprites(bool OnOrOff)
    {
        Image[] theSprites = gameObject.GetComponentsInChildren<Image>();
        foreach (Image theSpriteTMP in theSprites)
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
            thePlayerTransform.rotation = Quaternion.Euler(0, 0, posOfPl);
            if (Input.GetKeyDown(KeyCode.K))
            {
                int currPlayerRot = Mathf.RoundToInt(Mathf.Rad2Deg * thePlayerTransform.rotation.z);

                Debug.Log("currPlayerRot" + currPlayerRot);
                int currCapsuleRot = Mathf.RoundToInt(Mathf.Rad2Deg * thePointTransform.rotation.z);
                if ((Mathf.Abs(currPlayerRot) <= (currCapsuleRot + tail)) && (Mathf.Abs(currPlayerRot) >= (currCapsuleRot - tail)))
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
                    if (countToFail == 0)
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
                    thePointTransform.rotation = Quaternion.Euler(theRotation);
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
